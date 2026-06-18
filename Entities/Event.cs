using System;
using System.Collections.Generic;

namespace EventPortal.Entities
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public int VenueId { get; set; }
        public Venue Venue { get; set; } = null!;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; } = "announced"; // announced, ongoing, completed
        public string Description { get; set; } = string.Empty;
        public List<Registration> Registrations { get; set; } = new();
    }
}
