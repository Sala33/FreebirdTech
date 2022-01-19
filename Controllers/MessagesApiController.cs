using FreebirdTech.Data;
using FreebirdTech.Extensions;
using FreebirdTech.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FreebirdTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    /// <summary>
    /// This class is responsible for controlling the messages api.
    /// Some of it's responsabilities are:
    /// - define a context and user manager<see cref="MessagesApiController"/>
    /// - get the messages <see cref="Get(string, string)"/>
    /// - get a value <see cref="Get(int)"/>
    /// - post a message <see cref="Post(string, string, string)"/>
    /// - put a message <see cref="Put(int, string)"/>
    /// - delete the message <see cref="Delete(int)"/>
    /// - get the artist <see cref="GetArtist(string)"/>
    /// </summary> 
    public class MessagesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        /// <summary>
        /// The method defines the context and user manager of the controller
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <param name="userManager">The UserManager of the controller</param>
        /// <returns>Void</returns>
        public MessagesApiController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        /// <summary>
        /// The method gets the messages of the sender and receiver artists
        /// </summary>
        /// <param name="senderID"> The Id of the sender
        /// <param name="receiverID"> The Id of the receiver
        /// <returns>The messages of the sender and receiver artists</returns>
        // GET: api/<MessagesApiController>
        [HttpGet]
        public IEnumerable<Message> Get(string senderID, string receiverID)
        {
            Artist sender = GetArtist(senderID);
            Artist receiver = GetArtist(receiverID);

            var messages = _context.Message.Include(m => m.Sender).Include(m => m.Receiver).Where(m => m.Receiver == receiver && m.Sender == sender).ToArray();

            return messages;
        }

        /// <summary>
        /// The method sends a message from the sender to the receiver
        /// </summary>
        /// <param name="message"> A message to be sent
        /// <param name="receiverID"> The id of the receiver
        /// <returns>NoContent()</returns>
        // POST api/<MessagesApiController>
        [HttpPost]
        public async Task<IActionResult> Post(string message, string receiverID)
        {
            var user = await _userManager.GetUserAsync(User);
            var sender = GetArtist(user.Id);
            var receiver = GetArtist(receiverID);
            var m = new Message()
            {
                MessageText = message,
                Receiver = receiver,
                Sender = sender,
                Time = DateTime.Now
            };

            _context.Message.Add(m);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// The method is empty
        /// </summary>
        /// <param name="value"> A string
        /// <param name="id"> An int
        // PUT api/<MessagesApiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// The method is empty
        /// </summary>
        /// <param name="id"> An int
        // DELETE api/<MessagesApiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        /// <summary>
        /// The method gets the artist based on the owner id
        /// </summary>
        /// <param name="ownerId"> The id of the artist
        /// <returns>The artist related to the ownerId()</returns>
        private Artist GetArtist(string ownerId)
        {
            return _context.Artists.Where(m => m.OwnerId == ownerId).Include(m => m.AvatarImage).FirstOrDefault();
        }

    }
}
