using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T FindById(int id);
        Task<T> FindByIdAsync(int id);
        void Create(T item);
        void Update(T item);
        void Delete(T item);
    }
}
