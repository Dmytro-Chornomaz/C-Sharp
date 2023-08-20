using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public ActionResult<List<Pirate>> GetCrew()
        {
            if (_Context.PiratesDB.Count() != 0)
            {
                _Logger.LogInformation("Getting all crew.");
                return _Context.PiratesDB.ToList();
            }
            else
            {
                _Logger.LogWarning("The crew is empty.");
                return BadRequest();
            }
            
        }

        [HttpGet("byId/{id}")]
        public ActionResult<Pirate?> GetPirate([FromRoute] int id)
        {
            Pirate? pirate = _Context.PiratesDB.FirstOrDefault(x => x.Id == id);

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
        public ActionResult<Pirate?> GetPirateByName([FromRoute] string name)
        {
            Pirate? pirate = _Context.PiratesDB.FirstOrDefault(x => x.Name == name);

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
        public ActionResult<Pirate> AddPirate([FromBody] Pirate pirate)
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

            if (_Context.PiratesDB.FirstOrDefault(x => x.Name == pirate.Name) == null)
            {
                _Logger.LogInformation($"Creating pirate with name {pirate.Name}");
                var newPirate = new Pirate
                {
                    Id = _Context.PiratesDB.Max(x => x.Id) + 1,
                    Name = pirate.Name,
                    Description = pirate.Description,
                    Age = pirate.Age
                };
                _Context.PiratesDB.Add(newPirate);
                _Context.SaveChanges();
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
        public ActionResult DeletePirate([FromRoute] int id)
        {
            Pirate? pirate = _Context.PiratesDB.FirstOrDefault(x => x.Id == id);

            if (pirate != null)
            {
                _Logger.LogInformation($"Deleting pirate with id {id}");                
                _Context.PiratesDB.Remove(pirate);
                _Context.SaveChanges();
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
        public ActionResult<Pirate> ChangePirate([FromRoute] int id, [FromBody] Pirate pirate)
        {
            Regex regex = new Regex(@"[A-Z](\w*)");
            MatchCollection matches = regex.Matches(pirate.Name);

            if (matches.Count == 0)
            {
                _Logger.LogWarning($"Incorrect pirate name {pirate.Name}");
                return BadRequest();
            }

            Pirate? pirateForChanging = _Context.PiratesDB.FirstOrDefault(x => x.Id == id);

            if (pirateForChanging != null)
            {
                _Logger.LogInformation($"Changing pirate with id {id}");
                pirateForChanging.Name = pirate.Name;
                pirateForChanging.Age = pirate.Age;
                pirateForChanging.Description = pirate.Description;
                _Context.PiratesDB.Update(pirateForChanging);
                _Context.SaveChanges();
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
