using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class NewQuestionModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
