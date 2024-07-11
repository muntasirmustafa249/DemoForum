using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class RegistrationModel
    {
        [Required]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
