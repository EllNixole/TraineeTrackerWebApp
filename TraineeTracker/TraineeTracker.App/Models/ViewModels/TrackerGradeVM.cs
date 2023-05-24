using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace TraineeTracker.App.Models.ViewModels
{
    public class TrackerGradeVM
    {
        public int Id { get; set; }

        [DisplayName("Grade")]
        [Range(0, 100)]
        public int PercentGrade { get; set; } = 99;
    }
}
