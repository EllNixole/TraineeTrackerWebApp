using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TraineeTracker.App.Models
{
    public class Spartan : IdentityUser
    {
        [StringLength(500)]
        public string? Course { get; set; }
        [StringLength(500)]
        public string? Role { get; set; } = "Trainee";
        [StringLength(500)]
        public string? Stream { get; set; }
        public List<Tracker>? TrackerItems { get; set; }
    }
}
