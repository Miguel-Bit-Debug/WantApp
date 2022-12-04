using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WantApp.Domain.DTOs.Token;

namespace WantApp.API.Controllers.Security;

[AllowAnonymous]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;

    public TokenController(UserManager<IdentityUser> userManager,
                           IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> Index([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return BadRequest("User not found.");
        }

        if (!_userManager.CheckPasswordAsync(user, request.Password).Result)
        {
            return BadRequest();
        }

        var claims = await _userManager.GetClaimsAsync(user);

        var subject = new ClaimsIdentity
            (
                new Claim[]
                {
                    new Claim(ClaimTypes.Email, request.Email)
                });

        subject.AddClaims(claims);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = subject,
            SigningCredentials =
                    new SigningCredentials(
                        new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(_configuration["SecretKey"])),
                            SecurityAlgorithms.HmacSha256Signature),
            Audience = _configuration["Audience"],
            Issuer = _configuration["Issuer"],
            Expires = DateTime.UtcNow.AddHours(1)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }
}
