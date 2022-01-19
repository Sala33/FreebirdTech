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
    /// This class is responsible for controlling the mailing list.
    /// Some of it's responsabilities are:
    /// - define a context<see cref="MailingListsController"/>
    /// - show Index <see cref="Index()"/>
    /// - exhibit mailing list Details <see cref="Details(int?)"/>
    /// - creating a mailing list <see cref="Create"/>
    /// - saving created mailing list <see cref="Create(MailingList)"/>
    /// - edit mailing list<see cref="Edit(int?)"/>
    /// - delete mailing list<see cref="Delete(int?)"/>
    /// - confirming deletion <see cref="DeleteConfirmed(int)"/>.
    /// - verify mailing list <see cref="MailingListExists(int)"/>.
    /// </summary> 
    public class MailingListsController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The method defines the context of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <returns>Void</returns>
        public MailingListsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The method asynchronously gets the mailing list 
        /// </summary>
        /// <returns>A view with the mailing list</returns>
        // GET: MailingLists
        public async Task<IActionResult> Index()
        {
            return View(await _context.MailingList.ToListAsync());
        }

        /// <summary>
        /// The method asynchronously gets the details of the mailing list 
        /// </summary>
        /// <param name="id">The id related to the mailing list</param>
        /// <returns>NotFound or a view with the mailing list</returns>
        // GET: MailingLists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailingList = await _context.MailingList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailingList == null)
            {
                return NotFound();
            }

            return View(mailingList);
        }

        /// <summary>
        /// The method creates a view 
        /// </summary>
        /// <returns>An initialized view</returns>
        // GET: MailingLists/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// The method asynchronously save the changes made to the mailing list in the context 
        /// </summary>
        /// <param name="mailingList">The mailingList the be saved in the context</param>
        /// <returns>A view with the mailing list or a RedirectToAction</returns>
        // POST: MailingLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email")] MailingList mailingList)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mailingList);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("EmailInvalid", "Email em formato inválido");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The method asynchronously edits the mailing list 
        /// </summary>
        /// <param name="id">The id of the mailing list</param>
        /// <returns>A view with the mailing list or NotFound</returns>
        // GET: MailingLists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailingList = await _context.MailingList.FindAsync(id);
            if (mailingList == null)
            {
                return NotFound();
            }
            return View(mailingList);
        }

        /// <summary>
        /// The method asynchronously saves the edits in the mailing list if the ModalState is valid
        /// </summary>
        /// <param name="id">The mailingList id</param>
        /// <param name="mailingList">The edited mailingList to be saved in the context</param>
        /// <returns>A view with the mailing list, a RedirectToAction or NotFound</returns>
        // POST: MailingLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email")] MailingList mailingList)
        {
            if (id != mailingList.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mailingList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MailingListExists(mailingList.Id))
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
            return View(mailingList);
        }

        /// <summary>
        /// The method asynchronously deletes the mailing list if it's not null
        /// </summary>
        /// <param name="id">The mailingList id</param>
        /// <returns>A view with the mailing list or NotFound</returns>
        // GET: MailingLists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mailingList = await _context.MailingList
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mailingList == null)
            {
                return NotFound();
            }

            return View(mailingList);
        }

        /// <summary>
        /// The method asynchronously confirms if the deletion of the mailing list
        /// </summary>
        /// <param name="id">The mailingList id</param>
        /// <returns>A RedirectToAction</returns>
        // POST: MailingLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mailingList = await _context.MailingList.FindAsync(id);
            _context.MailingList.Remove(mailingList);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// The method asynchronously checks if the mailing list exists
        /// </summary>
        /// <param name="id">The mailingList id</param>
        /// <returns>The mailing list related to the id in the context</returns>
        private bool MailingListExists(int id)
        {
            return _context.MailingList.Any(e => e.Id == id);
        }
    }
}
