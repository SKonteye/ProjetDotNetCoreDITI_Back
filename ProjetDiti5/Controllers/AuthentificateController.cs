using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using ProjetDiti5.Model.Authentification;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System;

namespace ProjetDiti5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtManager _jwtManager;
        public AuthentificateController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, JwtManager jwtManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _jwtManager = jwtManager;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel
        model)
        {
            var user = await
            _userManager.FindByNameAsync(model.Username);
            if (user != null && await
            _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await
                _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role,
                    userRole));
                }
                var token = _jwtManager.CreateToken(authClaims);
                var refreshToken = _jwtManager.GenerateRefreshToken();
                _ =
                int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out
                int refreshTokenValidityInDays);
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime =
                DateTime.Now.AddDays(refreshTokenValidityInDays);
                await _userManager.UpdateAsync(user);
                return Ok(new
                {
                    Token = new
                JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await
            _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return
                StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User already exists!"
                });
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new Response
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    });
            return Ok(new Response
            {
                Status = "Success",
                Message =
            "User created successfully!"
            });
        }
        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await
            _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return
                StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User already exists!"
                });
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user,
            model.Password);
            if (!result.Succeeded)
                return
                StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User creation failed! Please check user details and try again."
                });
            if (!await
            _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new
                IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new
                IdentityRole(UserRoles.User));
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user,
                UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user,
                UserRoles.User);
            }
            return Ok(new Response
            {
                Status = "Success",
                Message =
            "User created successfully!"
            });
        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }
            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            var principal = _jwtManager.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            string username = principal.Identity.Name;

            var user = await _userManager.FindByNameAsync(username);
            if (user == null || user.RefreshToken != refreshToken ||
            user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }
            var newAccessToken = _jwtManager.CreateToken(principal.Claims.ToList());
            var newRefreshToken = _jwtManager.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return new ObjectResult(new
            {
                accessToken = new
            JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }
        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return NoContent();
        }
        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }
            return NoContent();
        }
    }
}


