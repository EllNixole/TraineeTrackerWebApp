using TraineeTracker.App.Data;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App.Services
{
	public class TrackerService : ITrackerService
	{
		private readonly TraineeTrackerContext _context;

		public TrackerService(TraineeTrackerContext context)
		{
			_context = context;
		}


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

		public async Task<ServiceResponse<TrackerVM>> UpdateTrackerReviewedAsync(int? id, MarkReviewedVM markReviewedVM)
		{
			var response = new ServiceResponse<TrackerVM>();
			if(id != markReviewedVM.Id)
			{
				response.Success = false;
				response.Message = "Model error";
				return response;
			}
			var tracker = await _context.TrackerItems.FindAsync(id);
			if (tracker == null)
			{
				response.Success = false;
				response.Message = "Cannot find ToDo item";
				return response;
			}
			tracker.IsReviewed = markReviewedVM.IsReviewed;

			await _context.SaveChangesAsync();
			return response;
		}
	}
}
