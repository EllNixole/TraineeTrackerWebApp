using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;

namespace TraineeTracker.App.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TraineeTrackerContext _context;

        public HomeController(ILogger<HomeController> logger, TraineeTrackerContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Authorize (Roles ="Admin")]
        public async Task<IActionResult> Admin()
        {
            var spartans = await _context.Spartans.ToListAsync();
            var rolesList = new List<string>();
            foreach (var s in spartans)
            {
                var role = Role(s.Id);
                rolesList.Add(role.Result);
            }
            var combinedList = Tuple.Create(spartans, rolesList);
            /*var query = _context.Users
                .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
                .Join(_context.Roles, ur => ur.userRole.RoleId, role => role.Id, (ur, role) => new { ur.user.UserName, role.Name }).Select(t => new { t.UserName, Role = t.Name });*/
            return View(combinedList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<string> Role(string id)
        {
            var spartan = await _context.Spartans.Where(t => t.Id == id).FirstOrDefaultAsync();
            var roleID = await _context.UserRoles.Where(t => t.UserId == spartan.Id).Select(t => t.RoleId).FirstOrDefaultAsync();
            var role = await _context.Roles.Where(t => t.Id == roleID).Select(t => t.Name).FirstOrDefaultAsync();

            return role;
        }
    }
}