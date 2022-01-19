using FreebirdTech.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageNotificationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MessageNotificationController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<bool> Get()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return false;

            return await _context.Message.AsNoTracking().Include(m => m.Receiver).Where(m => m.Receiver.OwnerId == user.Id).AnyAsync();
        }

        [HttpPost]
        public async Task PostAsync(string senderID, string receiverID)
        {
            var messages = _context.Message.Where(m => m.Receiver.OwnerId == receiverID && m.Sender.OwnerId == senderID).ToArray();

            foreach (var item in messages)
            {
                item.IsRead = true;
            }

            _context.Message.UpdateRange(messages);
            await _context.SaveChangesAsync();
        }
    }
}
