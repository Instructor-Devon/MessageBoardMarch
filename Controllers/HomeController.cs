using MessageBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MessageBoard.Controllers
{
    public class HomeController : Controller
    {
        private int? UserSession
        {
            get { return HttpContext.Session.GetInt32("UserId"); }
            set { HttpContext.Session.SetInt32("UserId", (int)value); }
        }
        private MainContext dbContext;
        public HomeController(MainContext context)
        {
            dbContext = context;
        }
        // localhost:5000
        [Route("")]
        public IActionResult Index()
        {
            if(UserSession != null)
                return RedirectToAction("Index", "Post");
            return View();
        }
        [HttpGet("{userId}")]
        public IActionResult Show(int userId)
        {
            User daUser = dbContext.Users
                .Include(u => u.Posts)
                .FirstOrDefault(u => u.UserId == userId);

            var favTopicGrouping = daUser.Posts
                .GroupBy(p => p.Topic);
            var ordered = favTopicGrouping.OrderByDescending(gr => gr.Count());
            return View(daUser);
        }
        [HttpPost("create")]
        public IActionResult Create(User newUser)
        {
            if(ModelState.IsValid)
            {
                // are we ready NO!?

                // check uniqueness of email!
                if(dbContext.Users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email is in use partner.");
                    return View("Index");
                }

                // Hash the ol password
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                string hashedPw = hasher.HashPassword(newUser, newUser.Password);

                // update user with newly hashed pw
                newUser.Password = hashedPw;


                dbContext.Users.Add(newUser);
                dbContext.SaveChanges();

                // flash a TempData message
                TempData["loginMessage"] = "You may now log in!";
                return RedirectToAction("Login");
            }
            return View("Index");
        }
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("loginattempt")]
        public IActionResult LoginAttempt(LoginUser logUser)
        {
            if(ModelState.IsValid)
            {
                // Attempt to query for user with logUser.Email

                User userAttempt = dbContext.Users.FirstOrDefault(u => u.Email == logUser.Email);

                if(userAttempt == null)
                {
                    ModelState.AddModelError("Email", "Invalid Email/Password");
                    return View("Login");
                }
                
                else
                {
                    // Verify the password
                    PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                    PasswordVerificationResult result = hasher.VerifyHashedPassword(logUser, userAttempt.Password, logUser.Password);

                    if(result == 0)
                    {
                        ModelState.AddModelError("Email", "Invalid Email/Password");
                        return View("Login");
                    }

                    // logem in partner
                    UserSession = userAttempt.UserId;
                    return RedirectToAction("Index", "Post");
                }

                // log in the buckeroo
            }
            return View("Login");
        }
        [HttpGet("logout")]
        public RedirectToActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}