using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using System.Collections.Generic;
using System.Linq;
using Blog.Helpers;

namespace Blog.Controllers
{
    public class CommentController : Controller
    {
        private readonly BlogContext _context;

        public CommentController(BlogContext context)
        {
            _context = context;
        }

        public IActionResult ShowComments(int postId)
        {
            IEnumerable<Comment> qqq =
                from qq in _context.Comments
                where qq.PostId == postId
                orderby qq.Date descending
                select qq;
            return View(qqq);
        }

        [HttpPost]
        public ActionResult Create(Comment comm)
        {
            comm.UserId = User.GetLoggedUserId();
            _context.Comments.Add(comm);
            _context.SaveChanges();
            return RedirectToAction("FullPost", "Post", new { id = comm.PostId });
        }
    }
}
