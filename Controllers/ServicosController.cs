using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreebirdTech.Data;
using FreebirdTech.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using FreebirdTech.Extensions;
using FreebirdTech.ViewModels;

namespace FreebirdTech.Controllers
{
    /// <summary>
    /// This class is responsible for controlling the servico.
    /// Some of it's responsabilities are:
    /// - define context and user <see cref="ServicosController"/>
    /// - show Index <see cref="Index"/>
    /// - exhibit servico Details <see cref="Details(int?)"/>
    /// - Create a View with a servico <see cref="CreateAsync()"/>
    /// - Create a servico <see cref="Create(Servico)"/>
    /// - Retrieves the servico to be edited <see cref="Edit(int?)"/>
    /// - Edits a servico in the database <see cref="Edit(int, Servico)"/>
    /// - Deletes a servico <see cref="Delete(int?)"/>
    /// - Checks the deletion of a servico <see cref="DeleteConfirmed(int)"/>
    /// - Check if a servico exists <see cref="ServicoExists(int)"/>
    /// - Get the artist based on an id <see cref="ArtistExists(int)"/>.
    /// - Get the artist based on an ownerId <see cref="ArtistExists(string)"/>.
    /// - Get the artist <see cref="GetArtist(string)"/>
    /// </summary> 
    [Authorize]
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// The method defines the context and user manager of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <param name="userManager">The UserManager of the controller</param>
        /// <returns>Void</returns>
        public ServicosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// The method is responsible for returning a list of servicos.
        /// </summary>
        /// <returns>A new view with a list of servicos</returns>
        // GET: Servicos
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var s = user.Id;

            var artist = await _context.Artists.Where(m => m.OwnerId == s).Include(m => m.AvatarImage).FirstOrDefaultAsync();

            if (artist == null)
            {
                return RedirectToAction("Create", "Artists");
            }

            var servicosList = await _context.Servicos.Where(m => m.OwnerID == artist).ToListAsync();

            return View(servicosList);
        }

        /// <summary>
        /// The method searches for an existing servico to exhibit it's details
        /// </summary>
        /// <param name="id"> The Id of the servico
        /// <returns>A new view of the servico selected</returns>
        // GET: Servicos/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }          

            var servico = await _context.Servicos.Include(m => m.OwnerID)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (servico == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (ArtistExists(user.Id))
                {
                    var artist = GetArtist(user.Id);
                    servico.Messages = _context.Message.Include(m => m.Sender)
                                                        .Include(m => m.Receiver)
                                                        .Where(m => m.Receiver == servico.OwnerID && m.Sender == artist)
                                                        .ToArray();
                    servico.HasProfile = true;
                }           
            }

            _context.Entry(servico.OwnerID).Reference(m => m.AvatarImage).Load();
            servico.OwnerID.AvatarImageURI = servico.OwnerID.AvatarImage.DecodeImage();

            servico.PortfolioData = servico.PopulatePortfolio(_context);

            return View(servico);
        }

        /// <summary>
        /// The method gets the data necessary for creating a servico
        /// </summary>
        /// <returns>A view of the servico</returns>
        // GET: Servicos/Create
        public async Task<IActionResult> CreateAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if(!ArtistExists(user.Id))
            {               
                ModelState.AddModelError("Artist", "Por favor crie um perfil antes de criar serviços");
                return RedirectToAction("Create", "Artists");
            }

            var s = new Servico(_context, user.Id);

            var Categorias = _context.Categorias.AsNoTracking().ToArray();
            var catArray = new string[Categorias.Length];
            for (int i = 0; i < Categorias.Length; i++)
            {
                Categorias item = Categorias[i];
                catArray[i] = item.Categoria;
            }

            s.Categorias = catArray;

            return View(s);
        }

        /// <summary>
        /// The method sends the data necessary for creating a servico
        /// </summary>
        /// <returns>A view of the servico</returns>
        // POST: Servicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Preco,BriefDescription,LongDescription,Topics,Publish,GigCategory,Address,PortfolioData")] Servico servico)
        {
            var user = await _userManager.GetUserAsync(User);
            var artist = GetArtist(user.Id);

            servico.OwnerID = artist;

            var s = servico.PortfolioData.Where(m => m.PortfolioType.ConvertToPortfolioType() == Utility.EPortfolioType.IMAGEM_CAPA).FirstOrDefault();

            if (string.IsNullOrEmpty(s.EmbedLink))
            {
                ModelState.AddModelError("Imagem Capa", "Por favor selecione uma imagem de capa");
                servico.PopulateOwnerData(_context, user.Id);
                return View(servico);
            }

            servico.PortfolioData.SavePortfolio(_context, servico, artist);

            if (!ModelState.IsValid)
            {
                servico.PopulateOwnerData(_context, user.Id);
                return View(servico);
            }          

            _context.Add(servico);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
           
        }

        /// <summary>
        /// The method edits an existing servico.
        /// </summary>
        /// <param name="id"> The servico id
        /// <returns>A new view of the servico</returns>
        // GET: Servicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (!ArtistExists(user.Id))
            {
                return NotFound();
            }

            var servico = await _context.Servicos.FindAsync(id);

            if (servico == null)
            {
                return NotFound();
            }

            _context.Entry(servico).Reference(m => m.OwnerID).Load();

            if (!servico.OwnerID.OwnerId.Equals(user.Id))
            {
                return NotFound();
            }

            _context.Entry(servico.OwnerID).Reference(m => m.AvatarImage).Load();

            servico.OwnerID.AvatarImageURI = servico.OwnerID.AvatarImage.DecodeImage();

            servico.PortfolioData = servico.PopulatePortfolio(_context);

            

            if (servico.PortfolioData.Length < 5)
            {
                var s = servico.PortfolioData;

                Array.Resize(ref s, 5);

                for (int i = servico.PortfolioData.Length; i < 5; i++)
                {
                    s[i] = new Portfolio() { Id = -1 };
                }

                servico.PortfolioData = s;
            }

            var Categorias = _context.Categorias.AsNoTracking().ToArray();
            var catArray = new string[Categorias.Length];
            for (int i = 0; i < Categorias.Length; i++)
            {
                Categorias item = Categorias[i];
                catArray[i] = item.Categoria;
            }

            servico.Categorias = catArray;

            return View(servico);
        }

        /// <summary>
        /// The method edits a servico and saves the changes in the database
        /// </summary>
        /// <param name="id"> The servico id
        /// <param name="servico"> The servico to be sent
        /// <returns>A view with the servico, NotFound or a RedirectToAction</returns>
        // POST: Servicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Preco,BriefDescription,LongDescription,Topics,Publish,GigCategory,Address,PortfolioData,OwnerID")] Servico servico)
        {
            Servico servicoLocal = servico;          

            if (id != servico.Id)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            Servico ownerCheck = _context.Servicos.Include(m => m.OwnerID).AsNoTracking().Where(m => m.Id == id).FirstOrDefault();

            if (ownerCheck.OwnerID.OwnerId != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var artist = GetArtist(user.Id);
                    servico.OwnerID = artist;

                    var s = servico.PortfolioData.Where(m => m.PortfolioType.ConvertToPortfolioType() == Utility.EPortfolioType.IMAGEM_CAPA).FirstOrDefault();

                    if (string.IsNullOrEmpty(s.EmbedLink))
                    {
                        ModelState.AddModelError("Imagem Capa", "Por favor selecione uma imagem de capa");
                        servico.PopulateOwnerData(_context, user.Id);
                        return View(servico);
                    }

                    servico.PortfolioData.SavePortfolio(_context, servico, artist);

                    if (!ModelState.IsValid)
                    {
                        servico.PopulateOwnerData(_context, user.Id);
                        return View(servico);
                    }

                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicoExists(servico.Id))
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
            return View(servico);
        }

        /// <summary>
        /// The method deletes a servico from the database
        /// </summary>
        /// <param name="id"> The servico id
        /// <returns>A view with the servico or NotFound</returns>
        // GET: Servicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var servico = await _context.Servicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (servico == null)
            {
                return NotFound();
            }

            return View(servico);
        }

        /// <summary>
        /// The method confirms if a servico was deleted
        /// </summary>
        /// <param name="id"> The servico id
        /// <returns>A RedirectToAction</returns>
        // POST: Servicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servicos.FindAsync(id);

            var portfolio = servico.PopulatePortfolio(_context);

            _context.Portfolios.RemoveRange(portfolio);

            _context.Servicos.Remove(servico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UpdateMessageBoardAsync(string receiver)
        {

            if (!User.Identity.IsAuthenticated) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var messages = _context.Message.Include(m => m.Sender)
                                                        .Include(m => m.Receiver)
                                                        .Where(m => m.Receiver.OwnerId == receiver && m.Sender.OwnerId == user.Id)
                                                        .ToArray();

            var vm = new MessageListViewModel()
            {
                Messages = messages,
                OwnerID = user.Id
            };

            return View(vm);
        }

        /// <summary>
        /// The method confirms if a servico exists
        /// </summary>
        /// <param name="id"> The servico id
        /// <returns>A servico related to the id</returns>
        private bool ServicoExists(int id)
        {
            return _context.Servicos.Any(e => e.Id == id);
        }

        /// <summary>
        /// The method verifies if an Artist exists based on it's servico id
        /// </summary>
        /// <param name="id"> The servico id
        /// <returns>The artist searched</returns>
        private bool ArtistExists(int id)
        {
            return _context.Artists.Any(e => e.Id == id);
        }

        /// <summary>
        /// The method verifies if an Artist exists based on it's ownerId
        /// </summary>
        /// <param name="ownerId"> The owner Id
        /// <returns>The artist searched</returns>
        private bool ArtistExists(string ownerId)
        {
            return _context.Artists.Any(e => e.OwnerId == ownerId);
        }

        /// <summary>
        /// The method verifies if an Artist exists based on it's ownerId
        /// </summary>
        /// <param name="ownerId"> The owner Id
        /// <returns>The artist searched</returns>
        private Artist GetArtist(string ownerId)
        {
            return _context.Artists.Where(m => m.OwnerId == ownerId).Include(m => m.AvatarImage).FirstOrDefault();
        }
    }
}
