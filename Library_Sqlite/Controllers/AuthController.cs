using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Library.Models;
using Library.Services;
using Library.DTOs;

namespace Library.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LibraryContext _context;
        private readonly HashService _hashService;
        private readonly ITokenService _tokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(LibraryContext context,
        HashService hashService, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hashService = hashService;
            _tokenService = tokenService;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserDTO user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userDB == null)
            {
                return Unauthorized();
            }

            var hashResult = _hashService.Hash(user.Password, userDB.Salt);
            if (userDB.Password == hashResult.Hash)
            {
                var response = _tokenService.GenerateToken(user);
                return Ok(response);
            }
            else
            {
                return Unauthorized();
            }
        }
        [Authorize]
        [HttpPost("renewToken")]
        public async Task<ActionResult> RenewToken([FromBody] UserDTO user)
        {
            var userDB = await _context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);
            if (userDB == null)
            {
                return Unauthorized();
            }

            var response = _tokenService.GenerateToken(user);
            return Ok(response);
        }

    }
}

