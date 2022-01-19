using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Message Object <see cref="Message"/>  which can be used for handling messages.
    /// </summary>
    public class Message
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public Artist Sender { get; set; }
        public Artist Receiver { get; set; }
        [Required]
        [MaxLength(170, ErrorMessage = "Mensagem deve ter no máximo 170 caracteres")]
        public string MessageText { get; set; }
        public bool IsRead { get; set; }
    }
}
