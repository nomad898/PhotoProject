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
    public class RatingRepository : IRatingRepository
    {
        private AppContext db;

        public RatingRepository(AppContext context)
        {
            this.db = context;
        }

        public void Create(Rating item)
        {
            db.Ratings.Add(item);
        }

        public void Delete(Rating item)
        {
            db.Ratings.Remove(item);
        }

        public Rating FindById(int id)
        {
            Rating item = db.Ratings.Find(id);           
            return item;
        }

        public async Task<Rating> FindByIdAsync(int id)
        {
            Rating item = await db.Ratings.FindAsync(id);         
            return item;
        }

        public async Task<Rating> FindByUserIdAsync(string userId)
        {
            Rating item = await db.Ratings
                .Where(r => r.UserId == userId).SingleOrDefaultAsync();           
            return item;
        }

        public IQueryable<Rating> GetAll()
        {
            return db.Ratings.AsQueryable();
        }

        public void Update(Rating item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
