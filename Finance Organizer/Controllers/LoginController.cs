using Finance_Organizer.Database;
using Finance_Organizer.Model;
using FluentValidation;
using FluentValidation.Results;
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
        private readonly IValidator<LoginModel> _LoginModelValidator;
        private readonly SaltedHash _SaltedHash;


        public LoginController(IConfiguration configuration, ILogger<LoginController> logger, ApplicationDbContext context,
            IValidator<LoginModel> loginModelValidator, SaltedHash saltedHash)
        {
            _MySecret = configuration.GetValue<string>("Auth:Secret")!;
            _MyIssuer = configuration.GetValue<string>("Auth:Issuer")!;
            _MyAudience = configuration.GetValue<string>("Auth:Audience")!;
            _Logger = logger;
            _Context = context;
            _LoginModelValidator = loginModelValidator;
            _SaltedHash = saltedHash;
        }

        // The function that generates a security token.
        [HttpPost("GenerateToken")]
        public async Task<ActionResult<string>> GenerateTokenAsync([FromBody] LoginModel request)
        {
            _Logger.LogInformation("*** Method GenerateTokenAsync started. ***");

            ValidationResult loginModelResult = await _LoginModelValidator.ValidateAsync(request);

            if (!loginModelResult.IsValid)
            {
                foreach (var error in loginModelResult.Errors)
                {
                    _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                }

                return BadRequest();
            }
            
            string hashedRequestPassword = _SaltedHash.ComputeSaltedHash(request.Password);

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
                _Logger.LogInformation("*** Login or/and password are wrong. ***");
                return Unauthorized();
            }            
        }        
    }
}
