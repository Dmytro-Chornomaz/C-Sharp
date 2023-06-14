using Microsoft.AspNetCore.Mvc;

namespace Finance_Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceOrganizerController : ControllerBase
    {        
        private readonly IUsers users;
        public FinanceOrganizerController(IUsers users)
        {
            this.users = users;
        }

        [HttpPost("CreatePerson/{name}")]
        public ActionResult<Person> CreatePerson([FromRoute] string name)
        {
            Person person = new Person
            {
                Id = users.ListOfUsers.Count() + 1,
                Name = name,
                Account = new Account() { Id = users.ListOfUsers.Count() + 1 }
            };
            users.ListOfUsers.Add(person);
            return person;
        }

        [HttpGet("GetAllPersons")]
        public ActionResult<List<Person>> GetUsers()
        {
            return users.ListOfUsers;
        }

        [HttpGet("GetPerson/{name}")]
        public ActionResult<Person?> GetPersonByName([FromRoute] string name)
        {
            if (users.ListOfUsers.FirstOrDefault(x => x.Name == name) != null)
            {
                return users.ListOfUsers.FirstOrDefault(x => x.Name == name);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("DeletePerson/{name}")]
        public ActionResult DeletePerson([FromRoute] string name, string confirmation)
        {
            if (users.ListOfUsers.FirstOrDefault(x => x.Name == name) != null)
            {
                if (confirmation.ToLower() == "yes")
                {
                    var personForDeleting = users.ListOfUsers.FirstOrDefault(x => x.Name == name);
                    users.ListOfUsers.Remove(personForDeleting);
                    return NoContent();
                }
                else
                {
                    return Ok();
                }
            }
            else
            {
                return NotFound();
            }
        }
    }
}