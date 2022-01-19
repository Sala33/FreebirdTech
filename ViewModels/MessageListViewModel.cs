using FreebirdTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.ViewModels
{
    public class MessageListViewModel
    {
        public Message[] Messages { get; set; }
        public string OwnerID { get; set; }
    }
}
