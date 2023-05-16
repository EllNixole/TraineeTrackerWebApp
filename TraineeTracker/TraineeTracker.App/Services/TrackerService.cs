using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App.Services
{
	public class TrackerService : ITrackerService
	{
		public Task<ServiceResponse<TrackerVM>> CreateTrackerAsync(int? id, CreateTrackerVM createTrackerVM)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<TrackerVM>> DeleteTrackerAsync(int? id)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<TrackerVM>> EditTrackerAsync(int? id, TrackerVM trackerVM)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<TrackerVM>> GetDetailsAsync(int? id)
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<IEnumerable<TrackerVM>>> GetTrackersAsync()
		{
			throw new NotImplementedException();
		}

		public Task<ServiceResponse<Spartan>> GetUserAsync(HttpContext httpContext)
		{
			throw new NotImplementedException();
		}

		public bool TrackerExists(int id)
		{
			throw new NotImplementedException();
		}
	}
}
