using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EventPortal.Models
{
    public class EventsIndexViewModel
    {
        public List<EventCardViewModel> Events { get; set; }
        public List<SelectListItem> CategoriesSelectList { get; set; }
    }

    public class EventCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string VenueName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public int FreePlaces { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
    }

    public class EventDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string VenueName { get; set; }
        public string VenueAddress { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public int FreePlaces { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool UserAlreadyRegistered { get; set; }
    }
}
