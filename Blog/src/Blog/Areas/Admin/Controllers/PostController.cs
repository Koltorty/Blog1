using System;
using System.Linq;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Blog.Libraries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Blog.ViewModels;

namespace Blog.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class PostController : Controller
    {
        private readonly BlogContext _context;
        public PostController(BlogContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Edit(int? id)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == id);
            return View(post);
        }

        public IActionResult Delete(int? id)
        {
            var post = _context.Posts.Include(p => p.Comments).SingleOrDefault(s => s.Id == id);
            return View(post);
        }

        [HttpPost]
        public virtual ActionResult PostList(DataSourceRequest command)
        {
            var posts = new PagedList<Post>(_context.Posts, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = posts,
                Total = posts.TotalCount
            };
            return Json(gridModel);
        }

        [HttpPost]
        public ActionResult Edit(Post post, int? id)
        {
            _context.Posts.Update(post);
            _context.SaveChanges();
            return RedirectToAction("FullPost", new { id = post.Id });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).SingleOrDefault(s => s.Id == id);
            var comm = _context.Comments.Where(p => p.PostId == id);
            _context.Posts.Remove(post);
            foreach (var i in comm)
            {
                _context.Comments.Remove(i);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult FullPost(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            return View(new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                Date = post.Date,
                Comment = new Comment { Date = DateTime.Now, PostId = post.Id },
                Comments = post.Comments.ToArray()
            });
        }
    }
}