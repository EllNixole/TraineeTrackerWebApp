using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;

namespace TraineeTracker.App.Controllers
{
    public class TrackersController : Controller
    {
        private readonly TraineeTrackerContext _context;
        private readonly UserManager<Spartan> _userManager;

        public TrackersController(TraineeTrackerContext context, UserManager<Spartan> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Trackers
        public async Task<IActionResult> Index()
        {
            var traineeTrackerContext = _context.TrackerItems.Include(t => t.Spartan);
            return View(await traineeTrackerContext.ToListAsync());
        }

        // GET: Trackers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TrackerItems == null)
            {
                return NotFound();
            }

            var tracker = await _context.TrackerItems
                .Include(t => t.Spartan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tracker == null)
            {
                return NotFound();
            }

            return View(tracker);
        }

        // GET: Trackers/Create
        public IActionResult Create()
        {
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id");
            return View();
        }

        // POST: Trackers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,StartDoingText,StopDoingText,ContinueDoingText,IsReviewed,TechnicalSkill,SpartanSkill,DateCreated,SpartanId")] Tracker tracker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tracker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id", tracker.SpartanId);
            return View(tracker);
        }

        // GET: Trackers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TrackerItems == null)
            {
                return NotFound();
            }

            var tracker = await _context.TrackerItems.FindAsync(id);
            if (tracker == null)
            {
                return NotFound();
            }
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id", tracker.SpartanId);
            return View(tracker);
        }

        // POST: Trackers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,StartDoingText,StopDoingText,ContinueDoingText,IsReviewed,TechnicalSkill,SpartanSkill,DateCreated,SpartanId")] Tracker tracker)
        {
            if (id != tracker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tracker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackerExists(tracker.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["SpartanId"] = new SelectList(_context.Spartans, "Id", "Id", tracker.SpartanId);
            return View(tracker);
        }

        // POST: Trackers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.TrackerItems == null)
            {
                return Problem("Entity set 'TraineeTrackerContext.TrackerItems'  is null.");
            }
            var tracker = await _context.TrackerItems.FindAsync(id);
            if (tracker != null)
            {
                _context.TrackerItems.Remove(tracker);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackerExists(int id)
        {
          return (_context.TrackerItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
