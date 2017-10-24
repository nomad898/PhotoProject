using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int AlbumId { get; set; }
        public ICollection<PhotoDTO> Photos { get; set; }
    }
}
