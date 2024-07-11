namespace DemoForum.ViewModels
{
    public class QuestionDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
        public string UserName { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<AnswerDetailModel> Answers { get; set; } = [];
        public List<CommentDetailModel> QuestionComments { get; set; } = [];
    }

    public class AnswerDetailModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int UpvoteCount { get; set; }
        public int DownvoteCount { get; set; }
        public string UserName { get; set; }
        public DateTime PostedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<CommentDetailModel> AnswerComments { get; set; } = [];
    }

    public class CommentDetailModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string PostType { get; set; }
        public string Content { get; set; }
        public int UpvoteCount { get; set; }
        //public int DownvoteCount { get; set; }
        public string UserName { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
    }
}
