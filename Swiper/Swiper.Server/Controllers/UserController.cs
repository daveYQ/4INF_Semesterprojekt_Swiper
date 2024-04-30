using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Swiper.Server.DBContexts;
using Swiper.Server.Models;

namespace Swiper.Server.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    [RequireHttps]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public UserController(ILogger<UserController> logger, UserContext context, IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        [HttpGet("/Health")]
        public async Task<IActionResult> Health()
        {
            return Ok("Up");
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> Index()
        {
            /*return Ok(_mapper.Map<IEnumerable<UserDTO>>(
                this._context.Users
                .Include(user => user.Images)
                .ToList()
                ));*/

            return Ok(_mapper.Map<IEnumerable<UserDTO>>(this._userManager.Users.Include(user => user.Images)));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return BadRequest("User not found.");
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            User? user = _context.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }
            ;
            if (user == (await _userManager.GetUserAsync(User)))
            {
                await _signInManager.SignOutAsync();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var users = _context.Users;

            _context.RemoveRange(users);
            await _context.SaveChangesAsync();

            await _signInManager.SignOutAsync();

            return Ok("All users deleted!");
        }

        [HttpPost("Register")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] UserCreationDTO userCreationDto)
        {
            try
            {
                User user = _mapper.Map<User>(userCreationDto);

                var result = await _userManager.CreateAsync(user, userCreationDto.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: true);

                    return Ok(userCreationDto);
                }

                return BadRequest("Bad: " + result.Errors.ToString());
            }
            catch
            {
                return BadRequest("Req");
            }
        }

        [HttpPost("LogIn/{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(string id, string password, bool rememberMe)
        {
            User? user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                return BadRequest("User not found!");
            }

            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("Invalid Password");
        }

        [HttpPost("LogOff")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpGet("LoggedIn")]
        public async Task<IActionResult> IsLoggedIn()
        {
            if ((User is not null) && User.Identity.IsAuthenticated)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [HttpPost]
        public async Task<IActionResult> Like(string id)
        {
            if ((User is not null) && User.Identity.IsAuthenticated)
            {
                return BadRequest("User is not logged in");
            }

            User? target = await _userManager.FindByIdAsync(id);

            if (target is null)
            {
                return BadRequest("Target does not exist!");
            }

            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest("User is not logged in!");
            }

            if (user.LikedUsers is null)
            {
                user.LikedUsers = new List<User>();
            }

            user.LikedUsers.Add(target);

            return Ok("User is liked now.");
        }

        [HttpGet("{id}/Matches")]
        public async Task<IActionResult> GetMatches()
        {
            if ((User is not null) && User.Identity.IsAuthenticated)
            {
                return BadRequest("User is not logged in");
            }

            User? user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest("User is not logged in!");
            }

            if (user.LikedUsers is null)
            {
                user.LikedUsers = new List<User>();
                return Ok(user.LikedUsers);
            }

            List<User> matches = new();

            foreach (User target in user.LikedUsers)
            {
                if (target.LikedUsers is null)
                {
                    target.LikedUsers = new List<User>();
                    continue;
                }

                if (target.LikedUsers.Contains(user))
                {
                    matches.Add(target);
                }
            }

            return Ok(matches);
        }

        [HttpPost("ProfilePicture")]
        public async Task<IActionResult> UploadPfp(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return BadRequest("No image uploaded!");
            }

            User? user;
            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] imageData = memoryStream.ToArray();

                var image = new Image(imageData);

                user = await _userManager.GetUserAsync(User);
                if (user is null)
                {
                    return BadRequest("User is not logged in!");
                }

                if (user.Images is null)
                {
                    user.Images = new List<Image>();
                }

                user.Images.Add(image);
            }

            await _userManager.UpdateAsync(user);
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            user = await _userManager.FindByIdAsync(user.Id);

            return Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
