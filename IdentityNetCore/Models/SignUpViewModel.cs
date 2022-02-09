using System.ComponentModel.DataAnnotations;

namespace IdentityNetCore.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage ="Email is required")]
        [DataType(DataType.EmailAddress,ErrorMessage ="Email Address Format is not correct")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Password is required")]
        [DataType(DataType.Password,ErrorMessage ="Missing Password")]
        public string Password { get; set; }


        public string Role { get; set; }
    }
}
