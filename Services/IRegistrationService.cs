using EventPortal.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventPortal.Services
{
    public interface IRegistrationService
    {
        Task<RegistrationResult> RegisterAsync(string userId, int eventId);
        Task<bool> CancelAsync(string userId, int eventId);
        Task<List<Registration>> GetUserRegistrationsAsync(string userId);
        Task<List<Registration>> GetEventRegistrationsAsync(int eventId);
        Task<bool> CanRegisterAsync(string userId, int eventId);
    }
}
