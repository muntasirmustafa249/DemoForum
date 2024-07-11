namespace DemoForum.ViewModels
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UpvoteCount { get; set; } = 0;
        public int DownvoteCount { get; set; } = 0;
        public string UserName { get; set; }
        //public string Email { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int AnswerCount { get; set; } = 0;
    }
}
