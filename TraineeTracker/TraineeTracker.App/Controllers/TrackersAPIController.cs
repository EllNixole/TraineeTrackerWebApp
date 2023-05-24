﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TraineeTracker.App.Data;
using TraineeTracker.App.Models;
using TraineeTracker.App.Models.ViewModels;

namespace TraineeTracker.App.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TrackersAPIController : ControllerBase
    {
        private readonly TraineeTrackerContext _context;
        private readonly IMapper _mapper;

        public TrackersAPIController(TraineeTrackerContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/TrackersAPI
        [HttpGet("trackers")]
        public async Task<ActionResult<IEnumerable<TrackerVM>>> GetTrackerItems()
        {
          if (_context.TrackerItems == null)
          {
              return NotFound();
          }
          return await _context.TrackerItems.Include(t=>t.Spartan).Select(t=>_mapper.Map<TrackerVM>(t)).ToListAsync();
        }

        // GET: api/TrackersAPI/5
        [HttpGet("trackers/{id}")]
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

        // GET: api/Trackers/bycourse
        [HttpGet("trackers/bycourse")]
        public async Task<ActionResult<IEnumerable<TrackerVM>>> GetTrackerItemsByCourse()
        {
            if (_context.TrackerItems == null)
            {
                return NotFound();
            }
            return await _context.TrackerItems.Include(t => t.Spartan).Select(t => _mapper.Map<TrackerVM>(t)).ToListAsync();
        }

        // GET: api/trainees
        [HttpGet("trainees")]
        public async Task<ActionResult<IEnumerable<SpartanDTO>>> GetTraineeItems()
        {
            if (_context.TrackerItems == null)
            {
                return NotFound();
            }
            return await _context.Spartans.Where(t=>t.Role == "Trainee").Select(td => _mapper.Map<SpartanDTO>(td)).ToListAsync();
        }
        // GET: api/trainees/?C#
        [HttpGet("trainees/bycourse")]
        public async Task<ActionResult<IEnumerable<SpartanDTO>>> GetTraineeItemsByCourse()
        {
            if (_context.TrackerItems == null)
            {
                return NotFound();
            }
            return await _context.Spartans.Where(t => t.Role == "Trainee").OrderBy(t => t.Course).Select(td => _mapper.Map<SpartanDTO>(td)).ToListAsync();
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