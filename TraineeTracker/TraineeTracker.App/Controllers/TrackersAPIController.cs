using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;

namespace TraineeTracker.App.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TrackersAPIController : ControllerBase
    {
        private readonly TraineeTrackerContext _context;
        private readonly UserManager<Spartan> _userManager;

        public TrackersAPIController(TraineeTrackerContext context, UserManager<Spartan> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/TrackersAPI
        [HttpGet("trackers")]
        public async Task<ActionResult<IEnumerable<Tracker>>> GetTrackerItems()
        {
          if (_context.TrackerItems == null)
          {
              return NotFound();
          }
            return await _context.TrackerItems.ToListAsync();
        }

        // GET: api/TrackersAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tracker>> GetTracker(int id)
        {
          if (_context.TrackerItems == null)
          {
              return NotFound();
          }
            var tracker = await _context.TrackerItems.FindAsync(id);

            if (tracker == null)
            {
                return NotFound();
            }

            return tracker;
        }

        // PUT: api/TrackersAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [NonAction]
        public async Task<IActionResult> PutTracker(int id, Tracker tracker)
        {
            if (id != tracker.Id)
            {
                return BadRequest();
            }

            _context.Entry(tracker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrackerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TrackersAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [NonAction]
        public async Task<ActionResult<Tracker>> PostTracker(Tracker tracker)
        {
          if (_context.TrackerItems == null)
          {
              return Problem("Entity set 'TraineeTrackerContext.TrackerItems'  is null.");
          }
            _context.TrackerItems.Add(tracker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTracker", new { id = tracker.Id }, tracker);
        }

        // DELETE: api/TrackersAPI/5
        [NonAction]
        public async Task<IActionResult> DeleteTracker(int id)
        {
            if (_context.TrackerItems == null)
            {
                return NotFound();
            }
            var tracker = await _context.TrackerItems.FindAsync(id);
            if (tracker == null)
            {
                return NotFound();
            }

            _context.TrackerItems.Remove(tracker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TrackerExists(int id)
        {
            return (_context.TrackerItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
