using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Mailing List Object <see cref="MailingList"/>  which can be used for creating and acessing 
    /// the Mailing List in other part of the software.
    /// </summary> 
    public class MailingList
    {
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
