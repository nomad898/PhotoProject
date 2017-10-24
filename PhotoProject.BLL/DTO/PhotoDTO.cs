using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.BLL.DTO
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        public int PostId { get; set; }
    }
}
