﻿using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static HomeWork7.Controllers.PiratesController;

namespace HomeWork7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PiratesController : ControllerBase
    {
        //public static List<Pirate> Crew { get; set; } = new List<Pirate>
        //{
        //    new(){Id = 1, Name = "Jack the Sparrow", Age = "40", Description = "Eccentric with a couple of pistols"},
        //    new(){Id = 2, Name = "Billy Bounce", Age = "Old stump", Description = "Pale alcoholic"}
        //};

        [HttpGet]
        public ActionResult<List<Pirate>> GetCrew()
        {
            return Crew.Pirates;
        }

        [HttpGet("{id}")]
        public ActionResult<Pirate?> GetPirate([FromRoute] int id)
        {
            if (id <= Crew.Pirates.Count)
            {
                return Crew.Pirates.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{name}")]
        public ActionResult<Pirate?> GetPirateByName([FromRoute] string name)
        {
            if (Crew.Pirates.FirstOrDefault(x => x.Name == name) != null)
            {
                Crew crew = new();
                return crew.GetByName(name);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public ActionResult<Pirate> AddPirate([FromBody] CreatePirateRequest request)
        {
            var pirate = new Pirate
            {
                Id = Crew.Pirates.Count + 1,
                Name = request.Name,
                Description = request.Description,
                Age = request.Age
            };
            Crew.Pirates.Add(pirate);
            return pirate;
        }

        [HttpDelete("{id}")]
        public ActionResult DeletePirate([FromRoute] int id)
        {
            var pirate = Crew.Pirates.FirstOrDefault(x => x.Id == id);
            if (pirate == null) return NotFound();
            Crew.Pirates.Remove(pirate);
            return Ok();
        }

        [HttpPut("{id}")]
        public ActionResult<Pirate> ChangePirate([FromRoute] int id, [FromBody] CreatePirateRequest request)
        {
            if (id <= Crew.Pirates.Count)
            {
                var pirate = Crew.Pirates.FirstOrDefault(x => x.Id == id);
                pirate.Id = id;
                pirate.Name = request.Name;
                pirate.Age = request.Age;
                pirate.Description = request.Description;
                return pirate;
            }
            else
            {
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
