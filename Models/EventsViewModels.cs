using System;
using System.Collections.Generic;

namespace EventPortal.Models
{
    public class EventsIndexViewModel
    {
        public List<EventCardViewModel> Events { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class EventCardViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public int FreePlaces { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class EventDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public string VenueName { get; set; } = string.Empty;
        public string VenueAddress { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public int FreePlaces { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool UserAlreadyRegistered { get; set; }
    }
}
