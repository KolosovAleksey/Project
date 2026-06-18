using Microsoft.AspNetCore.Authorization;
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

namespace EventPortal.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminEventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminEventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Events
        public async Task<IActionResult> Index()
        {
            var events = await _context.Events
                .Include(e => e.Category)
                .Include(e => e.Registrations)
                .ToListAsync();

            var totalRegistrations = events.Sum(e => e.Registrations.Count(r => r.Status == "confirmed"));

            var viewModel = new AdminEventsIndexViewModel
            {
                Events = events.Select(e => new AdminEventViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    CategoryName = e.Category.Name,
                    StartDateTime = e.StartDateTime,
                    Status = e.Status,
                    RegistrationsCount = e.Registrations.Count(r => r.Status == "confirmed")
                }).ToList(),
                TotalEvents = events.Count,
                TotalRegistrations = totalRegistrations,
                RegistrationsByCategory = events
                    .GroupBy(e => e.Category.Name)
                    .ToDictionary(g => g.Key, g => g.Sum(e => e.Registrations.Count(r => r.Status == "confirmed")))
            };

            return View(viewModel);
        }

        // GET: /Admin/Events/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "Name");
            return View();
        }

        // POST: /Admin/Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventCreateEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = new Event
                {
                    Title = model.Title,
                    CategoryId = model.CategoryId,
                    VenueId = model.VenueId,
                    StartDateTime = model.StartDateTime,
                    EndDateTime = model.EndDateTime,
                    Capacity = model.Capacity,
                    Status = model.Status,
                    Description = model.Description
                };

                _context.Events.Add(ev);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "Name", model.VenueId);
            return View(model);
        }

        // GET: /Admin/Events/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev == null)
                return NotFound();

            var model = new EventCreateEditViewModel
            {
                Id = ev.Id,
                Title = ev.Title,
                CategoryId = ev.CategoryId,
                VenueId = ev.VenueId,
                StartDateTime = ev.StartDateTime,
                EndDateTime = ev.EndDateTime,
                Capacity = ev.Capacity,
                Status = ev.Status,
                Description = ev.Description
            };

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", ev.CategoryId);
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "Name", ev.VenueId);
            return View(model);
        }

        // POST: /Admin/Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EventCreateEditViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var ev = await _context.Events.FindAsync(id);
                if (ev == null)
                    return NotFound();

                ev.Title = model.Title;
                ev.CategoryId = model.CategoryId;
                ev.VenueId = model.VenueId;
                ev.StartDateTime = model.StartDateTime;
                ev.EndDateTime = model.EndDateTime;
                ev.Capacity = model.Capacity;
                ev.Status = model.Status;
                ev.Description = model.Description;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", model.CategoryId);
            ViewBag.Venues = new SelectList(_context.Venues, "Id", "Name", model.VenueId);
            return View(model);
        }

        // POST: /Admin/Events/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id);
            if (ev != null)
            {
                _context.Events.Remove(ev);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Events/Participants/5
        public async Task<IActionResult> Participants(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            var viewModel = new ParticipantsViewModel
            {
                EventTitle = ev.Title,
                Participants = ev.Registrations
                    .Where(r => r.Status == "confirmed" || r.Status == "attended")
                    .Select(r => new ParticipantViewModel
                    {
                        FullName = r.User.FullName,
                        Email = r.User.Email,
                        RegisteredAt = r.RegisteredAt,
                        RegistrationStatus = r.Status
                    }).ToList()
            };

            return View(viewModel);
        }

        // GET: /Admin/Events/ExportCsv/5
        public async Task<IActionResult> ExportCsv(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Registrations)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (ev == null)
                return NotFound();

            var participants = ev.Registrations
                .Where(r => r.Status == "confirmed" || r.Status == "attended")
                .Select(r => new { r.User.FullName, r.User.Email, r.RegisteredAt, r.Status });

            var csv = "ФИО,Email,Дата регистрации,Статус\n";
            foreach (var p in participants)
            {
                csv += $"{p.FullName},{p.Email},{p.RegisteredAt:yyyy-MM-dd HH:mm},{p.Status}\n";
            }

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"Участники_{ev.Title}.csv");
        }
    }
}
