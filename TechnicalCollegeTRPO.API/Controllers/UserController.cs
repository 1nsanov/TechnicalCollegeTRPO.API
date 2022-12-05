using AspTestStage.Database;
using AspTestStage.Database.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AspTestStage.Dto;
using AspTestStage.Dto.Utillity;

namespace AspTestStage.BaseClasses;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IConfiguration _config;
    public UserController(AppDbContext db, IConfiguration config) : base(db)
    {
        _config = config;
    }

    [AllowAnonymous]
    [HttpPost("SingIn")]
    public IActionResult Login([FromBody] UserSingIn userSingIn)
    {
        var user = Authenticate(userSingIn);

        if (user is null) return Unauthorized("User not found");

        var token = GenerateTokenUser(user);
        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost("SingUp")]
    public async Task<IActionResult> SingUp([FromBody] UserSingUp user)
    {
        var userDouble = GetUserByUsername(user.Username);
        if (userDouble) return BadRequest($"User with username {user.Username} already exits");

        var entity = new User()
        {
            Username = user.Username,
            Password = user.Password,
            Email = user.Email,
            FullName = user.FullName,
            Birthdate = user.Birthdate,
            Phone = user.Phone,
            RoleId = user.RoleId
        };

        var token = GenerateTokenUser(entity);
        entity.RefreshToken = token.RefreshToken;
        entity.RefreshTokenExpiryTime = token.Expiration;

        await _db.Users.AddAsync(entity);
        await _db.SaveChangesAsync();

        return Ok(token);
    }

    [Authorize]
    [HttpPost("Test")]
    public IActionResult Test()
    {
        return Ok("You is login!");
    }

    [Authorize(Roles = "teacher")]
    [HttpPost("TestTeacher")]
    public IActionResult TestTeacher()
    {
        return Ok("You is teacher!");
    }

    [Authorize(Roles = "student")]
    [HttpPost("TestStudent")]
    public IActionResult TestStudent()
    {
        return Ok("You is student!");
    }

    private bool GetUserByUsername(string username)
    {
        var user = _db.Users.FirstOrDefault(u => u.Username == username);
        return user is not null;
    }

    [AllowAnonymous]
    [HttpPost("RefreshToken")]
    public IActionResult RefreshToken(TokenUser? tokenUser)
    {
        if (tokenUser is null) return BadRequest("Invalid client request");

        var accessToken = tokenUser.AccessToken;
        var refreshToken = tokenUser.RefreshToken;

        var principal = GetPrincipalFromExpiredToken(accessToken);
        if (principal is null) return BadRequest("Invalid access token or refresh token");

        var claim = principal.Claims.FirstOrDefault(claim => claim.Type.Contains("nameidentifier"));
        var username = claim?.Value;

        var user = _db.Users.FirstOrDefault(user => user.Username == username);
        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Invalid access token or refresh token");

        var newToken = GenerateTokenUser(user);

        user.RefreshToken = newToken.RefreshToken;

        return Ok(newToken);
    }

    private TokenUser GenerateTokenUser(User user)
    {
        var token = GenerateToken(user);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = GenerateRefreshToken();
        return new TokenUser
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        };
    }

    private JwtSecurityToken GenerateToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var role = _db.Roles.FirstOrDefault(r => r.Id == user.RoleId);
        if (role == null) throw new ArgumentNullException(nameof(role));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FullName),
            new Claim(ClaimTypes.Role, role.Code)
        };

        return new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddSeconds(60),
            signingCredentials: credentials);
    }

    private User? Authenticate(UserSingIn userLogin)
    {
        var currentUser = _db.Users.FirstOrDefault(o => o.Username == userLogin.Username && o.Password == userLogin.Password);
        return currentUser ?? null;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");

        return principal;
    }
}