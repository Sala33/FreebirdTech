using FreebirdTech.Data;
using FreebirdTech.Models;
using FreebirdTech.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Extensions
{
    public static class ContextExtensions
    {
        public static async Task<Servico[]> GetUnorderedServicosArrayAsync(this ApplicationDbContext context, int size = 0)
        {
            Servico[] servicos;
            if (size <= 0)
                servicos = await context.Servicos.Where(b => b.Publish).ToArrayAsync();
            else
                servicos = await context.Servicos.Where(b => b.Publish).Take(size).ToArrayAsync();

            foreach (var servico in servicos)
            {
                Portfolio portfolio = await context.Portfolios.Where(b => b.OwnerServico == servico && b.PortfolioType == EPortfolioType.IMAGEM_CAPA.ToString()).SingleAsync();

                servico.PreviewImageURI = portfolio.PopulateImage(context);
            }


            return servicos;
        }
        private static string PopulateImage(this Portfolio entry, ApplicationDbContext context)
        {
            context.Entry(entry).Reference(d => d.Image).Load();

            return GetBase64FromImageData(entry.Image.ImageData);
        }

        private static string GetBase64FromImageData(byte[] entry)
        {
            string imageBase64Data = Convert.ToBase64String(entry);
            string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);

            return imageDataURL;
        }

        public static bool SaveImageToDatabase(this ApplicationDbContext context, Image image, out EntityEntry<Image> savedImage)
        {
            var i = image;
            if (i.ImageData == null)
            {
                savedImage = null;
                return false;
            }

            EntityEntry<Image> im = context.Images.Update(i);
            savedImage = im;

            return true;
        }
    }
}
