using AutoMapper;
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
        private readonly UserContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, UserContext context, IMapper mapper)
        {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
        }

        // GET: UserController
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> Index()
        {
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(this._context.Users.ToList()));
        }

        // GET: UserController/Details/5
        [HttpGet("/{id}", Name = "GetUser")]
        public async Task<IActionResult> Details(int id)
        {
            User? user = _context.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            return Ok(_mapper.Map<UserDTO>(user));
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

            return Ok(_mapper.Map<UserDTO>(user));
        }

        // POST: UserController/Create
        [HttpPost("Create", Name = "PostCreate")]
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
        public async Task<ActionResult> Edit(UserDTO userDTO)
        {
            User? user = _context.Users.Find(userDTO.Id);

            if (user is null)
            {
                return BadRequest("User not found");
            }

            if (userDTO.Name is not null)
            {
                user.Name = userDTO.Name;
            }
            if (userDTO.Email is not null)
            {
                user.Email = userDTO.Email; 
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
