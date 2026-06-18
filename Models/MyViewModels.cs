using System;
using System.Collections.Generic;

namespace EventPortal.Models
{
    public class MyEventsViewModel
    {
        public List<MyRegistrationViewModel> Registrations { get; set; }
    }

    public class MyRegistrationViewModel
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; }
        public DateTime EventStart { get; set; }
        public string Status { get; set; }
        public string EventStatus { get; set; }
    }
}
