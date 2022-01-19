using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreebirdTech.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreebirdTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Project.Controllers
{

    /// <summary>
    /// This class is responsible for controlling the Calendar Events.
    /// Some of it's responsabilities are:
    /// - define context <see cref="CalendarEventsController"/>
    /// - query Calendar Events by a time space <see cref="GetEvents"/>
    /// - exhibit artist Details (returns Artist View) <see cref="Details(int?)"/>
    /// - get artist and redirect to creation  <see cref="CreateAsync"/> <see cref="Create(Artist)"/>
    /// - edit Artist attributes <see cref="Edit(int, Artist)"/>
    /// - get current Avatar Image <see cref="GetCurrentAvatarImage(Artist)"/>
    /// - delete Artist <see cref="Delete(int?)"/>
    /// - confirming deletion <see cref="DeleteConfirmed(int)"/>.
    /// </summary> 
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CalendarEventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// The method defines the context of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <returns>Void</returns>
        public CalendarEventsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// The method gets events from a slice of time determined by the User
        /// </summary>
        /// <returns>List of Calendar Events queried</returns>
        // GET: api/CalendarEvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            var artist = await GetArtistAsync();

            if (artist == null) 
                return await _context.CalendarEvent
                            .Where(e => !((e.End <= start) || (e.Start >= end)))
                            .ToListAsync();

            return await _context.CalendarEvent
                            .Include(m => m.Artist)
                            .Where(e => !((e.End <= start) || (e.Start >= end)) && e.Artist == artist)
                            .ToListAsync();
        }

        /// <summary>
        /// The method gets a Calendar Event by it's Id wich can be used to acess and
        /// modify Events in other part of the software
        /// </summary>
        /// <returns>Not found if the Id doesn't exist or Calendar Event query if exists</returns>
        // GET: api/CalendarEvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalendarEvent.FindAsync(id);

            if (calendarEvent == null)
            {
                return NotFound();
            }

            return calendarEvent;
        }

        /// <summary>
        /// The method saves the modified Calendar Event, updating it on the database.
        /// </summary>
        /// <returns>The Created NoContentResult object for the response</returns>
        // PUT: api/CalendarEvents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCalendarEvent(int id, CalendarEvent calendarEvent)
        {
            if (id != calendarEvent.Id)
            {
                return BadRequest();
            }

            _context.Entry(calendarEvent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CalendarEventExists(id))
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

        /// <summary>
        /// The method posts a Calendar Event created
        /// </summary>
        /// <returns>The created Calendar event for the response</returns>
        // POST: api/CalendarEvents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> PostCalendarEvent(CalendarEvent calendarEvent)
        {
            calendarEvent.Artist = await GetArtistAsync();
            _context.CalendarEvent.Add(calendarEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCalendarEvent", new { id = calendarEvent.Id }, calendarEvent);
        }

        /// <summary>
        /// The method deletes the selected Calendar Event based on the id
        /// </summary>
        /// <returns>NoContentResult object that produces an empty
        /// Microsoft.AspNetCore.Http.StatusCodes.Status204NoContent response.</returns>
        // DELETE: api/CalendarEvents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCalendarEvent(int id)
        {
            var calendarEvent = await _context.CalendarEvent.FindAsync(id);
            if (calendarEvent == null)
            {
                return NotFound();
            }

            _context.Entry(calendarEvent).Reference(m => m.Artist).Load();

            var artist = await GetArtistAsync();

            if (artist != calendarEvent.Artist)
                return NotFound();

            _context.CalendarEvent.Remove(calendarEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// The method verifies if the Calendar Event exists
        /// </summary>
        /// <returns>Calendar</returns>
        private bool CalendarEventExists(int id)
        {
            return _context.CalendarEvent.Any(e => e.Id == id);
        }

        private async Task<Artist> GetArtistAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return null;

            var artist = await _context.Artists.Where(m => m.OwnerId == user.Id).FirstOrDefaultAsync();

            return artist;
        }
    }
}
