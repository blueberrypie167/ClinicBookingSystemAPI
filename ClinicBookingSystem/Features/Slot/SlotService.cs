using Domain.Entities;
using Common.DTOs;
using Domain.Interfaces;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Features.Slot
{
    public class SlotService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IUserRepository _userRepository;
        private readonly userDbContext _context;
        private readonly IMapper _mapper;

        public SlotService(ISlotRepository slotRepository, IDoctorRepository doctorRepository, IUserRepository userRepository, userDbContext context, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _doctorRepository = doctorRepository;
            _userRepository = userRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<TimeslotDTO> CreateSlotAsync(TimeslotDTO slotDto, Guid userId)
        {

            // SETUP: default status enum for timeslot

            // Get the doctor from the provided userId
            var doctor = await _doctorRepository.GetDoctorByUserIdAsync(userId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found for the current user.");
            }

            // 1. Map DTO to Domain Entity
            var timeslotEntity = _mapper.Map<Timeslot>(slotDto);

            // Assign doctor ID + defaults
            timeslotEntity.doctorId = doctor.doctorId;
            timeslotEntity.CurrentStatus = TimeslotStatus.Open;

            // 2. Pass the Entity to the Repository
            var createdEntity = await _slotRepository.CreateTimeslotAsync(timeslotEntity);

            // 3. Map back to DTO
            return _mapper.Map<TimeslotDTO>(createdEntity);
        }

        public async Task<List<TimeslotDTO>> GetAllTimeslots(string doctorName)
        {
            // find entity with doctorname, find it's doctorid, find its slots by doctorid 

            var slots = await _context.timeSlots
                .Where(ts => ts.Doctor.User.Username == doctorName)
                .ToListAsync();

            if (!slots.Any())
                throw new Exception("Doctor not found or has no timeslots.");

            return _mapper.Map<List<TimeslotDTO>>(slots);

        }
    }
}
