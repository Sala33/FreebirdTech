using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    /// <summary>
    /// This class represents the Categorias Object <see cref="Categorias"/> which can be used for creating and acessing 
    /// the Categorias in other part of the software
    /// </summary>
    public class Categorias
    {
        [Key]
        public string Categoria { get; set; }
        public Categorias Parent { get; set; }
    }
}
