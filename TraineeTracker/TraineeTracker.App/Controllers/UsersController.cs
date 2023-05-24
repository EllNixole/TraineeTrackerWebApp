using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Versioning;
using System.Data;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;
using TraineeTracker.App.Services;

namespace TraineeTracker.App.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly TraineeTrackerContext _context;
        private readonly UserManager<Spartan> _userManager;
        private readonly IMapper _mapper;

        public UsersController(TraineeTrackerContext context, UserManager<Spartan> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var spartans = _userManager.Users.ToList();
            return View(spartans);
            
        }

        // GET: Trackers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string? id)
        {
            var spartan = _userManager.Users.Where(s => s.Id == id).FirstOrDefault();
            return View(spartan);
        }

        // GET: Trackers/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trackers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Spartan spartan)
        {
            return View();
        }

        // GET: Trackers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string? id)
        {
            var spartan = await _userManager.FindByIdAsync(id);

            if(spartan == null)
            {
                return Problem();
            }
            else
            {
                return View(spartan);
            }
        }

        // POST: Trackers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, Spartan spartan)
        {
            var user = await _userManager.FindByIdAsync(id);
            user.UserName = spartan.UserName;
            await _userManager.RemoveFromRoleAsync(user, user.Role);
            await _userManager.AddToRoleAsync(user, spartan.Role);
            user.Role = spartan.Role;
            user.Course = spartan.Course;
            user.Stream = spartan.Stream;
            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
		}

        // POST: Trackers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }

    }
}