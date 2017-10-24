using PhotoProject.BLL.Util;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IService<T> where T : class
    {
        Task<OperationDetails> CreateAsync(T item);
        Task<T> FindByIdAsync(int id);
        IQueryable<T> GetAll();
        Task<OperationDetails> DeleteByIdAsync(int id);
    }
}
