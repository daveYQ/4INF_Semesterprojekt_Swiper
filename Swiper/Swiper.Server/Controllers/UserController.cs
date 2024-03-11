using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swiper.Server.Models;

namespace Swiper.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private UserContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, UserContext context)
        {
            this._logger = logger;
            this._context = context;
        }

        // GET: UserController
        [HttpGet(Name = "GetUsers")]
        public IActionResult Index()
        {
            return Ok(_context.Users.ToList());
        }

        // GET: UserController/Details/5
        [HttpGet("Details/{id}", Name = "GetData")]
        public User Details(int id)
        {
            return _context.Users.Find(id);
        }

        // GET: UserController/Create
        /*[HttpGet("Create", Name = "GetCreate")]
        public User Create(User user)
        {
            _context.Users.Add(user);

            _context.SaveChanges();
            
            return user;
        }*/

        // POST: UserController/Create
        [HttpPost("Create", Name ="PostCreate")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok(user);
            }
            catch
            {
                return BadRequest();
            }
        }

        // GET: UserController/Edit/5
        [HttpGet("Edit/{id}", Name = "GetEdit")]
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost("Edit", Name = "PostEdit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        [HttpGet("Delete", Name = "GetDelete")]
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost("Delete", Name = "PostDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
