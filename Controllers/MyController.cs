using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using EventPortal.Models;

namespace EventPortal.Controllers
{
    public class MyController : Controller
    {
        public IActionResult Events()
        {
            var viewModel = new MyEventsViewModel
            {
                Registrations = new List<MyRegistrationViewModel>
                {
                    new MyRegistrationViewModel
                    {
                        Id = 1,
                        EventId = 1,
                        EventTitle = "Тестовая конференция",
                        EventStart = DateTime.Now.AddDays(3),
                        Status = "confirmed",
                        EventStatus = "ongoing"
                    }
                }
            };
            return View(viewModel);
        }
    }
}
