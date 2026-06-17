using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace EventPortal.Models
{
    public class AdminEventsIndexViewModel
    {
        public List<AdminEventViewModel> Events { get; set; }
    }

    public class AdminEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartDateTime { get; set; }
        public string Status { get; set; }
        public int RegistrationsCount { get; set; }
    }

    public class EventCreateEditViewModel
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; }
        public int VenueId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int Capacity { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> CategoriesSelectList { get; set; }
        public List<SelectListItem> VenuesSelectList { get; set; }
    }

    public class ParticipantsViewModel
    {
        public string EventTitle { get; set; }
        public List<ParticipantViewModel> Participants { get; set; }
    }

    public class ParticipantViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string RegistrationStatus { get; set; }
    }
}
