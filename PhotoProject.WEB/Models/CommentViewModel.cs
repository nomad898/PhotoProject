using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}