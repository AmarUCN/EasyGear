using System.ComponentModel.DataAnnotations;

namespace WebShop.DTO
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"[^\s]+", ErrorMessage = "No spaces in password")]
        public string Password { get; set; }
    }
}
