using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App.Services
{
    public interface ITrackerService
    {
        Task<ServiceResponse<IEnumerable<TrackerVM>>> GetTrackersAsync(Spartan? spartan, string role = "Trainee", string? filter1 = null);
        Task<ServiceResponse<TrackerVM>> GetDetailsAsync(Spartan? spartan, int? id, string role);
        Task<ServiceResponse<TrackerVM>> CreateTrackerAsync(Spartan? spartan, CreateTrackerVM createTrackerVM);
        Task<ServiceResponse<TrackerVM>> EditTrackerAsync(Spartan? spartan, int? id, TrackerVM trackerVM);
        Task<ServiceResponse<TrackerVM>> UpdateTrackerReviewedAsync(Spartan? spartan, int? id, MarkReviewedVM markCompleteVM);
        Task<ServiceResponse<TrackerVM>> DeleteTrackerAsync(Spartan? spartan, int? id);
        Task<ServiceResponse<Spartan>> GetUserAsync(HttpContext httpContext);
        bool TrackerExists(int id);
        string GetRole(HttpContext httpContext);
    }
}
