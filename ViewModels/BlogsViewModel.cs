using FreebirdTech.Models;
using System;
using System.Collections.Generic;

namespace FreebirdTech.ViewModels
{
    public class BlogsViewModel
    {
        public List<BlogPost> PostList { get; set; }
        public List<string> TagList { get; set; }
        public List<DateTime> DateList { get; set; }
    }
}
