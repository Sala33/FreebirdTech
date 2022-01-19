using FreebirdTech.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Controllers
{
    /// <summary>
    /// This class is responsible for exhibiting the calendar Index <see cref="IndexAsync"/>.
    /// </summary> 
    [Authorize]
    public class CalendarController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// The method defines the context of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <returns>Void</returns>
        public CalendarController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// The method is used to define an Index for Calendar
        /// </summary>
        /// <returns>Index View of the Calendar</returns>
        public async Task<IActionResult> IndexAsync()
        {
            var hasProfile = await ArtistExistsAsync();

            if (!hasProfile)
                return RedirectToAction("Create", "Artists");

            return View();
        }

        /// <summary>
        /// The method verifies if an Artist exists based on it's ownerId
        /// </summary>
        /// <param name="ownerId"> The owner Id
        /// <returns>The artist searched</returns>
        private async Task<bool> ArtistExistsAsync()
        {
            var owner = await _userManager.GetUserAsync(User);
            return _context.Artists.Any(e => e.OwnerId == owner.Id);
        }
    }
}
