using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Models
{
    public class PhotoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public byte[] Content { get; set; }
        public int PostId { get; set; }
    }

    public class CreatePhotoViewModel
    {
        public string Title { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}