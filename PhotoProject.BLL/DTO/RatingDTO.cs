using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.DTO
{
    public class RatingDTO
    {
        public int Id { get; set; }
        [Range(0, 5)]
        public int RatingValue { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}
