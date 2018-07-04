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
            var comments = _context.Comments.Where(c => c.PostId == postId).OrderByDescending(c => c.Date).AsEnumerable();
            return View(comments);
        }

        [HttpPost]
        public ActionResult Create(Comment comm)
        {
            comm.UserId = User.GetLoggedUserId();
            ViewBag.Name = _context.Users.FirstOrDefault(u => u.Id == User.GetLoggedUserId())?.UserName;
            _context.Comments.Add(comm);
            _context.SaveChanges();
            return RedirectToAction("FullPost", "Post", new { id = comm.PostId });
        }
    }
}
