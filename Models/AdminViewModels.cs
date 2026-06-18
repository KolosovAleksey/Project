using System;
using System.Collections.Generic;

namespace EventPortal.Models
{
    public class AdminEventsIndexViewModel
    {
        public List<AdminEventViewModel> Events { get; set; } = new();
        public int TotalEvents { get; set; }
        public int TotalRegistrations { get; set; }
        public Dictionary<string, int> RegistrationsByCategory { get; set; } = new();
    }

    public class AdminEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public string Status { get; set; } = string.Empty;
        public int RegistrationsCount { get; set; }
    }

    public class EventCreateEditViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public int VenueId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = "announced";
        public string Description { get; set; } = string.Empty;
    }

    public class ParticipantsViewModel
    {
        public string EventTitle { get; set; } = string.Empty;
        public List<ParticipantViewModel> Participants { get; set; } = new();
    }

    public class ParticipantViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime RegisteredAt { get; set; }
        public string RegistrationStatus { get; set; } = string.Empty;
    }
}
