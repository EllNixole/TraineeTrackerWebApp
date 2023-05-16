using Microsoft.AspNetCore.Identity;

namespace TraineeTracker.App.Models
{
    public class Spartan : IdentityUser
    {

        public List<Tracker>? TrackerItems { get; set; }
    }
}
