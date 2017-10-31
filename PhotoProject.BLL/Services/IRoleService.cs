using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IRoleService : IDisposable
    {
        Task<OperationDetails> CreateAsync(RoleDTO roleDto);
        Task<RoleDTO> FindByIdAsync(string id);
        IQueryable<RoleDTO> GetAll();
        Task<OperationDetails> DeleteByIdAsync(string id);
    }
}
