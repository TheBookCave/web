using System.ComponentModel.DataAnnotations;

namespace web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        public bool Remember { get; set; }
    }
}