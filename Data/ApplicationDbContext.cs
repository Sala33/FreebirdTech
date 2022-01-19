using FreebirdTech.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreebirdTech.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Servico> Servicos { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<MailingList> MailingList { get; set; }
        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<CalendarEvent> CalendarEvent { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }
    }
}
