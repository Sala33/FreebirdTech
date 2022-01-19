using FreebirdTech.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.ViewModels
{
    public class IndexViewModel
    {
        public string Email { get; set; }
        public Servico[] ExemplosServicos { get; set; }
    }
}
