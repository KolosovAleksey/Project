using EventPortal.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EventPortal.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.EnsureCreated();

            // Роли
            if (!roleManager.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole("admin")).Wait();
                roleManager.CreateAsync(new IdentityRole("participant")).Wait();
            }

            // Пользователи
            if (!userManager.Users.Any())
            {
                var admin = new ApplicationUser { UserName = "admin@example.com", Email = "admin@example.com", FullName = "Admin" };
                userManager.CreateAsync(admin, "Admin123!").Wait();
                userManager.AddToRoleAsync(admin, "admin").Wait();

                var user1 = new ApplicationUser { UserName = "user1@example.com", Email = "user1@example.com", FullName = "User One" };
                userManager.CreateAsync(user1, "User123!").Wait();
                userManager.AddToRoleAsync(user1, "participant").Wait();

                var user2 = new ApplicationUser { UserName = "user2@example.com", Email = "user2@example.com", FullName = "User Two" };
                userManager.CreateAsync(user2, "User123!").Wait();
                userManager.AddToRoleAsync(user2, "participant").Wait();
            }

            // Категории
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "IT" },
                    new Category { Name = "Business" },
                    new Category { Name = "Art" },
                    new Category { Name = "Education" },
                    new Category { Name = "Health" }
                );
                context.SaveChanges();
            }

            // Площадки
            if (!context.Venues.Any())
            {
                context.Venues.AddRange(
                    new Venue { Name = "Moscow Hall", Address = "Moscow, Red Square 1", Capacity = 200 },
                    new Venue { Name = "Tech Space", Address = "Moscow, Tverskaya 15", Capacity = 150 },
                    new Venue { Name = "Business Center", Address = "Moscow, Leninsky 25", Capacity = 100 },
                    new Venue { Name = "Art Loft", Address = "Moscow, Arbat 12", Capacity = 80 },
                    new Venue { Name = "University Hall", Address = "Moscow, Leninskie Gory 1", Capacity = 300 }
                );
                context.SaveChanges();
            }

            // Мероприятия
            if (!context.Events.Any())
            {
                var catIT = context.Categories.First(c => c.Name == "IT");
                var catBusiness = context.Categories.First(c => c.Name == "Business");
                var catArt = context.Categories.First(c => c.Name == "Art");
                var catEdu = context.Categories.First(c => c.Name == "Education");
                var catHealth = context.Categories.First(c => c.Name == "Health");

                var v1 = context.Venues.First(v => v.Name == "Moscow Hall");
                var v2 = context.Venues.First(v => v.Name == "Tech Space");
                var v3 = context.Venues.First(v => v.Name == "Business Center");
                var v4 = context.Venues.First(v => v.Name == "Art Loft");
                var v5 = context.Venues.First(v => v.Name == "University Hall");

                context.Events.AddRange(
                    new Event { Title = "Tech Conference 2026", CategoryId = catIT.Id, VenueId = v1.Id, StartDateTime = DateTime.Now.AddDays(10), EndDateTime = DateTime.Now.AddDays(10).AddHours(8), Capacity = 100, Status = "announced", Description = "Large tech conference" },
                    new Event { Title = "Business Summit", CategoryId = catBusiness.Id, VenueId = v3.Id, StartDateTime = DateTime.Now.AddDays(5), EndDateTime = DateTime.Now.AddDays(5).AddHours(6), Capacity = 80, Status = "ongoing", Description = "Business summit" },
                    new Event { Title = "Art Exhibition", CategoryId = catArt.Id, VenueId = v4.Id, StartDateTime = DateTime.Now.AddDays(-2), EndDateTime = DateTime.Now.AddDays(-1), Capacity = 50, Status = "completed", Description = "Art exhibition" },
                    new Event { Title = "Education Workshop", CategoryId = catEdu.Id, VenueId = v5.Id, StartDateTime = DateTime.Now.AddDays(15), EndDateTime = DateTime.Now.AddDays(15).AddHours(4), Capacity = 60, Status = "announced", Description = "Workshop for teachers" },
                    new Event { Title = "Health Fair", CategoryId = catHealth.Id, VenueId = v2.Id, StartDateTime = DateTime.Now.AddDays(20), EndDateTime = DateTime.Now.AddDays(20).AddHours(5), Capacity = 120, Status = "announced", Description = "Health fair" },
                    new Event { Title = "Startup Pitch", CategoryId = catBusiness.Id, VenueId = v2.Id, StartDateTime = DateTime.Now.AddDays(7), EndDateTime = DateTime.Now.AddDays(7).AddHours(3), Capacity = 40, Status = "ongoing", Description = "Pitch competition" },
                    new Event { Title = "Coding Hackathon", CategoryId = catIT.Id, VenueId = v1.Id, StartDateTime = DateTime.Now.AddDays(12), EndDateTime = DateTime.Now.AddDays(12).AddHours(12), Capacity = 50, Status = "announced", Description = "24-hour hackathon" },
                    new Event { Title = "Design Meetup", CategoryId = catArt.Id, VenueId = v4.Id, StartDateTime = DateTime.Now.AddDays(3), EndDateTime = DateTime.Now.AddDays(3).AddHours(3), Capacity = 30, Status = "ongoing", Description = "Design community meetup" }
                );
                context.SaveChanges();
            }

            // Регистрации
            if (!context.Registrations.Any())
            {
                var user1 = userManager.FindByEmailAsync("user1@example.com").Result;
                var user2 = userManager.FindByEmailAsync("user2@example.com").Result;
                var admin = userManager.FindByEmailAsync("admin@example.com").Result;
                var events = context.Events.ToList();

                if (events.Count >= 4)
                {
                    context.Registrations.AddRange(
                        new Registration { EventId = events[0].Id, UserId = user1.Id, Status = "confirmed" },
                        new Registration { EventId = events[0].Id, UserId = user2.Id, Status = "confirmed" },
                        new Registration { EventId = events[1].Id, UserId = user1.Id, Status = "confirmed" },
                        new Registration { EventId = events[1].Id, UserId = admin.Id, Status = "cancelled" },
                        new Registration { EventId = events[2].Id, UserId = user2.Id, Status = "confirmed" },
                        new Registration { EventId = events[3].Id, UserId = user1.Id, Status = "confirmed" },
                        new Registration { EventId = events[3].Id, UserId = user2.Id, Status = "cancelled" },
                        new Registration { EventId = events[4].Id, UserId = admin.Id, Status = "confirmed" },
                        new Registration { EventId = events[5].Id, UserId = user1.Id, Status = "confirmed" },
                        new Registration { EventId = events[5].Id, UserId = user2.Id, Status = "confirmed" },
                        new Registration { EventId = events[6].Id, UserId = user1.Id, Status = "confirmed" },
                        new Registration { EventId = events[6].Id, UserId = admin.Id, Status = "confirmed" },
                        new Registration { EventId = events[7].Id, UserId = user2.Id, Status = "confirmed" },
                        new Registration { EventId = events[0].Id, UserId = admin.Id, Status = "attended" },
                        new Registration { EventId = events[2].Id, UserId = user1.Id, Status = "attended" }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
