using System.ComponentModel.DataAnnotations;

namespace IdentityNetCore.Models
{
    public class MFASetupViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Code { get; set; }
    }
}
