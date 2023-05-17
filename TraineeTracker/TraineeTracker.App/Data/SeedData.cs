using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TraineeTracker.App.Models;

namespace TraineeTracker.App.Data
{
    public class SeedData
    {
        public static void Initialise(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<TraineeTrackerContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Spartan>>();
            var roleStore = new RoleStore<IdentityRole>(context);


            if (context.Spartans.Any())
            {
                context.Spartans.RemoveRange(context.Spartans);
                context.TrackerItems.RemoveRange(context.TrackerItems);
                context.UserRoles.RemoveRange(context.UserRoles);
                context.Roles.RemoveRange(context.Roles);
                context.SaveChanges();
            }


            var trainer = new IdentityRole
            {
                Name = "Trainer",
                NormalizedName = "TRAINER"
            };
            var trainee = new IdentityRole
            {
                Name = "Trainee",
                NormalizedName = "TRAINEE"
            };
            var admin = new IdentityRole
            {
                Name = "Admin",
                NormalizedName = "ADMIN"
            };


            roleStore
              .CreateAsync(trainer)
              .GetAwaiter()
              .GetResult();
            roleStore
                .CreateAsync(trainee)
                .GetAwaiter()
                .GetResult();
            roleStore
                .CreateAsync(admin)
                .GetAwaiter()
                .GetResult();

            var phil = new Spartan
            {
                UserName = "Phil@spartaglobal.com",
                Email = "Phil@spartaglobal.com",
                EmailConfirmed = true
            };
            var nish = new Spartan
            {
                UserName = "Nish@spartaglobal.com",
                Email = "Nish@spartaglobal.com",
                EmailConfirmed = true,
            };
            var peter = new Spartan
            {
                UserName = "Peter@spartaglobal.com",
                Email = "Peter@spartaglobal.com",
                EmailConfirmed = true
            };

            userManager
                .CreateAsync(phil, "Password1!")
                .GetAwaiter()
                .GetResult();
            userManager
                .CreateAsync(nish, "Password1!")
                .GetAwaiter()
                .GetResult();
            userManager
                .CreateAsync(peter, "Password1!")
                .GetAwaiter()
                .GetResult();

            context.UserRoles.AddRange(new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>
                {
                    UserId = userManager.GetUserIdAsync(phil).Result,
                    RoleId = roleStore.GetRoleIdAsync(admin).Result
                },
                new IdentityUserRole<string>
                {
                    UserId = userManager.GetUserIdAsync(nish).Result,
                    RoleId = roleStore.GetRoleIdAsync(trainer).Result
                },
               new IdentityUserRole<string>
                {
                    UserId = userManager.GetUserIdAsync(peter).Result,
                    RoleId = roleStore.GetRoleIdAsync(trainee).Result
                }

            });



            context.TrackerItems.AddRange(
                new Tracker
                {
                    Title = "Week 1",
                    StartDoingText = "Start it",
                    StopDoingText = "Stop it",
                    ContinueDoingText = "Continue it",
                    IsReviewed = false,
                    Spartan = nish
                },
                new Tracker
                {
                    Title = "Week 4",
                    StartDoingText = "Hello",
                    StopDoingText = "Goodbye",
                    ContinueDoingText = "...",
                    IsReviewed = true,
                    Spartan = peter
                },
                new Tracker
                {
                    Title = "Week 3",
                    StartDoingText = "Sleep",
                    StopDoingText = "Working",
                    ContinueDoingText = "Doing well",
                    IsReviewed = true,
                    Spartan = peter
                },
                new Tracker
                {
                    Title = "Week 10",
                    StartDoingText = "start",
                    StopDoingText = "Stop",
                    ContinueDoingText = "Continue",
                    IsReviewed = false,
                    Spartan = nish
                },
                new Tracker
                {
                    Title = "Week 1",
                    StartDoingText = "it",
                    StopDoingText = "it",
                    ContinueDoingText = "it",
                    IsReviewed = false,
                    Spartan = peter
                }
            );
            context.SaveChanges();
        }
    }
}