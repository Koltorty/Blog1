using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Blog.Helpers;
using Blog.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Blog.Libraries;

namespace Blog.Controllers
{
    public class PostController : Controller
    {
        private readonly BlogContext _context;

        public PostController(BlogContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Posts.ToList());
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View(new Post { Date = DateTime.Now });
        }

        [Authorize(Roles = "admin")]
        public IActionResult Edit(int? id)
        {
            var post = _context.Posts.SingleOrDefault(p => p.Id == id);
            return View(post);
        }
        
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int? id)
        {
            var post = _context.Posts.Include(p => p.Comments).SingleOrDefault(s => s.Id == id);
            return View(post);
        }

        [HttpPost]
        public ActionResult Create(Post post)
        {
            post.UserId = User.GetLoggedUserId();
            _context.Posts.Add(post);
            _context.SaveChanges();
            return RedirectToAction("Index");
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

        [Authorize(Roles = "admin, user")]
        public ActionResult FullPost(int id)
        {
            var post = _context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
            return View(new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                Date = post.Date,
                Comment = new Comment { Date = DateTime.Now, PostId = post.Id},
                Comments = post.Comments.ToArray()
            });
        }
    }
}
