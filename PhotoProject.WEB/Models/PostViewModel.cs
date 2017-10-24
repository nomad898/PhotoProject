using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Models
{
    public class PostInfo
    {
        public PostViewModel Post { get; set; }
        public RatingViewModel Rating { get; set; }
        public CommentViewModel Comment { get; set; }
    }

    public class PostViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int AlbumId { get; set; }
        public string UserId { get; set; }
        public float AverageRating { get; set; }
        public int VoteCounter { get; set; }
        public ICollection<PhotoViewModel> Photos { get; set; }
        public ICollection<CommentViewModel> Comments { get; set; }
        public ICollection<RatingViewModel> Ratings { get; set; }
    }

    public class CreatePostViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int AlbumId { get; set; }
        public ICollection<CreatePhotoViewModel> Photos { get; set; }
    }
}