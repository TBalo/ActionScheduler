using System;
using System.ComponentModel.DataAnnotations;

namespace TODO_LIST.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "User Name is required.")]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100)]
        public string Password { get; set; }


        public User()
        {
            UserName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }

        public User(string userName, string email, string password)
        {
            UserName = userName;
            Email = email;
            Password = password;
        }
    }

}
