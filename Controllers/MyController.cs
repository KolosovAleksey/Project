using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventPortal.Data;
using EventPortal.Entities;
using EventPortal.Models;
using Microsoft.AspNetCore.Identity;

namespace EventPortal.Controllers
{
    [Authorize]
    public class MyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MyController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /My
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var registrations = await _context.Registrations
                .Include(r => r.Event)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RegisteredAt)
                .ToListAsync();

            var viewModel = new MyEventsViewModel
            {
                Registrations = registrations.Select(r => new MyRegistrationViewModel
                {
                    Id = r.Id,
                    EventId = r.EventId,
                    EventTitle = r.Event.Title,
                    EventStart = r.Event.StartDateTime,
                    Status = r.Status,
                    EventStatus = r.Event.Status
                }).ToList()
            };

            return View(viewModel);
        }

        // POST: /My/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int registrationId)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var registration = await _context.Registrations
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.Id == registrationId && r.UserId == userId);

            if (registration == null)
                return NotFound();

            if (registration.Status != "confirmed")
            {
                TempData["Error"] = "Запись уже отменена или изменена.";
                return RedirectToAction("Index");
            }

            if (registration.Event.Status == "completed")
            {
                TempData["Error"] = "Нельзя отменить запись на завершённое мероприятие.";
                return RedirectToAction("Index");
            }

            registration.Status = "cancelled";
            await _context.SaveChangesAsync();

            TempData["Success"] = "Запись отменена, место освобождено.";
            return RedirectToAction("Index");
        }
    }
}
