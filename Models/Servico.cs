using FreebirdTech.Data;
using FreebirdTech.Extensions;
using Microsoft.EntityFrameworkCore;
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
    /// This class represents the Serviço Object <see cref="Servico"/>  which can be used for creating and acessing 
    /// the jobs and it's atributes in other part of the software
    /// </summary> 
    public class Servico
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayName("Titulo")]
        public string Title { get; set; }
        [Required]
        [DisplayName("Preço")]
        public float Preco { get; set; }
        [Required]
        [StringLength(128)]
        [DisplayName("Breve Descrição")]
        public string BriefDescription { get; set; }
        [Required]
        [StringLength(1000)]
        [DisplayName("Longa Descrição")]
        public string LongDescription { get; set; }
        public List<string> Topics { get; set; }
        [DisplayName("Publicar")]
        public bool Publish { get; set; }
        public Artist OwnerID { get; set; }
        [Required]
        [DisplayName("Categoria")]
        public string GigCategory { get; set; }
        [Required]
        [DisplayName("Endereço")]
        public string Address { get; set; }
        [NotMapped]
        public string PreviewImageURI { get; set; }
        [NotMapped]
        public Portfolio[] PortfolioData { get; set; }
        [NotMapped]
        public Message[] Messages { get; set; }
        [NotMapped]
        public bool HasProfile { get; set; }
        [NotMapped]
        public string[] Categorias { get; set; }

        public Servico()
        {
            PortfolioData = new Portfolio[5];

            for (int i = 0; i < PortfolioData.Length; i++)
            {
                PortfolioData[i] = new Portfolio();
            }

            Topics = new List<string> { "", "","","","" };

        }

        public Servico(ApplicationDbContext context, string ownerId)
        {
            PortfolioData = new Portfolio[5];

            for (int i = 0; i < PortfolioData.Length; i++)
            {
                PortfolioData[i] = new Portfolio();
            }

            Topics = new List<string> { "", "", "", "", "" };

            OwnerID = context.Artists.AsNoTracking().Where(m => m.OwnerId == ownerId).Include(m => m.AvatarImage).FirstOrDefault();

            OwnerID.AvatarImageURI = OwnerID.AvatarImage.DecodeImage();
        }

        public void PopulateOwnerData(ApplicationDbContext context, string ownerId)
        {
            OwnerID = context.Artists.AsNoTracking().Where(m => m.OwnerId == ownerId).Include(m => m.AvatarImage).FirstOrDefault();

            OwnerID.AvatarImageURI = OwnerID.AvatarImage.DecodeImage();
        }
    }
}
