using PhotoProject.BLL.DTO;
using PhotoProject.BLL.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.Services
{
    public interface IRatingService : IService<RatingDTO>
    {
        Task<OperationDetails> UpdateByIdAsync(RatingDTO ratingDto);
        Task<RatingDTO> FindByUserIdAsync(string userId);
    }
}
