using System.ComponentModel.DataAnnotations;

namespace IdentityNetCore.Models
{
    public class SigninViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public bool RememberMe { get; set;}

    }
}
