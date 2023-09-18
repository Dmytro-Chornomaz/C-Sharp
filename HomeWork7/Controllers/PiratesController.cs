using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace HomeWork7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiratesController : ControllerBase
    {
        private readonly ApplicationContext _Context;
        private readonly ILogger<PiratesController> _Logger;
        private readonly IValidator<Pirate> _Validator;

        public PiratesController(ApplicationContext context, ILogger<PiratesController> logger, IValidator<Pirate> validator)
        {
            _Context = context;
            _Logger = logger;
            _Validator = validator;
        }

        [HttpGet]
        public async Task<ActionResult<List<Pirate>>> GetCrewAsync()
        {
            if (_Context.PiratesDB.Count() != 0)
            {
                _Logger.LogInformation("Getting all crew.");
                return await _Context.PiratesDB.ToListAsync();
            }
            else
            {
                _Logger.LogWarning("The crew is empty.");
                return BadRequest();
            }
            
        }

        [HttpGet("byId/{id}")]
        public async Task<ActionResult<Pirate?>> GetPirateAsync([FromRoute] int id)
        {
            Pirate? pirate = await _Context.PiratesDB.FirstOrDefaultAsync(x => x.Id == id);

            if (pirate != null)
            {
                _Logger.LogInformation($"Getting pirate by id {id}.");
                return pirate;
            }
            else
            {
                _Logger.LogWarning($"No pirate with id {id}");
                return BadRequest();
            }

        }

        [HttpGet("byName/{name}")]
        public async Task<ActionResult<Pirate?>> GetPirateByNameAsync([FromRoute] string name)
        {
            Pirate? pirate = await _Context.PiratesDB.FirstOrDefaultAsync(x => x.Name == name);

            if (pirate != null)
            {
                _Logger.LogInformation($"Getting pirate by name {name}.");
                return pirate;
            }
            else
            {
                _Logger.LogWarning($"No pirate with name {name}");
                return BadRequest();
            }

        }

        [HttpPost]
        public async Task<ActionResult<Pirate>> AddPirateAsync([FromBody] Pirate pirate)
        {
            ValidationResult result = _Validator.Validate(pirate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                }

                return BadRequest();
            }

            if (await _Context.PiratesDB.FirstOrDefaultAsync(x => x.Name == pirate.Name) == null)
            {
                _Logger.LogInformation($"Creating pirate with name {pirate.Name}");
                var newPirate = new Pirate
                {
                    Id = _Context.PiratesDB.Max(x => x.Id) + 1,
                    Name = pirate.Name,
                    Description = pirate.Description,
                    Age = pirate.Age
                };
                await _Context.PiratesDB.AddAsync(newPirate);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation($"Created pirate with id {newPirate.Id} and name {pirate.Name}");
                return newPirate;
            }
            else
            {
                _Logger.LogWarning($"A pirate with the name {pirate.Name} already exists.");
                return BadRequest();
            }
            

        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeletePirateAsync([FromRoute] int id)
        {
            Pirate? pirate = await _Context.PiratesDB.FirstOrDefaultAsync(x => x.Id == id);

            if (pirate != null)
            {
                _Logger.LogInformation($"Deleting pirate with id {id}");                
                _Context.PiratesDB.Remove(pirate);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation($"Deleted pirate with id {id}");
                return NoContent();
            }
            else
            {
                _Logger.LogWarning($"No pirate with id {id}");
                return BadRequest();
            }
        }

        //[PirateFilter]
        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Pirate>> ChangePirateAsync([FromRoute] int id, [FromBody] Pirate pirate)
        {
            Regex regex = new Regex(@"[A-Z](\w*)");
            MatchCollection matches = regex.Matches(pirate.Name);

            if (matches.Count == 0)
            {
                _Logger.LogWarning($"Incorrect pirate name {pirate.Name}");
                return BadRequest();
            }

            Pirate? pirateForChanging = await _Context.PiratesDB.FirstOrDefaultAsync(x => x.Id == id);

            if (pirateForChanging != null)
            {
                _Logger.LogInformation($"Changing pirate with id {id}");
                pirateForChanging.Name = pirate.Name;
                pirateForChanging.Age = pirate.Age;
                pirateForChanging.Description = pirate.Description;
                _Context.PiratesDB.Update(pirateForChanging);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation($"Changed pirate with id {id}");
                return pirateForChanging;
            }
            else
            {
                _Logger.LogWarning($"No pirate with id {id}");
                return BadRequest();
            }

        }

        public class Pirate
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public string Description { get; set; }
        }
    }
}
