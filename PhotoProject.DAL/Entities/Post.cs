using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("Album")]
        public int AlbumId { get; set; }
        public virtual Album Album { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }     
    }
}
