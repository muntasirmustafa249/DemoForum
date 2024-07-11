using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class EditQuestionModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
