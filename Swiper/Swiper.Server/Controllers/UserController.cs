using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swiper.Server.DBContexts;
using Swiper.Server.Models;

namespace Swiper.Server.Controllers
{
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

            //InitializeDB().Wait();
        }

        [HttpGet("/Health")]
        public async Task<IActionResult> Health()
        {
            return Ok("Up");
        }

        //[Authorize]
        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> Index()
        {
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(this._userManager.Users.Include(user => user.Images).ToList().Where(u =>
            {
                return !u.IsBlocked && u.Images.Count > 0;
            })));
        }

        [Authorize(Roles = "Moderator,Administrator")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                return BadRequest("User not found.");
            }

            return Ok(_mapper.Map<UserModDTO>(user));
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            User? user = _context.Users.Include(u=> u.Images).ToList().Find(u => u.Id == id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }
            ;
            if (user == (await _userManager.GetUserAsync(User)))
            {
                await _signInManager.SignOutAsync();
            }

            if (user.Images is not null)
            {
                _context.Images.RemoveRange(user.Images);
            }
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<UserModDTO>(user));
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var users = _context.Users.Include(u => u.Images).ToList();

            _context.Images.RemoveRange(_context.Images);

            _context.RemoveRange(users);
            await _context.SaveChangesAsync();

            await _signInManager.SignOutAsync();

            return Ok("All users deleted!");
        }

        [HttpPost("Register")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromBody] UserCreationDTO userCreationDto)
        {
            if ((User is not null) && User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is logged in.");
            }

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

                string err = "";

                foreach (var error in result.Errors)
                {
                    err += error.Description + Environment.NewLine;
                }

                return BadRequest(err);
            }
            catch
            {
                return BadRequest("Req");
            }
        }

        [HttpGet("LogIn")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(string email, string password, bool rememberMe)
        {
            User? user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                return NotFound("User not found!");
            }

            //await _signInManager.SignOutAsync();
            var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, false);

            if (!result.Succeeded)
            {
                return BadRequest("Invalid Password!");
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [Authorize]
        [HttpPost("LogOff")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        //[Authorize]
        [HttpGet("CurrentUser")]
        public async Task<IActionResult> IsLoggedIn()
        {
            if ((User is not null) && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                return Ok(_mapper.Map<UserDTO>(user));
            }
            return Ok();
        }

        [Authorize]
        [HttpPost("Like")]
        public async Task<IActionResult> Like(string id)
        {
            if ((User is not null) && !User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not logged in");
            }

            User? target = await _userManager.FindByIdAsync(id);

            if (target is null)
            {
                return BadRequest("Target does not exist!");
            }

            User? user = await _userManager.GetUserAsync(User);

            if (user == target)
            {
                return BadRequest("User cannot like themselves!");
            }

            if (user.LikedUsers is null)
            {
                user.LikedUsers = new List<User>();
            }

            if (user.LikedUsers.Contains(target))
            {
                return BadRequest("User is already liked!");
            }

            user.LikedUsers.Add(target);

            await _userManager.UpdateAsync(user);

            return Ok("User is liked now.");
        }

        [Authorize]
        [HttpGet("Matches")]
        public async Task<IActionResult> GetMatches()
        {
            if ((User is not null) && !User.Identity.IsAuthenticated)
            {
                return Unauthorized("User is not logged in");
            }

            //User? user = await _userManager.GetUserAsync(User);
            User? user = this._userManager.Users.Include(user => user.LikedUsers).ToListAsync().Result.Find(u => User.Identity.Name == u.UserName);
            if (user is null)
            {
                return BadRequest("User is not logged in!");
            }

            if (user.LikedUsers is null)
            {
                user.LikedUsers = new List<User>();
                await _userManager.UpdateAsync(user);
                return Ok(user.LikedUsers);
            }

            List<User> matches = new();

            foreach (User target in user.LikedUsers)
            {
                if (target.LikedUsers is null)
                {
                    continue;
                }

                if (target.LikedUsers.Contains(user))
                {
                    matches.Add(target);
                }
            }

            return Ok(_mapper.Map<List<UserDTO>>(matches));
        }

        [Authorize]
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

            user = await _userManager.FindByIdAsync(user.Id);

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [Authorize(Roles = "Moderator,Administrator")]
        [HttpGet("Block/{id}")]
        public async Task<IActionResult> BlockUser(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);

            if(user is null)
            {
                return NotFound();
            }

            user.IsBlocked = true;
            await _userManager.UpdateAsync(user);

            return Ok(_mapper.Map<UserModDTO>(user));
        }
    }
}
