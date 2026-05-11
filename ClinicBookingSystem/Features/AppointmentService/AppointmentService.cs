using AutoMapper;
using ClinicBookingSystem.Features.DoctorServices;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ClinicBookingSystem.Features.AppointmentService
{
    public class AppointmentService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IMapper _mapper;

        public AppointmentService(IUserRepository userRepository, IDoctorRepository doctorRepository, ISlotRepository slotRepository, IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDTO> BookTimeslotAsync(CreateAppointmentDTO timeslotDto, Guid patientId)
        {
            var timeslot = await _slotRepository.GetTimeslotAsync(timeslotDto.timeslotId);

            if (timeslot is null)
            {
                throw new NotFoundException($"Timeslot with ID {timeslotDto.timeslotId} was not found.");
            }

            if (!timeslot.IsAvailable())
            {
                throw new ConflictException("Timeslot is already booked.");
            }

            var doctorName = await _doctorRepository.GetUsernameByDoctorIdAsync(timeslot.doctorId);
            
            var patientName = await _userRepository.GetUserByIdAsync(patientId);

            if (patientName is null)
            {
                throw new NotFoundException("Patient not found.");
            }

            var newAppointment = new Appointment(timeslotDto.timeslotId, patientId, timeslot.doctorId, timeslotDto.Notes, patientName.Username, doctorName)
            {
                timeslotId = timeslotDto.timeslotId
            };

            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _appointmentRepository.CreateAppointmentAsync(newAppointment);
                timeslot.MarkAsBooked();
                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync();

                return _mapper.Map<AppointmentDTO>(newAppointment);
            }
            catch (DbUpdateConcurrencyException)
            {
                await tx.RollbackAsync();
                throw new ConflictException("This timeslot was booked by another request. Please try again.");
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<AppointmentDTO> CancelAppointment(int appointmentId, Guid patientId)
        {
            var appointment = await _appointmentRepository.GetAppointmentAsync(appointmentId);

            if (appointment is null)
            {
                throw new NotFoundException("Appointment was not found.");
            }

            var timeslot = await _slotRepository.GetTimeslotAsync(appointment.timeslotId);

            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                appointment.MarkAsCancelled(patientId);

                if (timeslot is not null)
                {
                    timeslot.MarkAsAvailable(); 
                }

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync();

                return _mapper.Map<AppointmentDTO>(appointment);
            }
            catch (DbUpdateConcurrencyException)
            {
                await tx.RollbackAsync();
                throw new ConflictException("The appointment was modified by another request.");
            }
            catch (ForbiddenException)
            {
                await tx.RollbackAsync();
                throw;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsByUserId(Guid userId)
        {           
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync(userId);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }

        public async Task<List<AppointmentDTO>> GetAppointmentsForDoctorAsync(Guid userDoctorId)
        {
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userDoctorId);

            if (doctor is null)
            {
                throw new NotFoundException("Doctor profile was not found for this user");
            }

            var appointments = await _appointmentRepository.GetAppointmentsByDoctorIdAsync(doctor.doctorId);
            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }
    }
}
