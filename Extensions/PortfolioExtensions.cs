using FreebirdTech.Data;
using FreebirdTech.Models;
using FreebirdTech.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Extensions
{
    public static class PortfolioExtensions
    {
        public static bool SavePortfolio(this Portfolio[] portfolio, ApplicationDbContext context, Servico owner, Artist ownerArtist)
        {
            bool s = false;

            foreach (var entry in portfolio)
            {

                var type = entry.PortfolioType.ConvertToPortfolioType();

                if (type == EPortfolioType.NONE) continue; //delete portfolio if exists

                Portfolio p = context.Portfolios.Find(entry.Id);

                if (p == null)
                {
                    p = new Portfolio();
                }

                bool isCurrentImage = p.Type == EPortfolioType.IMAGEM || p.Type == EPortfolioType.IMAGEM_CAPA;

                if (type != EPortfolioType.EMBED)
                {
                    if (string.IsNullOrEmpty(entry.EmbedLink)) continue; // error

                    Image img = new Image();

                    if (isCurrentImage)
                    {
                        //Updates old Image to new one
                        context.Entry(p).Reference(l => l.Image).Load();
                        img = p.Image;
                    }
                    else
                    {
                        //Delete old link for sanity purposes
                        p.EmbedLink = null;
                    }

                    p.Image = entry.EmbedLink.EncodeImage(owner.Title, EImageType.PORTFOLIO, img);

                }
                else
                {
                    if (isCurrentImage)
                    {
                        //Delete old image to save DB Space
                        context.Entry(p).Reference(l => l.Image).Load();
                        context.Images.Remove(p.Image);
                    }

                    p.EmbedLink = entry.PageEmbed;
                }

                p.PortfolioType = entry.PortfolioType;
                p.OwnerArtist = ownerArtist;
                p.OwnerServico = owner;

                s = true;

                context.Portfolios.Update(p);
            }
          
            return s;
        }
    }
}
