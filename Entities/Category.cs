using System.Collections.Generic;

namespace EventPortal.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<Event> Events { get; set; } = new();
    }
}
