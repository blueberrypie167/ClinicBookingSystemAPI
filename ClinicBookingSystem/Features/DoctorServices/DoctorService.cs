using AutoMapper;
using Common.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Features.DoctorServices
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
        public async Task<DoctorDTO> CreateDoctor(string username)
        {
            // check if requested user to become a doctor exists in the first place
            var user = await _userRepository.GetUserAsync(username);

            if(user is null)
            {
                throw new KeyNotFoundException("User Not Found.");
            }

            var existing = await _doctorRepository.GetDoctorByUserIdAsync(user.userId);
            if (existing is not null)
            {
                throw new InvalidOperationException("Doctor profile already exists for this user.");
            }
            var doctorEntity = new Doctor
            {
                userId = user.userId,
                Is_Active = true
            };

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
    }
}
