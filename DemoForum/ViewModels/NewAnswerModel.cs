using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class NewAnswerModel
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
