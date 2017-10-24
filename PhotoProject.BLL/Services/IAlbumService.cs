using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Util;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IAlbumService : IService<AlbumDTO>
    {
        Task<OperationDetails> UpdateByIdAsync(AlbumDTO albumDto);
        IQueryable<AlbumDTO> FindByName(string name);
    }
}
