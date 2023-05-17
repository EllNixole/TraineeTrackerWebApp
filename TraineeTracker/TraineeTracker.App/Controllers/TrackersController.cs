using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TraineeTracker.App.Models.ViewModels;
using TraineeTracker.App.Services;

namespace TraineeTracker.App.Controllers
{
    [Authorize]
    public class TrackersController : Controller
    {
        private readonly ITrackerService _service;

        public TrackersController(ITrackerService service)
        {
            _service = service;
        }

        // GET: Trackers
        [Authorize(Roles = "Trainee, Trainer, Admin")]
        public async Task<IActionResult> Index(string? filter = null)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.GetTrackersAsync(user.Data, _service.GetRole(HttpContext), filter);
            return response.Success ? View(response.Data) : Problem(response.Message);
        }

        // GET: Trackers/Details/5
        [Authorize(Roles = "Trainee, Trainer, Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.GetDetailsAsync(user.Data, id, _service.GetRole(HttpContext));
            return response.Success ? View(response.Data) : Problem(response.Message);
        }

        // GET: Trackers/Create
        [Authorize(Roles = "Trainee, Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trackers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trainee, Admin")]
        public async Task<IActionResult> Create(CreateTrackerVM createTrackerVM)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.CreateTrackerAsync(user.Data, createTrackerVM);
            return response.Success ? RedirectToAction(nameof(Index)) : View(createTrackerVM);
        }

        // GET: Trackers/Edit/5
        [Authorize(Roles = "Trainee, Trainer, Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.GetDetailsAsync(user.Data, id, _service.GetRole(HttpContext));
            return response.Success ? View(response.Data) : Problem(response.Message);
        }

        // POST: Trackers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trainee, Admin")]
        public async Task<IActionResult> Edit(int id, TrackerVM trackerVM)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.EditTrackerAsync(user.Data, id, trackerVM);
            return response.Success ? RedirectToAction(nameof(Index)) : NotFound();
        }

        // POST: Trackers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Trainee, Trainer, Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.DeleteTrackerAsync(user.Data, id);
            return response.Success ? RedirectToAction(nameof(Index)) : Problem(response.Message);
        }

        [Authorize(Roles = "Trainee, Admin")]
        public async Task<IActionResult> UpdateTrackerReviewed(int id, MarkReviewedVM markReviewedVM)
        {
            var user = await _service.GetUserAsync(HttpContext);
            var response = await _service.UpdateTrackerReviewedAsync(user.Data, id, markReviewedVM);
            return response.Success ? RedirectToAction(nameof(Index)) : Problem(response.Message);
        }
    }
}
