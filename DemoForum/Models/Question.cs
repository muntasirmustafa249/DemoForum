﻿namespace DemoForum.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UpvoteCount { get; set; } = 0;
        public int DownvoteCount { get; set; } = 0;
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }
    }
}
