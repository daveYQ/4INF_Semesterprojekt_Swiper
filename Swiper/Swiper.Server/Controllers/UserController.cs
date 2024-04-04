using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swiper.Server.Models;

namespace Swiper.Server.Controllers
{
    //[ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
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

        [HttpGet("/Health")]
        public async Task<IActionResult> Health()
        {
            return Ok("Up");
        }

        [HttpGet(Name = "GetUsers")]
        public async Task<IActionResult> Index()
        {
            return Ok(_mapper.Map<IEnumerable<UserDTO>>(this._context.Users.ToList()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User? user = _context.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            User? user = _context.Users.Find(id);

            if (user is null)
            {
                return BadRequest("User not found.");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserDTO>(user));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]UserCreationDTO userCreationDto)
        {
            try
            {
                await _context.Users.AddAsync(_mapper.Map<User>(userCreationDto));
                await _context.SaveChangesAsync();
                return Ok(userCreationDto);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<ActionResult> Edit([FromBody]UserDTO userDTO)
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

        [HttpPost("{id}/Like/{likeId}")]
        public async Task<IActionResult> LikeUser(int id, int likeId)
        {
            if (id == likeId)
            {
                return BadRequest("Users cannot like themselves.");
            }

            User? userA = await _context.Users.FindAsync(id);
            User? userB = await _context.Users.FindAsync(likeId);
            if (userA is null || userB is null)
            {
                return BadRequest("User not found");
            }

            Relationship? rel = _context.Relationships.Where(rel => rel.UserA.Id == id || rel.UserB.Id == id || rel.UserA.Id == likeId || rel.UserB.Id == likeId).FirstOrDefault();

            if(rel is null)
            {
                rel = new Relationship(userA, userB);
                await _context.Relationships.AddAsync(rel);
            }

            if (id == rel.UserA.Id)
            {
                rel.ALikedB = true;
            }
            else
            {
                rel.BLikedA = true;
            }

            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<RelationshipDTO>(rel));
        }

        [HttpGet("{id}/Matches")]
        public async Task<IActionResult> GetMatches(int id)
        {
            var list = _context.Relationships.Where(r => (r.UserA.Id == id || r.UserB.Id == id) && (r.ALikedB || r.BLikedA));

            var matches = new List<User>();

            foreach (var rel in list)
            {
                if(rel.UserA.Id == id)
                {
                    matches.Add(rel.UserB);
                }
                else
                {
                    matches.Add(rel.UserA);
                }
            }

            return Ok(_mapper.Map<UserDTO>(matches));
        }

        [HttpPost("{id}/ProfilePicture")]
        public async Task<IActionResult> UploadPfp(int id, IFormFile file)
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

                user = _context.Users.Find(id);
                if (user is null)
                {
                    return BadRequest("User does not exist!");
                }

                if (user.Images is null)
                {
                    user.Images = new List<Image>();
                }

                user.Images.Add(image);
            }

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<UserDTO>(user));
        }
    }
}
