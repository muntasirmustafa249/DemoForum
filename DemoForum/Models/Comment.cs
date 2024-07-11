namespace DemoForum.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string PostType { get; set; }
        public string Content { get; set; }
        public int UpvoteCount { get; set; } = 0;
        //public int DownvoteCount { get; set; } = 0;
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
    }
}
