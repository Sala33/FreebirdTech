using FreebirdTech.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FreebirdTech.Extensions;
using FreebirdTech.ViewComponents;
using FreebirdTech.Models;

namespace FreebirdTech.Controllers
{
    /// <summary>
    /// This class is responsible for controlling the Search.
    /// Some of it's responsabilities are:
    /// - define context and user <see cref="ArtistsController"/>
    /// - Creates a view with a SearchViewModel <see cref="Index(string)"/>
    /// </summary> 
    public class SearchController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The method defines the context
        /// </summary>
        /// <param name="context">The ApplicationDbContext of the controller</param>
        /// <returns>Void</returns>
        public SearchController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The method is responsible Creating a SearchViewModel with cardInfo and an
        /// array of Categorias
        /// </summary>
        /// <returns>A new view with a SearchViewModel</returns>
        public IActionResult Index(string search, string iscategory)
        {
            Servico[] servicos = _context.Servicos.Include(m => m.OwnerID).AsNoTracking().ToArray();

            var Categorias = _context.Categorias.AsNoTracking().ToArray();

            if (!string.IsNullOrEmpty(search))
            {
                if (iscategory.Equals("true"))
                {
                    servicos = servicos.Where(s => s.GigCategory.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                }
                else
                {
                    servicos = servicos.Where(s => s.Title.Contains(search, StringComparison.InvariantCultureIgnoreCase) || s.OwnerID.Name.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToArray();
                }
            }             

            var cardInfo = new CardInfo[servicos.Length];         

            var catArray = new string[Categorias.Length];

            for (int i = 0; i < servicos.Length; i++)
            {
                Servico item = servicos[i];

                cardInfo[i] = item.CreateCardInfo(_context);
            }

            for (int i = 0; i < Categorias.Length; i++)
            {
                Categorias item = Categorias[i];
                catArray[i] = item.Categoria;
            }

            var vm = new SearchViewModel()
            {
                Cards = cardInfo,
                Categorias = catArray
            };

            return View(vm);
        }
    }
}
