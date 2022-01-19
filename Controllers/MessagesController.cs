using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FreebirdTech.Data;
using FreebirdTech.Models;
using Microsoft.AspNetCore.Identity;
using FreebirdTech.ViewModels;
using FreebirdTech.Extensions;

namespace FreebirdTech.Controllers
{
    /// <summary>
    /// This class is responsible for controlling the Messages.
    /// Some of it's responsabilities are:
    /// - define context and user <see cref="ArtistsController"/>
    /// - show Index <see cref="Index"/>
    /// - exhibit message Details <see cref="Details(int?)"/>
    /// - Create a ChatViewModel <see cref="Create(ChatViewModel)"/>
    /// - Retrieves the message to be edited <see cref="Edit(int?)"/>
    /// - Edits a message in the database <see cref="Edit(int, Message)"/>
    /// - Deletes a message <see cref="Delete(int?)"/>
    /// - Checks the deletion of a message <see cref="DeleteConfirmed(int)"/>
    /// - Check if a message exists <see cref="MessageExists(int)"/>
    /// - Get the artist <see cref="GetArtist(string)"/>
    /// - Get the artist asynchronously <see cref="GetArtistAsync()"/>.
    /// </summary> 
    public class MessagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// The method defines the context and user manager of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <param name="userManager">The UserManager of the controller</param>
        /// <returns>Void</returns>
        public MessagesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// The method is responsible for returning a view with list of messages
        /// </summary>
        /// <returns>A new view with a list of messages</returns>
        // GET: Messages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Message.ToListAsync());
        }

        /// <summary>
        /// The method searches for an existing message to exhibit it's details
        /// </summary>
        /// <param name="id"> The Id of the message
        /// <returns>A new view of the message selected</returns>
        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.ID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        /// <summary>
        /// The method gets the data necessary for creating a ChatViewModel
        /// </summary>
        /// <param name="userid"> The Id of the user
        /// <returns>A view with a ChatViewModel</returns>
        // GET: Messages/Create
        public async Task<IActionResult> CreateAsync(string userid)
        {
            var artist = await GetArtistAsync();

            if(artist == null)
            {
                return RedirectToAction("Create", "Artists");
            }

            Artist sender;
            if (string.IsNullOrEmpty(userid))
                sender = artist;
            else
                sender = GetArtist(userid);

            var messages = _context.Message.AsNoTracking().Include(m => m.Sender).Include(m => m.Receiver).Where(m => m.Receiver == artist && m.Sender == sender).ToArray();

            var messageLog = new HashSet<Artist>();

            var messageLogArtists = _context.Message
                                            .Include(m => m.Sender)
                                            .Where(m => m.Receiver == artist).ToArray();

            foreach (var item in messageLogArtists)
            {
                messageLog.Add(item.Sender);
            }

            foreach (var item in messageLog)
            {
                _context.Entry(item).Reference(m => m.AvatarImage).Load();
                item.AvatarImageURI = item.AvatarImage.DecodeImage();
            }

            artist.AvatarImageURI = artist.AvatarImage.DecodeImage();

            var vm = new ChatViewModel
            {
                MessageLog = messageLog.ToArray(),
                Messages = messages,
                Reciever = artist,
                SenderID = sender.OwnerId,
                ReceiverID = artist.OwnerId
            };

            return View(vm);
        }

        /// <summary>
        /// The method sends the data necessary for creating message
        /// </summary>
        /// <param name="messageViewModel"> The view model of the message
        /// <returns>A view with the messageViewModel or a RedirectToAction</returns>
        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MessageToSend")] ChatViewModel messageViewModel)
        {
            if (ModelState.IsValid)
            {
                Message message = messageViewModel.MessageToSend;
                var artist = await GetArtistAsync();
                message.Time = DateTime.Now;
                message.Sender = artist;
                message.Receiver = artist;
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(messageViewModel);
        }

        /// <summary>
        /// The method finds a message related to the id
        /// </summary>
        /// <param name="id"> The message id
        /// <returns>A view with the message or a NotFound</returns>
        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        /// <summary>
        /// The method edits a message and saves the changes in the database
        /// </summary>
        /// <param name="id"> The message id
        /// <param name="message"> The message to be sent
        /// <returns>A view with the message, NotFound or a RedirectToAction</returns>
        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Time,MessageText")] Message message)
        {
            if (id != message.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.ID))
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
            return View(message);
        }

        /// <summary>
        /// The method deletes a message from the database
        /// </summary>
        /// <param name="id"> The message id
        /// <returns>A view with the message or NotFoun</returns>
        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.ID == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        /// <summary>
        /// The method confirms if a message was deleted
        /// </summary>
        /// <param name="id"> The message id
        /// <returns>A RedirectToAction</returns>
        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetMessageDataAsync(string sender)
        {
            if (!User.Identity.IsAuthenticated) return NotFound();

            var user = await _userManager.GetUserAsync(User);

            var messages = _context.Message
                                    .AsNoTracking()
                                    .Include(m => m.Sender)
                                    .Include(m => m.Receiver).Where(m => m.Receiver.OwnerId == user.Id && m.Sender.OwnerId == sender)
                                    .ToArray();

            var vm = new MessageListViewModel()
            {
                Messages = messages,
                OwnerID = user.Id
            };

            return View(vm);
        }

        /// <summary>
        /// The method confirms if a message exists
        /// </summary>
        /// <param name="id"> The message id
        /// <returns>A message related to the id</returns>
        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.ID == id);
        }

        /// <summary>
        /// The method returns the artist related to the ownerId
        /// </summary>
        /// <param name="ownerId"> The id of the owner
        /// <returns>The artist related to the ownerId</returns>
        private Artist GetArtist(string ownerId)
        {
            return _context.Artists.Where(m => m.OwnerId == ownerId).Include(m => m.AvatarImage).FirstOrDefault();
        }

        /// <summary>
        /// The method returns the artist related to the user in the context
        /// </summary>
        /// <returns>The artist related to the context user</returns>
        private async Task<Artist> GetArtistAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            return _context.Artists.Where(m => m.OwnerId == user.Id).Include(m => m.AvatarImage).FirstOrDefault();
        }
    }
}
