using System.ComponentModel.DataAnnotations;

namespace TraineeTracker.App.Models.ViewModels
{
    public class TrackerVM
    {
        public int Id { get; set; }

        [Range(1, 5)]
        public int TechnicalSkill { get; set; }

        [Range(1, 5)]
        public int SpartanSkill { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? StartDoingText { get; set; }

        [StringLength(500)]
        public string? StopDoingText { get; set; }

        [StringLength(500)]
        public string? ContinueDoingText { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Created")]
        public DateTime DateCreated { get; init; } = DateTime.Now;
    }
}