using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoProject.DAL.EF;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Util;
using System.Data.Entity;

namespace PhotoProject.DAL.Repositories.Impl
{
    public class PhotoRepository : IPhotoRepository
    {
        private AppContext db;

        public PhotoRepository(AppContext context)
        {
            this.db = context;
        }

        public void Create(Photo item)
        {
            db.Photos.Add(item);
        }

        public void Delete(Photo item)
        {
            db.Photos.Remove(item);
        }

        public Photo FindById(int id)
        {
            Photo item = db.Photos.Find(id);           
            return item;
        }

        public async Task<Photo> FindByIdAsync(int id)
        {
            Photo item = await db.Photos.FindAsync(id);          
            return item;
        }

        public IQueryable<Photo> GetAll()
        {
            return db.Photos.AsQueryable();
        }

        public void Update(Photo item)
        {        
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
