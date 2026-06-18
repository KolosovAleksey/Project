using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: /Events
        public async Task<IActionResult> Index(int? categoryId, string status, string searchTerm)
        {
            var query = _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .Include(e => e.Registrations)
                .AsQueryable();

            if (categoryId.HasValue && categoryId.Value > 0)
                query = query.Where(e => e.CategoryId == categoryId.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(e => e.Status == status);

            if (!string.IsNullOrEmpty(searchTerm))
                query = query.Where(e => e.Title.Contains(searchTerm) || e.Description.Contains(searchTerm));

            var events = await query.ToListAsync();

            var viewModel = new EventsIndexViewModel
            {
                Events = events.Select(e => new EventCardViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    CategoryName = e.Category.Name,
                    VenueName = e.Venue.Name,
                    StartDateTime = e.StartDateTime,
                    EndDateTime = e.EndDateTime,
                    Capacity = e.Capacity,
                    FreePlaces = e.Capacity - e.Registrations.Count(r => r.Status == "confirmed"),
                    Status = e.Status,
                    Description = e.Description
                }).ToList(),
                SearchTerm = searchTerm ?? string.Empty,
                CategoryId = categoryId,
                Status = status ?? string.Empty
            };

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            return View(viewModel);
        }

        // GET: /Events/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Venue)
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var userRegistered = false;
            if (!string.IsNullOrEmpty(userId))
            {
                userRegistered = await _context.Registrations
                    .AnyAsync(r => r.EventId == id && r.UserId == userId && r.Status == "confirmed");
            }

            var viewModel = new EventDetailsViewModel
            {
                Id = ev.Id,
                Title = ev.Title,
                CategoryName = ev.Category.Name,
                VenueName = ev.Venue.Name,
                VenueAddress = ev.Venue.Address,
                StartDateTime = ev.StartDateTime,
                EndDateTime = ev.EndDateTime,
                Capacity = ev.Capacity,
                FreePlaces = ev.Capacity - ev.Registrations.Count(r => r.Status == "confirmed"),
                Status = ev.Status,
                Description = ev.Description,
                UserAlreadyRegistered = userRegistered
            };

            return View(viewModel);
        }

        // POST: /Events/Register/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int eventId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var ev = await _context.Events
                .Include(e => e.Registrations)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (ev == null)
                return NotFound();

            if (ev.Status.ToLower() == "completed")
            {
                TempData["Error"] = "Мероприятие завершено, запись невозможна.";
                return RedirectToAction("Details", new { id = eventId });
            }

            var confirmedCount = ev.Registrations.Count(r => r.Status == "confirmed");
            if (confirmedCount >= ev.Capacity)
            {
                TempData["Error"] = "Свободных мест нет.";
                return RedirectToAction("Details", new { id = eventId });
            }

            var already = await _context.Registrations
                .AnyAsync(r => r.EventId == eventId && r.UserId == userId && r.Status == "confirmed");
            if (already)
            {
                TempData["Error"] = "Вы уже записаны на это мероприятие.";
                return RedirectToAction("Details", new { id = eventId });
            }

            var registration = new Registration
            {
                EventId = eventId,
                UserId = userId,
                RegisteredAt = DateTime.UtcNow,
                Status = "confirmed"
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Вы успешно записаны на мероприятие!";
            return RedirectToAction("Details", new { id = eventId });
        }
    }
}
