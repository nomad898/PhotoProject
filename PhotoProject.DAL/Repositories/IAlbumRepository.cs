using PhotoProject.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Repositories
{
    public interface IAlbumRepository : IRepository<Album>
    {
        Task<Album> FindByNameAsync(string name);
    }
}
