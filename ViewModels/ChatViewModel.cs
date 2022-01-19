using FreebirdTech.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.ViewModels
{
    public class ChatViewModel
    {
        public Artist Reciever { get; set; }
        public Message[] Messages { get; set; }
        public Artist[] MessageLog { get; set; }
        public Message MessageToSend { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
    }
}
