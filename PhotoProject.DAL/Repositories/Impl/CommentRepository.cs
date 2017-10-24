using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.EF;
using PhotoProject.DAL.Util;
using System.Data.Entity;

namespace PhotoProject.DAL.Repositories.Impl
{
    public class CommentRepository : ICommentRepository
    {
        private AppContext db;

        public CommentRepository(AppContext context)
        {
            this.db = context;
        }

        public void Create(Comment item)
        {         
            db.Comments.Add(item);
        }

        public void Delete(Comment item)
        {
            db.Comments.Remove(item);
        }

        public Comment FindById(int id)
        {
            Comment item = db.Comments.Find(id);           
            return item;
        }

        public async Task<Comment> FindByIdAsync(int id)
        {
            Comment item = await db.Comments.FindAsync(id);          
            return item;
        }

        public IQueryable<Comment> GetAll()
        {
            return db.Comments.AsQueryable();
        }

        public void Update(Comment item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
