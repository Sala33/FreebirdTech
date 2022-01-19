using FreebirdTech.Models;
using FreebirdTech.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreebirdTech.Controllers
{
    public class BlogsController : Controller
    {
        public IActionResult Index()
        {
            List<string> taglist = new List<string>
            {
                "Mundo",
                "Brasil",
                "Tecnologia",
                "Design",
                "Negócios",
                "Saúde"
            };

            var post = new BlogPost() 
            { 
                Abstract = "Neque porro quisquam est qui dolorem ipsum quia dolor sit amet, consectetur, adipisci velit...",
                Author = "Ian Gigliotti",
                Body = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.",
                Date = DateTime.Now,
                Tag = "World",
                Title = "Exemplo"
            };

            var postList = new List<BlogPost>();

            for (int i = 0; i < 5; i++)
            {
                postList.Add(post);
            }

            var vm = new BlogsViewModel()
            {
                TagList = taglist,
                PostList = postList
                
            };

            return View(vm);
        }
    }
}
