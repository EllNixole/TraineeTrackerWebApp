using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App.Services
{
    public class TrackerService : ITrackerService
    {
        private readonly TraineeTrackerContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<Spartan> _userManager;

        public TrackerService(TraineeTrackerContext context, IMapper mapper, UserManager<Spartan> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public async Task<ServiceResponse<TrackerVM>> CreateTrackerAsync(Spartan? spartan, CreateTrackerVM createTrackerVM)
        {
            var response = new ServiceResponse<TrackerVM>();
            if (spartan == null)
            {
                response.Success = false;
                response.Message = "No user found";
                return response;
            }

            try
            {
                var tracker = _mapper.Map<Tracker>(createTrackerVM);
                tracker.Spartan = spartan;
                _context.Add(tracker);
                await _context.SaveChangesAsync();
                return response;
            }
            catch
            {
                response.Success = false;
                response.Message = "Database could not be updated";
            }
            return response;
        }


        public async Task<ServiceResponse<TrackerVM>> DeleteTrackerAsync(Spartan? spartan, int? id)
        {
            var response = new ServiceResponse<TrackerVM>();
            var tracker = await _context.TrackerItems.FindAsync(id);
            if (spartan == null)
            {
                response.Success = false;
                response.Message = "No user found";
                return response;
            }
            if (tracker == null)
            {
                response.Success = false;
                response.Message = "No Tracker Items";
                return response;
            }
            if (tracker.SpartanId == spartan.Id)
            {
                _context.TrackerItems.Remove(tracker);
                await _context.SaveChangesAsync();
                response.Success = true;
                response.Message = "Tracker item removed";
            }
            return response;
        }

        public async Task<ServiceResponse<TrackerVM>> EditTrackerAsync(Spartan? spartan, int? id, TrackerVM trackerVM)
        {
            var response = new ServiceResponse<TrackerVM>();
            if (spartan == null)
            {
                response.Success = false;
                response.Message = "No spartan found";
                return response;
            }

            var spartanOwnerId = await GetSpartanOwnerAsync(id);
            if (id != trackerVM.Id)
            {
                response.Message = "Error updating";
                response.Success = false;
                return response;
            }

            var tracker = _mapper.Map<Tracker>(trackerVM);
            tracker.SpartanId = spartanOwnerId;
            _context.Update(tracker);
            await _context.SaveChangesAsync();
            return response;
        }

        public async Task<ServiceResponse<TrackerVM>> GetDetailsAsync(Spartan? spartan, int? id, string role)
        {
            var response = new ServiceResponse<TrackerVM>();
            if (id == null || _context.TrackerItems == null)
            {
                response.Success = false;
                response.Message = "ID or context not found.";
                return response;
            }
            if (spartan == null)
            {
                response.Success = false;
                response.Message = "No user found";
                return response;
            }
            var tracker = await _context.TrackerItems.FirstOrDefaultAsync(t => t.Id == id);
            if (role == "Trainee" && (tracker == null || tracker.SpartanId != spartan.Id))
            {
                response.Success = false;
                response.Message = "ID null or mismatch.";
                return response;
            }
            response.Data = _mapper.Map<TrackerVM>(tracker);
            return response;

        }

        public async Task<ServiceResponse<IEnumerable<TrackerVM>>> GetTrackersAsync(Spartan? spartan, string role = "Trainee", string? filter = null)
        {
            var response = new ServiceResponse<IEnumerable<TrackerVM>>();
            if (spartan == null)
            {
                response.Success = false;
                response.Message = "Can't find Spartan";
                return response;
            }
            if (_context.TrackerItems == null)
            {
                response.Success = false;
                response.Message = "There no Tracker Items.";
                return response;
            }

            List<Tracker> trackerItems = new List<Tracker>();
            if (role == "Trainee")
            {
                trackerItems = await _context.TrackerItems.Where(t => t.SpartanId == spartan.Id).ToListAsync();
            }
            if (role == "Trainer" || role == "Admin")
            {
                trackerItems = await _context.TrackerItems.Include(s => s.Spartan).ToListAsync();
            }
            if (string.IsNullOrEmpty(filter))
            {
                response.Data = trackerItems.Select(t => _mapper.Map<TrackerVM>(t));
                return response;
            }
            response.Data = trackerItems
                .Where(t => (t.Title.Contains(filter!, StringComparison.OrdinalIgnoreCase)) ||
                (t.Spartan.UserName.Contains(filter!, StringComparison.OrdinalIgnoreCase)))
            .Select(t => _mapper.Map<TrackerVM>(t));
            return response;

        }

        public async Task<ServiceResponse<Spartan>> GetUserAsync(HttpContext httpContext)
        {
            var response = new ServiceResponse<Spartan>();
            var currentUser = await _userManager.GetUserAsync(httpContext.User);
            if (currentUser == null)
            {
                response.Success = false;
                response.Message = "Could not find Spartan";
                return response;
            }
            response.Data = currentUser;
            return response;
        }

        public bool TrackerExists(int id)
        {
            return (_context.TrackerItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<ServiceResponse<TrackerVM>> UpdateTrackerReviewedAsync(Spartan? spartan, int? id, MarkReviewedVM markReviewedVM)
        {
            var response = new ServiceResponse<TrackerVM>();
            if (spartan == null)
            {
                response.Success = false;
                response.Message = "No spartan found";
                return response;
            }
            if (id != markReviewedVM.Id)
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

        public async Task<string> GetSpartanOwnerAsync(int? id)
        {
            return await _context.TrackerItems.Where(td => td.Id == id).Select(td => td.SpartanId).FirstAsync();
        }

        public string GetRole(HttpContext httpContext)
        {
            if (httpContext.User.IsInRole("Admin"))
            {
                return "Admin";
            } else if (httpContext.User.IsInRole("Trainer"))
            {
                return "Trainer";
            }
            else
            {
                return "Trainee";
            }
        }
    }
}
