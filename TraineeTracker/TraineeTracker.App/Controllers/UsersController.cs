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
            var userList = new List<SpartanRoleVM>();

            foreach (var user in spartans)
            {
                var id = user.Id;
                var username = user.UserName;
                var roles = await _userManager.GetRolesAsync(user);
                var spartanAndRole = new SpartanRoleVM
                {
                    Id = id,
                    Username = username,
                    Roles = roles.ToList()
                };

                userList.Add(spartanAndRole);
            }
            return View(userList);
            
        }

        // GET: Trackers/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string? id)
        {
            var spartan = _userManager.Users.Where(s => s.Id == id).FirstOrDefault();
            var spartanRoleVM = _mapper.Map<SpartanRoleVM>(spartan);
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
        public async Task<IActionResult> Create(SpartanRoleVM spartanRoleVM)
        {
            
            var user =  _userManager.Users.Where(u => u.Id == spartanRoleVM.Id).FirstOrDefault();
            user.UserName = spartanRoleVM.Username;
            await _userManager.RemoveFromRolesAsync(user, new List<string>() { "Trainee", "Trainer", "Admin" });
            await _userManager.AddToRolesAsync(user, spartanRoleVM.Roles);

            return RedirectToAction(nameof(Index));
        }

        // GET: Trackers/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }

        // POST: Trackers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, SpartanRoleVM spartanRoleVM)
        {
            return View();
        }

        // POST: Trackers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return View();
        }

    }
}