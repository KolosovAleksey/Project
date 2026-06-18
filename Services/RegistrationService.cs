using EventPortal.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPortal.Data;

namespace EventPortal.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly ApplicationDbContext _context;

        public RegistrationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RegistrationResult> RegisterAsync(string userId, int eventId)
        {
            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return RegistrationResult.Fail("Мероприятие не найдено.");

            if (ev.Status == "completed")
                return RegistrationResult.Fail("Мероприятие завершено, запись невозможна.");

            var confirmedCount = ev.Registrations.Count(r => r.Status == "confirmed");
            if (confirmedCount >= ev.Capacity)
                return RegistrationResult.Fail("Свободных мест нет.");

            var already = await _context.Registrations
                .AnyAsync(r => r.EventId == eventId && r.UserId == userId && r.Status == "confirmed");
            if (already)
                return RegistrationResult.Fail("Вы уже записаны на это мероприятие.");

            var registration = new Registration
            {
                EventId = eventId,
                UserId = userId,
                RegisteredAt = DateTime.UtcNow,
                Status = "confirmed"
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            return RegistrationResult.Success();
        }

        public async Task<bool> CancelAsync(string userId, int eventId)
        {
            var registration = await _context.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.EventId == eventId && r.UserId == userId && r.Status == "confirmed");

            if (registration == null)
                return false;

            if (registration.Event.Status == "completed")
                return false;

            registration.Status = "cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Registration>> GetUserRegistrationsAsync(string userId)
        {
            return await _context.Registrations
                .Include(r => r.Event)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();
        }

        public async Task<List<Registration>> GetEventRegistrationsAsync(int eventId)
        {
            return await _context.Registrations
                .Include(r => r.User)
                .Where(r => r.EventId == eventId)
                .ToListAsync();
        }

        public async Task<bool> CanRegisterAsync(string userId, int eventId)
        {
            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null || ev.Status == "completed")
                return false;

            var confirmedCount = ev.Registrations.Count(r => r.Status == "confirmed");
            if (confirmedCount >= ev.Capacity)
                return false;

            var already = await _context.Registrations
                .AnyAsync(r => r.EventId == eventId && r.UserId == userId && r.Status == "confirmed");
            if (already)
                return false;

            return true;
        }
    }
}
