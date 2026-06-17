using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using EventPortal.Models;

namespace EventPortal.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new EventsIndexViewModel
            {
                Events = new List<EventCardViewModel>
                {
                    new EventCardViewModel
                    {
                        Id = 1,
                        Title = "Тестовая конференция",
                        CategoryName = "IT",
                        VenueName = "Москва, Кремль",
                        StartDateTime = DateTime.Now.AddDays(3),
                        EndDateTime = DateTime.Now.AddDays(3).AddHours(4),
                        Capacity = 100,
                        FreePlaces = 30,
                        Status = "ongoing",
                        Description = "Описание мероприятия для проверки вёрстки."
                    }
                },
                CategoriesSelectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "IT", Value = "1" },
                    new SelectListItem { Text = "Бизнес", Value = "2" }
                }
            };
            return View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var viewModel = new EventDetailsViewModel
            {
                Id = id,
                Title = "Тестовая конференция",
                CategoryName = "IT",
                VenueName = "Москва, Кремль",
                VenueAddress = "ул. Кремль, д. 1",
                StartDateTime = DateTime.Now.AddDays(3),
                EndDateTime = DateTime.Now.AddDays(3).AddHours(4),
                Capacity = 100,
                FreePlaces = 30,
                Status = "ongoing",
                Description = "Подробное описание мероприятия.",
                UserAlreadyRegistered = false
            };
            return View(viewModel);
        }
    }
}
