using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using EventPortal.Models;

namespace EventPortal.Controllers
{
    public class AdminEventsController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new AdminEventsIndexViewModel
            {
                Events = new List<AdminEventViewModel>
                {
                    new AdminEventViewModel
                    {
                        Id = 1,
                        Title = "Админское мероприятие",
                        CategoryName = "IT",
                        StartDateTime = DateTime.Now.AddDays(3),
                        Status = "ongoing",
                        RegistrationsCount = 5
                    }
                }
            };
            return View(viewModel);
        }

        public IActionResult Create()
        {
            var model = new EventCreateEditViewModel
            {
                CategoriesSelectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "IT", Value = "1" },
                    new SelectListItem { Text = "Бизнес", Value = "2" }
                },
                VenuesSelectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Москва", Value = "1" },
                    new SelectListItem { Text = "СПб", Value = "2" }
                }
            };
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            var model = new EventCreateEditViewModel
            {
                Id = id,
                Title = "Редактируемое мероприятие",
                CategoryId = 1,
                VenueId = 1,
                StartDateTime = DateTime.Now.AddDays(2),
                EndDateTime = DateTime.Now.AddDays(2).AddHours(3),
                Capacity = 50,
                Status = "ongoing",
                Description = "Описание",
                CategoriesSelectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "IT", Value = "1" },
                    new SelectListItem { Text = "Бизнес", Value = "2" }
                },
                VenuesSelectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Москва", Value = "1" },
                    new SelectListItem { Text = "СПб", Value = "2" }
                }
            };
            return View(model);
        }

        public IActionResult Participants(int id)
        {
            var viewModel = new ParticipantsViewModel
            {
                EventTitle = "Тестовое мероприятие",
                Participants = new List<ParticipantViewModel>
                {
                    new ParticipantViewModel { FullName = "Иван Иванов", Email = "ivan@example.com", RegisteredAt = DateTime.Now, RegistrationStatus = "confirmed" },
                    new ParticipantViewModel { FullName = "Петр Петров", Email = "petr@example.com", RegisteredAt = DateTime.Now.AddDays(-1), RegistrationStatus = "confirmed" }
                }
            };
            return View(viewModel);
        }
    }
}
