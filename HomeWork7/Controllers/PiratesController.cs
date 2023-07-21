using Microsoft.AspNetCore.Mvc;

namespace HomeWork7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiratesController : ControllerBase
    {
        private readonly ApplicationContext Context;
        private readonly ILogger<PiratesController> Logger;

        public PiratesController(ApplicationContext context, ILogger<PiratesController> logger)
        {
            Context = context;
            Logger = logger;
        }

        [HttpGet]
        public ActionResult<List<Pirate>> GetCrew()
        {
            if (Context.PiratesDB.Count() > 0)
            {
                Logger.LogInformation("Getting all crew.");
                return Context.PiratesDB.ToList();
            }
            else
            {
                Logger.LogWarning("The crew is empty.");
                return NotFound();
            }
            
        }

        [HttpGet("byId/{id}")]
        public ActionResult<Pirate?> GetPirate([FromRoute] int id)
        {
            Pirate? pirate = Context.PiratesDB.FirstOrDefault(x => x.Id == id);

            if (pirate != null)
            {
                Logger.LogInformation($"Getting pirate by id {id}.");
                return pirate;
            }
            else
            {
                Logger.LogWarning($"No pirate with id {id}");
                return NotFound();
            }

        }

        [HttpGet("byName/{name}")]
        public ActionResult<Pirate?> GetPirateByName([FromRoute] string name)
        {
            Pirate? pirate = Context.PiratesDB.FirstOrDefault(x => x.Name == name);

            if (pirate != null)
            {
                Logger.LogInformation($"Getting pirate by name {name}.");
                return pirate;
            }
            else
            {
                Logger.LogWarning($"No pirate with name {name}");
                return NotFound();
            }

        }

        [HttpPost]
        public ActionResult<Pirate> AddPirate([FromBody] CreatePirateRequest request)
        {
            if (Context.PiratesDB.FirstOrDefault(x => x.Name == request.Name) == null)
            {
                Logger.LogInformation($"Creating pirate with name {request.Name}");
                var pirate = new Pirate
                {
                    Id = Context.PiratesDB.Max(x => x.Id) + 1,
                    Name = request.Name,
                    Description = request.Description,
                    Age = request.Age
                };
                Context.PiratesDB.Add(pirate);
                Context.SaveChanges();
                Logger.LogInformation($"Created pirate with id {pirate.Id} and name {request.Name}");
                return pirate;
            }
            else
            {
                Logger.LogWarning($"A pirate with the name {request.Name} already exists.");
                return BadRequest();
            }
            

        }

        [HttpDelete("{id}")]
        public ActionResult DeletePirate([FromRoute] int id)
        {
            Pirate? pirate = Context.PiratesDB.FirstOrDefault(x => x.Id == id);

            if (pirate != null)
            {
                Logger.LogInformation($"Deleting pirate with id {id}");                
                Context.PiratesDB.Remove(pirate);
                Context.SaveChanges();
                Logger.LogInformation($"Deleted pirate with id {id}");
                return NoContent();
            }
            else
            {
                Logger.LogWarning($"No pirate with id {id}");
                return NotFound();
            }
        }

        //[PirateFilter]
        [HttpPut("{id}")]
        public ActionResult<Pirate> ChangePirate([FromRoute] int id, [FromBody] CreatePirateRequest request)
        {
            Pirate? pirate = Context.PiratesDB.FirstOrDefault(x => x.Id == id);

            if (pirate != null)
            {
                Logger.LogInformation($"Changing pirate with id {id}");                                
                pirate.Name = request.Name;
                pirate.Age = request.Age;
                pirate.Description = request.Description;
                Context.SaveChanges();
                Logger.LogInformation($"Changed pirate with id {id}");
                return pirate;
            }
            else
            {
                Logger.LogWarning($"No pirate with id {id}");
                return NotFound();
            }

        }

        public class Pirate
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public string Description { get; set; }
        }

        public class CreatePirateRequest
        {
            public string Name { get; set; }
            public string Age { get; set; }
            public string Description { get; set; }
        }
    }
}
