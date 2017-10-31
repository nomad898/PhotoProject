using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Infrastructure;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IRatingService : IService<RatingDTO>
    {
        Task<OperationDetails> UpdateByIdAsync(RatingDTO ratingDto);
        Task<RatingDTO> FindByUserIdAsync(string userId);
    }
}
