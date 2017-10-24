using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Models
{
    public class RatingViewModel
    {
        public int Id { get; set; }
        [Range(0, 5)]
        public int RatingValue { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int PostId { get; set; }
    }
}