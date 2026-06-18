using System;

namespace EventPortal.Entities
{
    public class Registration
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser User { get; set; } = null!;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "confirmed"; // confirmed, cancelled, attended
    }
}
