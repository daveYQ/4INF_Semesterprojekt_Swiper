using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swiper.Server.Models;

namespace Swiper.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RelationshipController : Controller
    {
        private readonly UserContext _context;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public RelationshipController(ILogger<UserController> logger, UserContext context, IMapper mapper)
        {
            this._logger = logger;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("/Get/{id}", Name = "GetRelationshipsByUser")]
        public async Task<IActionResult> RelationshipsByUser(int id)
        {
            IEnumerable<Relationship> rels = _context.Relationships.Where(rel => rel.UserA.Id == id || rel.UserB.Id == id);

            return Ok(rels);
        }

        //TODO: Request body as list of users (only 2 tho)
        [HttpPost("Create", Name = "Create")]
        public async Task<IActionResult> Create(UserDTO userADTO, UserDTO userBDTO)
        {
            if(userADTO.Id is null || userBDTO is null) 
            {
                return BadRequest();
            }

            Relationship rel = new Relationship();
            rel.UserA = _context.Users.Find(userADTO.Id);
            rel.UserB = _context.Users.Find(userBDTO.Id);

            _context.Relationships.Add(_mapper.Map<Relationship>(rel));
            await _context.SaveChangesAsync();

            return Ok(rel);
        }

        [HttpPut("Edit", Name = "Edit")]
        public async Task<IActionResult> Update(RelationshipDTO relationshipDTO)
        {
            Relationship? rel = _context.Relationships.Find(relationshipDTO.Id);

            if (rel is null)
            {
                return BadRequest();
            }

            if (relationshipDTO.ALikedB.HasValue)
            {
                rel.ALikedB = relationshipDTO.ALikedB.Value;
            }
            if (relationshipDTO.BLikedA.HasValue)
            {
                rel.BLikedA = relationshipDTO.BLikedA.Value;
            }

            _context.Update(rel);
            await _context.SaveChangesAsync();

            return Ok(relationshipDTO);
        }
    }
}
