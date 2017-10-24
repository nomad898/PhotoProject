using Microsoft.AspNet.Identity.EntityFramework;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.EF
{
    public class AppContext : IdentityDbContext<AppUser>
    {
        static AppContext()
        {
            Database.SetInitializer<AppContext>(new AppDbInit());
        }

        public AppContext() : base(MyConstants.CONNECTION_STRING) { }

        public AppContext(string connectionString)
            : base(connectionString)
        {

        }
        
        public DbSet<Album> Albums { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
