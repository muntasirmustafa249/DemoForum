using System.ComponentModel.DataAnnotations;

namespace DemoForum.ViewModels
{
    public class NewCommentModel
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public string PostType { get; set; }
        [Required]
        public string Content { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
    }
}
