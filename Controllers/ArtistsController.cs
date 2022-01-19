using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FreebirdTech.Data;
using FreebirdTech.Models;
using Microsoft.AspNetCore.Authorization;
using FreebirdTech.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace FreebirdTech.Controllers
{    
    /// <summary>
    /// This class is responsible for controlling the artist.
    /// Some of it's responsabilities are:
    /// - define context and user <see cref="ArtistsController"/>
    /// - show Index <see cref="Index"/>
    /// - exhibit artist Details (returns Artist View) <see cref="Details(int?)"/>
    /// - get artist and redirect to creation  <see cref="CreateAsync"/> <see cref="Create(Artist)"/>
    /// - edit Artist attributes <see cref="Edit(int, Artist)"/>
    /// - get current Avatar Image <see cref="GetCurrentAvatarImage(Artist)"/>
    /// - delete Artist <see cref="Delete(int?)"/>
    /// - confirming deletion <see cref="DeleteConfirmed(int)"/>.
    /// </summary> 
    [Authorize]
    public class ArtistsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// The method defines the context and user manager of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <param name="userManager">The UserManager of the controller</param>
        /// <returns>Void</returns>
        public ArtistsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        /// <summary>
        /// The method is responsible for returning a list.
        /// </summary>
        /// <returns>A new view with a list of Artists</returns>
        // GET: Artists
        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(User);

            //return View(await _context.Artists.Where(m => m.OwnerId == user.Id).ToListAsync());
            return RedirectToAction("Create");
        }

        /// <summary>
        /// The method searches for an existing profile to exhibit it's details
        /// </summary>
        /// <param name="id"> The Id of the Artist
        /// <returns>A new view of the Artist selected</returns>
        // GET: Artists/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artists.Include(m => m.AvatarImage)
                                                .FirstOrDefaultAsync(m => m.Id == id);
            if (artist == null)
            {
                return NotFound();
            }

            artist.AvatarImageURI = artist.AvatarImage.DecodeImage();

            return View(artist);
        }

        /// <summary>
        /// The method gets the data necessary for creating a profile
        /// </summary>
        /// <returns>A view of the Artist</returns>
        // GET: Artists/Create
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            var s = user.Id;

            var artist = await _context.Artists.Where(m => m.OwnerId == s).Include(m => m.AvatarImage).FirstOrDefaultAsync();

            if(artist != null)
            {
                return RedirectToAction(nameof(Edit));
            }

            ModelState.AddModelError(string.Empty, "Por favor crie um perfil para utilizar");

            var vm = new Artist()
            {
                OwnerId = user.Id
            };
            return View(vm);
        }

        /// <summary>
        /// The method sends the data necessary for creating a profile
        /// </summary>
        /// <returns>A view of the Index</returns>
        // POST: Artists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Categoria,BriefDescription,Contact,Description,OwnerId,AvatarImageURI")] Artist artist)
        {
            var a = artist;
            if (!ModelState.IsValid) return View(a);

            var img = a.AvatarImageURI.EncodeImage(a.Name, EImageType.AVATAR);

            if(img != null && _context.SaveImageToDatabase(img, out var savedImage))
            {
                artist.AvatarImage = savedImage.Entity;
            }
            else 
            {
                ModelState.AddModelError("Arquivo", "O arquivo da imagem é muito grande.");
                return View(a);
            }

            _context.Add(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// The method edits an existing profile. If it does not exists, redirects to Creation.
        /// </summary>
        /// <returns>A new view of the Artist edited</returns>
        // GET: Artists/Edit
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var artist = await _context.Artists.Where(m => m.OwnerId == user.Id).Include(m => m.AvatarImage).SingleAsync();

            if (artist == null)
            {
                return Redirect("Create");
            }

            artist.AvatarImageURI = artist.AvatarImage.DecodeImage();

            return View(artist);
        }

        /// <summary>
        /// The method posts an edited existing profile. If it does not exists, or if does not belong to the
        /// actual user, throw 404 error.
        /// </summary>
        /// <returns>The edit area with the Artist view</returns>
        // POST: Artists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Categoria,BriefDescription,Contact,Description,OwnerId,AvatarImageURI")] Artist artist)
        {
            var user = await _userManager.GetUserAsync(User);

            if (id != artist.Id || user.Id != artist.OwnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var a = artist;

                    Image image = GetCurrentAvatarImage(a);

                    var img = a.AvatarImageURI.EncodeImage(a.Name, EImageType.AVATAR, image);

                    if (_context.SaveImageToDatabase(img, out var savedImage))
                    {
                        artist.AvatarImage = savedImage.Entity;
                    }
                    else
                    {
                        ModelState.AddModelError("Arquivo", "O arquivo da imagem é muito grande.");
                        return View(a);
                    }

                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return View(artist);
            }
            return View(artist);
        }

        /// <summary>
        /// The method gets the current avatar image
        /// </summary>
        /// <returns>Current Avatar Image</returns>
        private Image GetCurrentAvatarImage(Artist a)
        {
            var entity = _context.Artists.Include(m => m.AvatarImage).AsNoTracking().Where(m => m.Id == a.Id).FirstOrDefault();

            var image = entity?.AvatarImage;

            return image;
        }

        /// <summary>
        /// The method gets the data selected for deletion based in the user Id.
        /// </summary>
        /// <returns>Artist view</returns>
        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _userManager.GetUserAsync(User);

            var artist = await _context.Artists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (artist == null || artist.OwnerId != user.Id)
            {
                return NotFound();
            }

            return View(artist);
        }

        /// <summary>
        /// The method posts the data selected for deletion based in the user Id.
        /// </summary>
        /// <returns>Index view</returns>
        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var artist = await _context.Artists.FindAsync(id);

            if (artist == null || artist.OwnerId != user.Id)
            {
                return NotFound();
            }

            _context.Artists.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// The method verifies if an Artist exists
        /// </summary>
        /// <returns>View of the artist searched</returns>
        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }

        private async Task<bool> ArtistExistsAsync(int id)
        {
            return await _context.Artists.AnyAsync(e => e.Id == id);
        }
    }
}
