using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserApi.Repository;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserDetails(int userId)
        {
            var (name, phoneNumber) = await _userRepository.GetUserDetailsAsync(userId);

            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            return Ok(new { Name = name, PhoneNumber = phoneNumber });
        }
    }
}
