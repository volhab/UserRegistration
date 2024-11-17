using dotnet_registration_api.Data.Entities;
using dotnet_registration_api.Data.Models;
using dotnet_registration_api.Services;
using Microsoft.AspNetCore.Mvc;
using dotnet_registration_api.Helpers;

namespace dotnet_registration_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        public UsersController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody]LoginRequest model)
        {
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return NotFound();
            var u = await _userService.GetByUserName(model.Username);
            if ( u == null )
                return BadRequest();
            if (u.PasswordHash != HashHelper.HashPassword(model.Password))
                return BadRequest();
            return Ok(u);
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody]RegisterRequest model)
        {
            if (string.IsNullOrEmpty(model.Password))
                return BadRequest();        
            var u = await _userService.GetByUserName(model.Username);
            if ( u != null )
                return BadRequest();
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.Username,
                PasswordHash = HashHelper.HashPassword(model.Password)
            };
            var res = await _userService.Register(newUser);
            return Ok(res);
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            return await _userService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var u = await _userService.GetById(id);
            if (u == null)
                return NotFound();
            return Ok(u);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> Update(int id, [FromBody]UpdateRequest model)
        {
            var u = await _userService.GetById(id);
            if (u == null)
                return NotFound();
            if (u.PasswordHash != HashHelper.HashPassword(model.OldPassword))
                return BadRequest();
            var u2 = await _userService.GetByUserName(model.Username);
            if (u2.Id != u.Id)
                 return BadRequest();
            u.FirstName = model.FirstName;
            u.LastName = model.LastName;
            u.Username = model.Username;
            u.PasswordHash = HashHelper.HashPassword(model.NewPassword);
            await _userService.Update(u);
            return Ok(u);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var res = await _userService.Delete(id);
            if (res)
                return Ok();
            return NotFound();
        }
    }
}
