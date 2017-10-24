using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoProject.DAL.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        [Range(0, 5)]
        public int RatingValue { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }       
    }
}
