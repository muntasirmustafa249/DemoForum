using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class EditCommentModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
