using FreebirdTech.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FreebirdTech.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Portfolio Object <see cref="Portfolio"/>  which can be used for creating and acessing 
    /// the Portfolios in other part of the software.
    /// </summary> 
    public class Portfolio
    {
        public int Id { get; set; }
        public Artist OwnerArtist { get; set; }
        public Servico OwnerServico { get; set; }
        [Required]
        public string PortfolioType { get; set; }
        public string EmbedLink { get; set; }
        public Image Image { get; set; }

        [NotMapped]
        public string PageEmbed { get; set; }
        [NotMapped]
        public EPortfolioType Type {
            get
            {
                return PortfolioType.ConvertToPortfolioType(); 
            } 
        }

        public Portfolio()
        {
            PortfolioType = EPortfolioType.NONE.ToString();
        }

        public void SetUpOwnership(Artist ownerArtist, Servico ownerServico)
        {
            OwnerArtist = ownerArtist;
            OwnerServico = ownerServico;
        }
    }
}
