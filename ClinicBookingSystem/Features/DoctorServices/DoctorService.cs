using AutoMapper;
using ClinicBookingSystem.Features.AppointmentService;
using ClinicBookingSystem.Features.SharedDtos;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Features.DoctorServices
{
    public class DoctorService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DoctorService(ISlotRepository slotRepository, IDoctorRepository doctorRepository, IMapper mapper, IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _slotRepository = slotRepository;
            _doctorRepository = doctorRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }
        // creates a doctor entity for an existing user entity
        public async Task<DoctorDTO> CreateDoctor(CreateDoctorDTO doctor)
        {
            if (doctor is null) { 
                throw new InvalidInputException("Doctor input is required."); 
            }

            // check if requested user to become a doctor exists in the first place
            var user = await _userRepository.GetUserAsync(doctor.Username);

            if(user is null)
            {
                throw new NotFoundException("User Not Found.");
            }

            if (user.IsDoctor())
            {
                throw new ConflictException("Doctor profile already exists for this user.");
            }

            // create doctor object
            var doctorEntity = new Doctor(user.userId, doctor.Specialty, user.Username);

            // use transaction, stages the changes** to the db, to commit at once ALL-OR-NOTHING
            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try { 
                await _userRepository.UpdateUserRole(user.userId, UserRole.Doctor);

                await _doctorRepository.CreateNewDoctor(doctorEntity);

                await _unitOfWork.SaveChangesAsync();

                await tx.CommitAsync();

                return _mapper.Map<DoctorDTO>(doctorEntity);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
            
        }

        public async Task<TimeslotDTO> CreateTimeslotAsync(CreateTimeslotDTO slotDto, Guid userId)
        {
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId);
            if (doctor == null)
            {
                throw new NotFoundException("Doctor not found for the current user.");
            }
            if (slotDto.Starts_At <= DateTime.UtcNow) { 
                throw new BusinessRuleException("Timeslot must be scheduled in the future."); }

            if (slotDto.Duration <= 30) {
                throw new InvalidInputException("Duration must be atleast greater than 30"); 
            }

            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                bool hasOverlap = await _slotRepository.HasOverlappingSlotAsync(doctor.doctorId, slotDto);
                if (hasOverlap)
                {
                    throw new ConflictException("This timeslot conflicts with an existing scheduled slot.");
                }

                var timeslot = new Timeslot(doctor.doctorId, slotDto.Starts_At, slotDto.Duration);
                await _slotRepository.CreateTimeslotAsync(timeslot);
                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync();

                return _mapper.Map<TimeslotDTO>(timeslot);
            }
            catch (DbUpdateConcurrencyException)
            {
                await tx.RollbackAsync();
                throw new ConflictException("This timeslot is already booked.");
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<PagedResult<TimeslotDTO>> GetAllTimeslots(PaginatedDTO paginatedDTO)
        {
            var result = await _slotRepository.GetTimeslotsByDoctorNameAsync(paginatedDTO);

            if (!result.Items.Any())
            {
                throw new NotFoundException("Doctor not found or has no timeslots.");
            }

            return new PagedResult<TimeslotDTO>
            {
                Items = _mapper.Map<IEnumerable<TimeslotDTO>>(result.Items),
                TotalCount = result.TotalCount,
                Page = result.Page,
                PageSize = result.PageSize
            };
        }
        public async Task<List<DoctorDTO>> ViewAllDoctors()
        {
            var doctors = await _doctorRepository.GetAllDoctorsAsync();

            if (!doctors.Any())
            {
                throw new NotFoundException("No doctors found.");
            }

            return _mapper.Map<List<DoctorDTO>>(doctors);
        }

        public async Task<List<DoctorDTO>> ViewDoctorsBySpecialty(Specialty specialty)
        {
            var doctors = await _doctorRepository.GetDoctorsBySpecialtyAsync(specialty);

            if (!doctors.Any())
            {
                throw new NotFoundException("No doctors found.");
            }

            return _mapper.Map<List<DoctorDTO>>(doctors);
        }

        public async Task<DoctorAvailabilityDTO> GetDoctorAvailabilityAsync(Guid doctorId)
        {
            var doctor = await _doctorRepository.GetDoctorByDoctorIdAsync(doctorId);

            if (doctor is null)
            {
                throw new NotFoundException("Doctor not found.");
            }

            var timeslots = await _slotRepository.GetTimeslotsByDoctorIdAsync(doctorId);

            var available = timeslots
                .Where(t => t.CurrentStatus == TimeslotStatus.Open && t.Starts_At > DateTime.UtcNow)
                .OrderBy(t => t.Starts_At)
                .ToList();

            return new DoctorAvailabilityDTO
            {
                doctorId = doctor.doctorId,
                username = doctor.Username,
                specialtyId = (int)doctor.Specialty,
                timeslots = _mapper.Map<List<TimeslotDTO>>(available)
            };
        }
    }
}
