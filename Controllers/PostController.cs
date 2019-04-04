using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MessageBoard.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace MessageBoard.Controllers
{
    [Route("posts")]
    public class PostController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private DashboardVM DashViewModel
        {
            get 
            { 
                return new DashboardVM() 
                {
                    AllPosts = dbContext.Posts.Include(p => p.Creator).ToList()
                };
            }
        }
        private MainContext dbContext;
        public PostController(MainContext context)
        {
            dbContext = context;
        }
        // localhost:5000/posts
        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            
            if(UserSession == null)
                return RedirectToAction("Index", "Home");

        

            // all posts ordered by desc on CreatedAt with username of DevMan26

            return View(DashViewModel);
        }
        // localhost:5000/posts/create POST
        [HttpPost("create")]
        public IActionResult Create(DashboardVM model)
        {
            Post newPost = model.NewPost;
            if(ModelState.IsValid)
            {
                // update newPost with user in session!
                newPost.UserId = (int)UserSession;
                dbContext.Posts.Add(newPost);
                dbContext.SaveChanges();


                return RedirectToAction("Index");
            }
            return View("Index", DashViewModel);
        }

        [HttpGet("{postId}")]
        public IActionResult Show(int postId)
        {
            // list of posts
            Post thePost = dbContext.Posts
                .Include(p => p.VotesGiven)
                .ThenInclude(v => v.Voter)
                .FirstOrDefault(p => p.PostId == postId);
            if(thePost == null)
                return RedirectToAction("Index");

            return View(thePost);
        }

        [HttpPost("update/{postId}")]
        public IActionResult Update(Post post, int postId)
        {
            // list of posts
            if(ModelState.IsValid)
            {
                Post toUpdate = dbContext.Posts.FirstOrDefault(p => p.PostId == postId);
                toUpdate.Content = post.Content;
                toUpdate.Topic = post.Topic;
                toUpdate.UpdatedAt = DateTime.Now;
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Show", post);

        }

        [HttpGet("delete/{postId}")]
        public IActionResult Delete(int postId)
        {
            // list of posts
            Post toDelete = dbContext.Posts.FirstOrDefault(p => p.PostId == postId);

            dbContext.Posts.Remove(toDelete);
            dbContext.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpGet("vote/{postId}/{isUpvote}")]
        public IActionResult Vote(int postId, bool isUpvote)
        {
            // we need to make sure someone is logged in
            if(UserSession == null)
                return RedirectToAction("Index", "Home");

            Vote newVote = new Vote()
            {
                UserId = (int)UserSession,
                PostId = postId,
                IsUpvote = isUpvote
            };

            dbContext.Votes.Add(newVote);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
