using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDTO> LoginUserAsync(userDTO request);
        Task<AuthResultDTO> RegisterUserAsync(userDTO request);

        Task SaveChangesAsync();
    }
}
