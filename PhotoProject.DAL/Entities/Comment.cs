using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }     
    }
}
