using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraineeTracker.App.Models
{
    public class Tracker
    {

        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        [StringLength(500)]
        public string? StartDoingText { get; set; }

        [StringLength(500)]
        public string? StopDoingText { get; set; }

        [StringLength(500)]
        public string? ContinueDoingText { get; set; }

        [DisplayName("Reviewed")]
        public bool IsReviewed { get; set; }

        [Range(1, 5)]
        public int TechnicalSkill { get; set; }

        [Range(1, 5)]
        public int SpartanSkill { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public Spartan? Spartan { get; set; }

        [ValidateNever]
        [ForeignKey("Spartan")]
        public string SpartanId { get; set; } = null!;

        
    }
}
