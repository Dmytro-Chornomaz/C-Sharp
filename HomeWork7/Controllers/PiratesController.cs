using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeWork7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiratesController : ControllerBase
    {
        public static List<Pirate> Crew { get; set; } = new List<Pirate> { };

        [HttpGet]
        public List<Pirate> GetCrew()
        {
            return Crew;
        }

        [HttpGet("{id}")]
        public Pirate? GetPirate([FromRoute] int id)
        {
            return Crew.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public Pirate AddPirate([FromBody] CreatePirateRequest request)
        {
            var pirate = new Pirate
            {
                Id = Crew.Count + 1,
                Name = request.Name,
                Description = request.Description,
                Age = request.Age
            };
            Crew.Add(pirate);
            return pirate;
        }

        [HttpDelete("{id}")]
        public bool DeletePirate([FromRoute] int id)
        {
            var pirate = Crew.FirstOrDefault(x => x.Id == id);
            if (pirate == null) return false;            
            Crew.Remove(pirate);
            return true;
        }

        [HttpPut("{id}")]
        public Pirate ChangePirate([FromRoute] int id, [FromBody] CreatePirateRequest request)
        {
            var pirate = Crew.FirstOrDefault(x => x.Id == id);            
            pirate.Id = id;
            pirate.Name = request.Name;
            pirate.Age = request.Age;
            pirate.Description = request.Description;
            return pirate;
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
