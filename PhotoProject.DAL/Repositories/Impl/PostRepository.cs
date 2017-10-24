using PhotoProject.DAL.EF;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Repositories.Impl
{
    public class PostRepository : IPostRepository
    {
        private AppContext db;

        public PostRepository(AppContext context)
        {
            this.db = context;
        }

        public void Create(Post item)
        {
            db.Posts.Add(item);
        }

        public void Delete(Post item)
        {
            db.Posts.Remove(item);
        }

        public IQueryable<Post> GetAll()
        {
            return db.Posts.AsQueryable();
        }

        public Post FindById(int id)
        {
            Post item = db.Posts.Find(id);          
            return item;
        }
        
        public void Update(Post item)
        {
            db.Entry(item).State = EntityState.Modified;
        }

        public async Task<Post> FindByIdAsync(int id)
        {
            Post item = await db.Posts.FindAsync(id);         
            return item;
        }
    }
}
