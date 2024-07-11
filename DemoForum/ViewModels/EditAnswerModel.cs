using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class EditAnswerModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
