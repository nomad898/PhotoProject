using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoProject.WEB.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public byte[] Avatar { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Display(Name = "Avatar")]
        public HttpPostedFileBase Avatar { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}