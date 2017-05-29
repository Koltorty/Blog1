using System;
using System.Collections.Generic;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public IEnumerable<Comment> Comments { get; set; }
    }
}
