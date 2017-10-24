using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Public { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
