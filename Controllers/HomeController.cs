using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using beltExam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace beltExam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private MyContext _context;
        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("/users/create")]
        public IActionResult createUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.Any(s => s.Email == newUser.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use!");
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }



        [HttpPost("/users/login")]
        public IActionResult PostLogin(LoginUser loginUser)
        {
            if (ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(d => d.Email == loginUser.LoginEmail);
                if (userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                PasswordHasher<LoginUser> hasher = new PasswordHasher<LoginUser>();
                var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [HttpPost("/users/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        //===================================LOGIN=======================================

        [HttpGet("/DashBoard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            var AllMeetUps = _context.MeetUps.Include(e => e.GuestList).ThenInclude(w => w.MeetUp).Include(e => e.Host).OrderBy(d => d.DateOfMeetUp).ToList();
            ViewBag.UserId =  (int)HttpContext.Session.GetInt32("UserId");
            ViewBag.name = _context.Users.FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("UserId"));
            return View("Dashboard", AllMeetUps);
        }


        [HttpGet("/meetups/new")]
        public IActionResult newMeetUp()
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            return View("NewMeetUp");
        }



        [HttpPost("meetup/create/post")]
        public IActionResult newMeetUpPost(MeetUp newMeetUp)
        {
            if (newMeetUp.DateOfMeetUp < DateTime.Now)
                {
                    ModelState.AddModelError("DateOfMeetUp", "Date has to be in the future");
                    return View("newMeetUp");
                }
            if (ModelState.IsValid)
            {
                newMeetUp.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Add(newMeetUp);
                _context.SaveChanges();
                return Redirect($"/meetup/view/{newMeetUp.MeetUpId}");
            }
            else
            {
                return View("newMeetUp");
            }
        }


        [HttpGet("/meetup/view/{id}")]
        public IActionResult InfoMeetUp(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            ViewBag.MeetUpInfo = _context.MeetUps.Include(a => a.Host).FirstOrDefault(a => a.MeetUpId == id);
            ViewBag.List = _context.MeetUps.Include(e => e.GuestList).ThenInclude(w => w.User).FirstOrDefault(w => w.MeetUpId == id);
            ViewBag.UserId = (int)HttpContext.Session.GetInt32("UserId");

            return View("InfoMeetUp");
        }




        [HttpPost("/removeguest")]
        public IActionResult RemoveGuest(Managment newManagment)
        {
            Managment removeManagment = _context.Managments.SingleOrDefault(a => a.MeetUpId == newManagment.MeetUpId && a.UserId == newManagment.UserId);
            _context.Remove(removeManagment);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }



        [HttpPost("/addguest")]
        public IActionResult AddGuest(Managment newManagment)
        {
            _context.Add(newManagment);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }



        [HttpPost("/deleteMeetUp")]
        public IActionResult DeleteWedding(MeetUp newManagment)
        {
            MeetUp removeMeetUp = _context.MeetUps.SingleOrDefault(a => a.MeetUpId == newManagment.MeetUpId);
            _context.Remove(removeMeetUp);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }















        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

































