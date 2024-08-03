using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Account
    {
        public int AccountID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"[^\s]+", ErrorMessage = "No spaces in password")]
        public string Password { get; set; }

        public Account(int accountID, string email, string password)
        {
            AccountID = accountID;
            Email = email;
            Password = password;
        }

        public Account() { }
    }
}

