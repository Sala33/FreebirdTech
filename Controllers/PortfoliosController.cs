using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreebirdTech.Data;
using FreebirdTech.Models;

namespace FreebirdTech.Controllers
{
    /// <summary>
    /// This class is responsible for controlling the porfolios.
    /// Some of it's responsabilities are:
    /// - define context and user <see cref="ArtistsController"/>
    /// - show Index <see cref="Index"/>
    /// - exhibit portfolio Details <see cref="Details(int?)"/>
    /// - Create a View <see cref="Create()"/>
    /// - Create a Portfolio <see cref="Create(Portfolio)"/>
    /// - Retrieves the portfolio to be edited <see cref="Edit(int?)"/>
    /// - Edits a portfolio in the database <see cref="Edit(int, Portfolio)"/>
    /// - Deletes a portfolio <see cref="Delete(int?)"/>
    /// - Checks the deletion of a portfolio <see cref="DeleteConfirmed(int)"/>
    /// - Check if a portfolio exists <see cref="PortfolioExists(int)"/>
    /// </summary> 
    public class PortfoliosController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The method defines the context
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <returns>Void</returns>
        public PortfoliosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The method is responsible for returning a view with list of porfolios
        /// </summary>
        /// <returns>A new view with a list of porfolios</returns>
        // GET: Portfolios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Portfolios.ToListAsync());
        }

        /// <summary>
        /// The method searches for an existing porfolio to exhibit it's details
        /// </summary>
        /// <param name="id"> The Id of the porfolio
        /// <returns>A new view of the porfolio selected</returns>
        // GET: Portfolios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        /// <summary>
        /// The method Creates a view
        /// </summary>
        /// <returns>A created view</returns>
        // GET: Portfolios/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// The method sends the data necessary for creating a portfolio
        /// </summary>
        /// <param name="portfolio"> The portfolio
        /// <returns>A view with the portfolio or a RedirectToAction</returns>
        // POST: Portfolios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PortfolioType,EmbedLink")] Portfolio portfolio)
        {
            if (ModelState.IsValid)
            {
                _context.Add(portfolio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(portfolio);
        }

        /// <summary>
        /// The method finds a portfolio related to the id
        /// </summary>
        /// <param name="id"> The portfolio id
        /// <returns>A view with the portfolio or a NotFound</returns>
        // GET: Portfolios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios.FindAsync(id);
            if (portfolio == null)
            {
                return NotFound();
            }
            return View(portfolio);
        }

        /// <summary>
        /// The method edits a portfolio and saves the changes in the database
        /// </summary>
        /// <param name="id"> The portfolio id
        /// <param name="message"> The portfolio to be sent
        /// <returns>A view with the portfolio, NotFound or a RedirectToAction</returns>
        // POST: Portfolios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PortfolioType,EmbedLink")] Portfolio portfolio)
        {
            if (id != portfolio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(portfolio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PortfolioExists(portfolio.Id))
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
            return View(portfolio);
        }

        /// <summary>
        /// The method deletes a portfolio from the database
        /// </summary>
        /// <param name="id"> The portfolio id
        /// <returns>A view with the portfolio or NotFound</returns>
        // GET: Portfolios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var portfolio = await _context.Portfolios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }

        /// <summary>
        /// The method confirms if a portfolio was deleted
        /// </summary>
        /// <param name="id"> The portfolio id
        /// <returns>A RedirectToAction</returns>
        // POST: Portfolios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var portfolio = await _context.Portfolios.FindAsync(id);
            _context.Portfolios.Remove(portfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// The method confirms if a portfolio exists
        /// </summary>
        /// <param name="id"> The portfolio id
        /// <returns>A portfolio related to the id</returns>
        private bool PortfolioExists(int id)
        {
            return _context.Portfolios.Any(e => e.Id == id);
        }
    }
}
