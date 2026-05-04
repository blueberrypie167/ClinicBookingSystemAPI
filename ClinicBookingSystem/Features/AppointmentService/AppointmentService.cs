using AutoMapper;
using Common.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace ClinicBookingSystem.Features.AppointmentService
{
    public class AppointmentService
    {
        private readonly ISlotRepository _slotRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;
        public AppointmentService(ISlotRepository slotRepository, IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _slotRepository = slotRepository;
            _unitOfWork = unitOfWork;
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentDTO> BookTimeslotAsync(int timeslotId, string notes, Guid patientId)
        {
            // load timeslot, reject if not open
            var timeslot = await _slotRepository.GetTimeslotAsync(timeslotId);

            if (timeslot is null)
            {
                throw new KeyNotFoundException($"Timeslot with ID {timeslotId} was not found.");
            }

            if (!timeslot.IsAvailable())
            {
                throw new Exception("Timeslot is Booked.");
            }


            // create new appointment, update slot status
            var newAppointment = new Appointment
            {
                timeslotId = timeslotId,
                patientUserId = patientId,
                appointmentStatus = AppointmentStatus.Confirmed,
                Notes = notes,
                Created_At = DateTime.UtcNow
            };

            // save in one transaction
            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                await _appointmentRepository.CreateAppointmentAsync(newAppointment);

                timeslot.MarkAsBooked();

                await _unitOfWork.SaveChangesAsync();

                await tx.CommitAsync();

                return _mapper.Map<AppointmentDTO>(newAppointment);
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
                throw new KeyNotFoundException("Appointment was not found.");
            }
             
            // check if patientid matches patient's id in req
            if(appointment.patientUserId != patientId)
            {
                throw new UnauthorizedAccessException("You are not authorized to cancel this appointment.");
            }

            // make the timeslot available again
            var timeslot = await _slotRepository.GetTimeslotAsync(appointment.timeslotId);

            await using var tx = await _unitOfWork.BeginTransactionAsync();
            try
            {
                appointment.MarkAsCancelled();

                if (timeslot is not null)
                {
                    
                    timeslot.MarkAsAvailable(); 
                }

                await _unitOfWork.SaveChangesAsync();
                await tx.CommitAsync();

                return _mapper.Map<AppointmentDTO>(appointment);
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }
        public async Task<List<AppointmentDTO>> GetAppointmentsByUsername(string username)
        {
            var appointments = await _appointmentRepository.GetAllAppointmentsAsync(username);

            return _mapper.Map<List<AppointmentDTO>>(appointments);
        }
    }
}
