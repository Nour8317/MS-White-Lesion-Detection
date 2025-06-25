using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MS_white_lesion_detection.Models;
using MS_white_lesion_detection.DTOs;
using MS_white_lesion_detection.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _config;

    public UserController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUnitOfWork unitOfWork,
        IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _unitOfWork = unitOfWork;
        _config = config;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        var user = new User
        {
            UserName = dto.Gmail,
            Email = dto.Gmail,
            CreatedAt = DateTime.UtcNow
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Gmail);
        if (user == null) return Unauthorized("Invalid credentials");

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
        if (!result.Succeeded) return Unauthorized("Invalid credentials");

        var claims = new[]
         {
            new Claim("id", user.Id.ToString()),  
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );


        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new
        {
            token = tokenString,
            userId = user.Id,
            message = "Login successful"
        });
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok("Logged out");
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("sessions")]
    public async Task<IActionResult> GetUserScanSessions()
    {
        var userIdClaim = User.FindFirst("id")?.Value ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized("User ID not found.");

        var allSessions = await _unitOfWork.ScanSessions.GetAllAsync();
        var userSessions = allSessions
            .Where(s => s.UserId == userId)
            .Select(s => new
            {
                s.Id,
                s.CreatedAt
            });

        return Ok(userSessions);
    }
    [Authorize]
    [HttpGet("sessions/{sessionId}")]
    public async Task<IActionResult> GetSessionDetails(int sessionId)
    {
        var userIdClaim = User.FindFirst("id")?.Value ?? User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            return Unauthorized("User ID not found.");

        var session = await _unitOfWork.ScanSessions.GetByIdAsync(sessionId);
        if (session == null || session.UserId != userId)
            return NotFound("Session not found or access denied.");

        var scans = await _unitOfWork.Scans.GetAllAsync();

        var sessionScans = scans
            .Where(scan => scan.ScanSessionId == sessionId)
            .Select(scan => new
            {
                scan.Id,
                scan.FilePath
            });

        return Ok(new
        {
            SessionId = session.Id,
            session.CreatedAt,
            Scans = sessionScans
        });
    }

}