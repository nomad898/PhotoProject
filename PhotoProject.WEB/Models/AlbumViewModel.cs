using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Models
{
    public class AlbumViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool Public { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }

        public AlbumViewModel()
        {
            Posts = new List<PostViewModel>();
        }
    }  
}