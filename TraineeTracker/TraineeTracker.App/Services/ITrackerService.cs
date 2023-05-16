using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App.Services
{
	public interface ITrackerService
	{
		Task<ServiceResponse<IEnumerable<TrackerVM>>> GetTrackersAsync();
		Task<ServiceResponse<TrackerVM>> GetDetailsAsync(int? id);
		Task<ServiceResponse<TrackerVM>> CreateTrackerAsync(int? id, CreateTrackerVM createTrackerVM);
		Task<ServiceResponse<TrackerVM>> EditTrackerAsync(int? id, TrackerVM trackerVM);
		Task<ServiceResponse<TrackerVM>> DeleteTrackerAsync(int? id);
		Task<ServiceResponse<Spartan>> GetUserAsync(HttpContext httpContext);
		bool TrackerExists(int id);
	}
}
