using FreebirdTech.Data;
using FreebirdTech.Models;
using FreebirdTech.Utility;
using FreebirdTech.ViewComponents;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Extensions
{
    public static class ServicoExtensions
    {
        public static Portfolio[] PopulatePortfolio(this Servico servico, ApplicationDbContext context)
        {
            var s = servico;

            var p = context.Portfolios.AsNoTracking().Where(m => m.OwnerServico == s).Include(m => m.Image).ToArray();

            foreach (var item in p)
            {
                if (item.Image != null)
                    item.EmbedLink = item.Image.DecodeImage();
            }

            return p;
        }

        public static CardInfo CreateCardInfo(this Servico servico, ApplicationDbContext context)
        {
            var capaPortfolio = context.Portfolios.Include(m => m.Image)
                                                    .AsNoTracking()
                                                    .Where(m => m.OwnerServico == servico && m.PortfolioType.Equals(EPortfolioType.IMAGEM_CAPA.ToString()))
                                                    .FirstOrDefault();           
            var img = capaPortfolio.Image.DecodeImage();

            var card = new CardInfo
            {
                ID = servico.Id,
                ImageData = img,
                OwnerName = servico.OwnerID.Name,
                BriefDescription = servico.BriefDescription,
                Title = servico.Title,
                Categoria = servico.GigCategory
            };

            return card;
        }

    }
}
