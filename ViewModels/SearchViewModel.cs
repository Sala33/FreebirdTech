using FreebirdTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.ViewComponents
{
    public class SearchViewModel
    {
        public CardInfo[] Cards { get; set; }
        public string[] Categorias { get; set; }
    }

    public class CardInfo
    {
        public int ID { get; set; }
        public string ImageData { get; set; }
        public string OwnerName { get; set; }
        public string BriefDescription { get; set; }
        public string Title { get; set; }
        public string Categoria { get; set; }
    }
}
