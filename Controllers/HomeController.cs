using FreebirdTech.Data;
using FreebirdTech.Extensions;
using FreebirdTech.Models;
using FreebirdTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Controllers
{
    /// <summary>
    /// This class is responsible for controlling the Home attributes.
    /// Some of it's responsabilities are:
    /// - verify login <see cref="HomeController"/>
    /// - show Index <see cref="IndexAsync"/>
    /// - <see cref="Privacy"/>
    /// - throw error <see cref="Error"/>
    /// </summary> 
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The method defines logger and context of Home Controller.
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <param name="logger">The ILogger of the controller</param>
        /// <returns>Void</returns>
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        /// The method is responsible for getting a list of services
        /// </summary>
        /// <returns>Index view</returns>
        public async Task<IActionResult> IndexAsync()
        {
            var vm = new IndexViewModel
            {
                ExemplosServicos = await _context.GetUnorderedServicosArrayAsync(3)
            };

            return View(vm);
        }


        /// <summary>
        /// The method is responsible for getting a list of services
        /// </summary>
        /// <returns>Index view</returns>
        public IActionResult Privacy()
        {
            return View();
        }


        /// <summary>
        /// The method is responsible for showing a error.
        /// </summary>
        /// <returns>Error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
