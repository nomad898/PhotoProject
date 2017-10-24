using Microsoft.AspNet.Identity.EntityFramework;
using PhotoProject.DAL.EF;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Repositories.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppContext db;
        private AppUserManager userManager;
        private AppRoleManager roleManager;
        private PostRepository postRepository;  
        private AlbumRepository albumRepository;
        private PhotoRepository photoRepository;
        private CommentRepository commentRepository;
        private RatingRepository ratingRepository;

        public UnitOfWork(string connectionString)
        {
            db = new AppContext(connectionString);
            userManager = new AppUserManager(
                new UserStore<AppUser>(db));
            roleManager = new AppRoleManager(
               new RoleStore<AppRole>(db));
        }

        public AppUserManager UserManager
        {
            get
            {
                if (userManager == null)
                    userManager = new AppUserManager(
                new UserStore<AppUser>(db));
                return userManager;
            }
        }

        public AppRoleManager RoleManager
        {
            get
            {
                if (roleManager == null)
                    roleManager = new AppRoleManager(
                new RoleStore<AppRole>(db));
                return roleManager;
            }
        }        

        public PostRepository Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepository(db);
                return postRepository;
            }
        }

        public PhotoRepository Photos
        {
            get
            {
                if (photoRepository == null)
                    photoRepository = new PhotoRepository(db);
                return photoRepository;
            }
        }
           
        public AlbumRepository Albums
        {
            get
            {
                if (albumRepository == null)
                    albumRepository = new AlbumRepository(db);
                return albumRepository;
            }
        }

        public CommentRepository Comments
        {
            get
            {
                if (commentRepository == null)
                    commentRepository = new CommentRepository(db);
                return commentRepository;
            }
        }

        public RatingRepository Ratings
        {
            get
            {
                if (ratingRepository == null)
                    ratingRepository = new RatingRepository(db);
                return ratingRepository;
            }
        }
       
        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    userManager.Dispose();
                    roleManager.Dispose();
                    db.Dispose();
                }
                this.disposed = true;
            }
        }
    }
}
