using System;

namespace Blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int PostId { get; set;}
        public Post Post { get; set; }
    }
}
