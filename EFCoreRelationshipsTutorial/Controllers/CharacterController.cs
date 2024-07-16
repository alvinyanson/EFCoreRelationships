using EFCoreRelationshipsTutorial.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreRelationshipsTutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;

        public CharacterController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userId)
        {
            var characters = await _context.Characters
                .Where(c => c.UserId == userId)
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .ToListAsync();

            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDTO request)
        {

            var user = await _context.Users.FindAsync(request.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var newCharacter = new Character
            {
                Name = request.Name,
                RpgClass = request.RpgClass,
                UserId = user.Id,
            };

            _context.Characters.Add(newCharacter);
            await _context.SaveChangesAsync();

            return await Get(newCharacter.UserId);
        }

        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDTO request)
        {

            var character = await _context.Characters.FindAsync(request.CharacterId);

            if (character == null)
            {
                return NotFound();
            }

            var newWeapon = new Weapon
            {
                Name = request.Name,
                Damage = request.Damage,
                CharacterId = character.Id,
            };

            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();

            return character;
        }

        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDTO request)
        {

            var character = await _context.Characters
                .Where(c => c.Id == request.CharacterId)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync();

            if (character == null)
            {
                return NotFound();
            }

            var skill = await _context.Skills.FindAsync(request.SkillId);

            if (skill == null)
            {
                return NotFound();
            }

            character.Skills.Add(skill);

            await _context.SaveChangesAsync();

            return character;
        }

    }
}
