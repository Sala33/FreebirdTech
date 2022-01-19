using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Models
{
    public class BlogPost
    {
        public int ID { get; set; }
        public string Body { get; set; }
        public string Abstract { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string Author { get; set; }
        public string Tag { get; set; }
    }
}
