using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Util;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateAsync(UserDTO userDto);
        Task<ClaimsIdentity> AuthenticateAsync(UserDTO userDto);
        Task<UserDTO> FindByIdAsync(string id);
        IQueryable<UserDTO> GetAll();
        Task<OperationDetails> DeleteByIdAsync(string id);
        Task<OperationDetails> UpdateAsync(UserDTO userDto);
        Task<UserDTO> FindByNameAsync(string userName);
        Task<OperationDetails> UpdateByIdAsync(string id);
    }
}
