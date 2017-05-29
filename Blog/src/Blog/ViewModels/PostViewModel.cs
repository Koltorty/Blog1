using System;
using Blog.Models;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public Comment[] Comments { get; set; }
        public Comment Comment { get; set; }
    }
}
