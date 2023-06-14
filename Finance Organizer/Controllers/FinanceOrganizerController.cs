using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

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
        public ActionResult<List<Person>> GetAllPersons()
        {
            return users.ListOfUsers;
        }

        [HttpGet("GetPerson/{name}")]
        public ActionResult<Person?> GetPerson([FromRoute] string name)
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

        [HttpPost("AddTransaction/{name}")]
        public ActionResult<Transaction> AddTransaction([FromRoute] string name, [FromBody] Transaction transaction)
        {
            if (users.ListOfUsers.FirstOrDefault(x => x.Name == name) != null)
            {
                Person person = users.ListOfUsers.FirstOrDefault(x => x.Name == name);                
                transaction.Id = person.Account.Transactions.Count + 1;                
                person.Account.Transactions.Add(transaction);
                return transaction;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("AllTransactionsByPerson/{name}")]
        public ActionResult<List<Transaction>> GetAllTransactionsByPerson([FromRoute] string name)
        {
            if (users.ListOfUsers.FirstOrDefault(x => x.Name == name) != null)
            {
                Person person = users.ListOfUsers.FirstOrDefault(x => x.Name == name);
                return person.Account.Transactions;
            }
            else
            {
                return NotFound();
            }
        }
    }
}