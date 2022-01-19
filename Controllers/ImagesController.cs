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
    /// This class is responsible for controlling the Images.
    /// Some of it's responsabilities are:
    /// - define a context<see cref="ImagesController"/>
    /// - show Index <see cref="Index"/>
    /// - exhibit image Details <see cref="Details(int?)"/>
    /// - creating an image <see cref="Create"/>
    /// - saving created image <see cref="Create(Image)"/>
    /// - edit image<see cref="Edit(int?)"/>
    /// - delete image<see cref="Delete(int?)"/>
    /// - confirming deletion <see cref="DeleteConfirmed(int)"/>.
    /// - verify image <see cref="ImageExists(int)"/>.
    /// </summary> 
    public class ImagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The method defines the context of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <returns>Void</returns>
        public ImagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The method defines the Index of the controller
        /// </summary>
        /// <returns>View with listed images</returns>
        // GET: Images
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        /// <summary>
        /// The method returns NotFound if the id is null
        /// otherwise it retrieves an image related to the parameter id
        /// and returns null if the retrieved image is null
        /// or returns a view view with the image if it´s not null
        /// </summary>
        /// <param name="id">An int wich represents the image related to the id</param>
        /// <returns>NotFound if id or image is null, otherwise a view with an image</returns>
        // GET: Images/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        /// <summary>
        /// The method initializes and returns a view
        /// </summary>
        /// <returns>A initialized view</returns>
        // GET: Images/Create
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// The method returns a view with an image which is an argument if the ModalState is invalid
        /// otherwise the image is added to the context and an async task to save the changes is called
        /// then the method returns a RedirectToaction with the name of the index as argument
        /// </summary>
        /// <param name="image">The image to be added to the context or return in the view</param>
        /// <returns>
        /// A view with an image if the ModalState is invalid, otherwise
        /// a RedirectToaction to the index
        /// </returns>
        // POST: Images/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageTitle,ImageData")] Image image)
        {
            if (ModelState.IsValid)
            {
                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        /// <summary>
        /// The method checks if the id passed as argument is null, if so it returns a NotFound
        /// If the id isn't null an async task tries to retrieve an image related to the id
        /// If the retrieved image is null the method returns a NotFound
        /// If the image isn't null then the method returns a view with the retrieved image
        /// The method retrieves an image to be edited.
        /// </summary>
        /// <param name="id">An int wich represents the image related to the id</param>
        /// <returns>Returns a view with the image or NotFound</returns>
        // GET: Images/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        /// <summary>
        /// The method verifies if the image id is the same as the id and then saves it
        /// in the database, otherwise it returns a NotFound
        /// </summary>
        /// <param name="image">The image to be saved to the database</param>
        /// <param name="id">An int wich represents the image related to the id</param>
        /// <returns>A view with the image, Notfound or a RedirectToAtcion</returns>
        // POST: Images/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageTitle,ImageData")] Image image)
        {
            if (id != image.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.Id))
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
            return View(image);
        }

        /// <summary>
        /// The method is responsible for finding and image to be deleted based on it's id
        /// </summary>
        /// <param name="id">The id of the image</param>
        /// <returns>Returns a view with the image or NotFound</returns>
        // GET: Images/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.Id == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        /// <summary>
        /// The method confirms the deletion of the image related to the id
        /// </summary>
        /// <param name="id">The id of the image</param>
        /// <returns>Returns a RedirectToAction</returns>
        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// The method checks if the image related to the id exists
        /// </summary>
        /// <param name="id">The id of the image</param>
        /// <returns>Returns true if the image exists, otherwise returns false</returns>
        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.Id == id);
        }
    }
}
