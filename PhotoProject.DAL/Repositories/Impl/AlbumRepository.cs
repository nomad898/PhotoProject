using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoProject.DAL.Entities;
using PhotoProject.DAL.Util;
using PhotoProject.DAL.EF;
using System.Data.Entity;

namespace PhotoProject.DAL.Repositories.Impl
{
    public class AlbumRepository : IAlbumRepository
    {
        private AppContext db;

        public AlbumRepository(AppContext context)
        {
            this.db = context;
        }

        public void Create(Album item)
        {
            db.Albums.Add(item);
        }

        public void Delete(Album item)
        {
            db.Albums.Remove(item);
        }

        public Album FindById(int id)
        {
            Album item = db.Albums.Find(id);        
            return item;
        }

        public async Task<Album> FindByIdAsync(int id)
        {
            Album item = await db.Albums.FindAsync(id);    
            return item;
        }

        public async Task<Album> FindByNameAsync(string name)
        {
            Album item = await db.Albums.Where(x => x.Title == name)
                .SingleOrDefaultAsync();      
            return item;
        }

        public IQueryable<Album> GetAll()
        {
            return db.Albums.AsQueryable();
        }

        public void Update(Album item)
        {        
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
