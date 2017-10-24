using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class Photo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
