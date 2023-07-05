using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static HomeWork7.Controllers.PiratesController;

namespace HomeWork7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiratesController : ControllerBase
    {
        //private readonly ICrew crew;

        //public PiratesController(ICrew crew)
        //{
        //    this.crew = crew;
        //}

        private readonly IPiratesRepository piratesRepository;

        public PiratesController(IPiratesRepository piratesRepository)
        {
            this.piratesRepository = piratesRepository;
        }

        [HttpGet]
        public ActionResult<List<Pirate>> GetCrew()
        {
            using (piratesRepository.Context)
            {
                return piratesRepository.Context.PiratesDB.ToList();
            }
        }

        [HttpGet("byId/{id}")]
        public ActionResult<Pirate?> GetPirate([FromRoute] int id)
        {
            using (piratesRepository.Context)
            {
                if (id <= piratesRepository.Context.PiratesDB.ToList().Count)
                {
                    return piratesRepository.Context.PiratesDB.FirstOrDefault(x => x.Id == id);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("byName/{name}")]
        public ActionResult<Pirate?> GetPirateByName([FromRoute] string name)
        {
            using (piratesRepository.Context)
            {
                if (piratesRepository.Context.PiratesDB.FirstOrDefault(x => x.Name == name) != null)
                {

                    return piratesRepository.GetByName(name);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public ActionResult<Pirate> AddPirate([FromBody] CreatePirateRequest request)
        {
            using (piratesRepository.Context)
            {
                var pirate = new Pirate
                {
                    Id = piratesRepository.Context.PiratesDB.ToList().Count + 1,
                    Name = request.Name,
                    Description = request.Description,
                    Age = request.Age
                };
                piratesRepository.Context.PiratesDB.Add(pirate);
                piratesRepository.Context.SaveChanges();
                return pirate;
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePirate([FromRoute] int id)
        {
            using (piratesRepository.Context)
            {
                var pirate = piratesRepository.Context.PiratesDB.FirstOrDefault(x => x.Id == id);
                if (pirate == null) return NotFound();
                piratesRepository.Context.PiratesDB.Remove(pirate);
                piratesRepository.Context.SaveChanges();
                return Ok();
            }
        }

        //[PirateFilter]
        [HttpPut("{id}")]
        public ActionResult<Pirate> ChangePirate([FromRoute] int id, [FromBody] CreatePirateRequest request)
        {
            using (piratesRepository.Context)
            {
                if (id <= piratesRepository.Context.PiratesDB.ToList().Count)
                {
                    var pirate = piratesRepository.Context.PiratesDB.FirstOrDefault(x => x.Id == id);
                    pirate.Id = id;
                    pirate.Name = request.Name;
                    pirate.Age = request.Age;
                    pirate.Description = request.Description;
                    piratesRepository.Context.SaveChanges();
                    return pirate;
                }
                else
                {
                    return NotFound();
                }
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
