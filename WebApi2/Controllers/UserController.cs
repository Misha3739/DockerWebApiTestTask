using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApi2.Data;
using WebApi2.Repository;

namespace WebApi2.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("api/user/{id}")]
        public async Task<IActionResult> GetUser(long id)
        {
            User user = await _userRepository.GetAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return BadRequest($"User with {id} was not found!");
        }
        
        [HttpPost("api/user")]
        public async Task<IActionResult> SaveUser(User user)
        {
            if (user == null)
                return BadRequest($"User cannot be null!");
            if(string.IsNullOrEmpty(user.Name))
                return BadRequest($"User should have name!");

            await _userRepository.AddOrUpdateAsync(user);
            return Ok($"User {user.Name} successfully saved!");
        }
    }
}