using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeWork7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string MySecret;
        private readonly string MyIssuer;
        private readonly string MyAudience;

        public LoginController(IConfiguration configuration)
        {
            MySecret = configuration.GetValue<string>("Auth:Secret");
            MyIssuer = configuration.GetValue<string>("Auth:Issuer");
            MyAudience = configuration.GetValue<string>("Auth:Audience");

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
                return false;
            }
            return true;
        }
    }

    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
