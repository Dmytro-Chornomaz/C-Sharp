using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Finance_Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string MySecret;
        private readonly string MyIssuer;
        private readonly string MyAudience;
        private readonly ILogger<LoginController> Logger;

        public LoginController(IConfiguration configuration, ILogger<LoginController> logger)
        {
            MySecret = configuration.GetValue<string>("Auth:Secret")!;
            MyIssuer = configuration.GetValue<string>("Auth:Issuer")!;
            MyAudience = configuration.GetValue<string>("Auth:Audience")!;
            Logger = logger;
        }

        [HttpPost]
        public string GenerateToken([FromBody] LoginModel request)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, request.Login),                    
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = MyIssuer,
                Audience = MyAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            Logger.LogInformation($"*** The token was successfully created. ***");
            return tokenHandler.WriteToken(token);
        }

        [HttpGet]
        public bool VerifyToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(MySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = MyIssuer,
                    ValidAudience = MyAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                Logger.LogWarning($"*** The token is incorrect. ***");
                return false;
            }
            Logger.LogInformation($"*** The token is correct. ***");
            return true;
        }
    }
}
