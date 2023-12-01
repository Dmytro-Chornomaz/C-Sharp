using Finance_Organizer.Database;
using Finance_Organizer.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Finance_Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string _MySecret;
        private readonly string _MyIssuer;
        private readonly string _MyAudience;
        private readonly ILogger<LoginController> _Logger;
        private readonly ApplicationDbContext _Context;


        public LoginController(IConfiguration configuration, ILogger<LoginController> logger, ApplicationDbContext context)
        {
            _MySecret = configuration.GetValue<string>("Auth:Secret")!;
            _MyIssuer = configuration.GetValue<string>("Auth:Issuer")!;
            _MyAudience = configuration.GetValue<string>("Auth:Audience")!;
            _Logger = logger;
            _Context = context;
        }

        // The function that generates a security token.
        [HttpPost("GenerateToken")]
        public async Task<ActionResult<string>> GenerateTokenAsync([FromBody] LoginModel request)
        {
            _Logger.LogInformation("*** Method GenerateTokenAsync started. ***");

            if (request.Login == null || request.Password == null)
            {
                _Logger.LogInformation("*** Request is null. ***");
                return BadRequest();
            }

            SaltedHash saltedHash = new SaltedHash();
            string hashedRequestPassword = saltedHash.ComputeSaltedHash(request.Password);

            Person? person = await _Context.GetPersonByNameAndPasswordAsync(request.Login, hashedRequestPassword);

            if (person != null)
            {
                _Logger.LogInformation("*** The person was found in the database. ***");
                var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_MySecret));
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, person.Name),
                    }),
                    Expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(15)),
                    Issuer = _MyIssuer,
                    Audience = _MyAudience,
                    SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                SecurityToken secutityToken = tokenHandler.CreateToken(tokenDescriptor);
                _Logger.LogInformation("*** The token was successfully created. ***");
                string token = tokenHandler.WriteToken(secutityToken);
                //return token;

                var responseObj = new
                {
                    accessToken = token,
                    userName = person.Name
                };

                string response = JsonSerializer.Serialize(responseObj);
                return response;
            }
            else
            {
                _Logger.LogInformation("*** The person was not found in the database. ***");
                return Unauthorized();
            }            
        }        

        // The function that verifies the token compliance.
        [HttpGet("VerifyToken")]
        public bool VerifyToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_MySecret));
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _MyIssuer,
                    ValidAudience = _MyAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                _Logger.LogWarning("*** The token is incorrect. ***");
                return false;
            }
            _Logger.LogInformation("*** The token is correct. ***");
            return true;
        }
    }
}
