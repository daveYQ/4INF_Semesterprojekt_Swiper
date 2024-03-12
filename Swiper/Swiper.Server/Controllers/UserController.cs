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
        public async Task<IActionResult> Index()
        {
            return Ok(_context.Users.ToList());
        }

        // GET: UserController/Details/5
        [HttpGet("/{id}", Name = "GetData")]
        public async Task<IActionResult> Details(int id)
        {
            User? user = _context.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            return Ok(user);
        }

        [HttpDelete("Delete/{id}", Name = "DeleteUser")]
        public async Task<IActionResult> Delete(int id)
        {
            User? user = _context.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            _context.Users.Remove(user);
            _context.SaveChangesAsync();

            return Ok(user);
        }

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
        [HttpPut("Edit", Name = "GetEdit")]
        public async Task<ActionResult> Edit(User user)
        {
            User? user2 = _context.Users.Find(user.Id);

            if (user2 is null)
            {
                return BadRequest("User not found");
            }

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok(user);    
        }
    }
}
