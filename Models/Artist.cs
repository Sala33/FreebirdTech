using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Artist Object <see cref="Artist"/> which can be used for creating and acessing 
    /// the Artist in other part of the software
    /// </summary>
    public class Artist
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        [DisplayName("Nome")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Categoria")]
        public string Categoria { get; set; }
        [Required]
        [StringLength(150)]
        [DisplayName("Breve Descrição")]
        public string BriefDescription { get; set; }
        [Required]
        [EmailAddress]
        [DisplayName("Email para contato")]
        public string Contact { get; set; }
        [Required]
        [StringLength(500)]
        [DisplayName("Descrição")]
        public string Description { get; set; }
        [Required]
        public string OwnerId { get; set; }
        public Image AvatarImage { get; set; }
        [Required]
        [NotMapped]
        public string AvatarImageURI { get; set; }
    }
}
