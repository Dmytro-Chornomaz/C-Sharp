using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;

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

        [HttpPost("CreatePerson")]
        public ActionResult<Person> CreatePerson([FromQuery] string name)
        {
            if (name != null)
            {
                int id = users.ListOfUsers.Count() + 1;

                Person person = new Person
                {
                    Id = id,
                    Name = name,
                    Account = new Account() { Id = id }
                };
                users.ListOfUsers.Add(person);
                return person;
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetAllPersons")]
        public ActionResult<List<Person>> GetAllPersons()
        {
            return users.ListOfUsers;
        }

        [HttpGet("GetPerson")]
        public ActionResult<Person> GetPerson([FromQuery] string name)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
            {
                return person;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("DeletePerson")]
        public ActionResult DeletePerson([FromQuery] string name, string confirmation)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
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

        [HttpPost("AddTransaction")]
        public ActionResult<Transaction> AddTransaction([FromQuery] string name, [FromBody] Transaction transaction)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
            {
                transaction.AccountId = person.Account.Id;
                transaction.Categories.AccountId = transaction.AccountId;
                transaction.Id = person.Account.Transactions.Count + 1;
                transaction.Categories.Id = transaction.Id;
                person.Account.Transactions.Add(transaction);
                return transaction;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetLastTransaction")]
        public ActionResult<Transaction> GetLastTransaction([FromQuery] string name)
        {
            Person person = users.GetPersonByName(name);
            bool verification = person.Account.Transactions.Count > 0;

            if (person != null && verification)
            {
                var lastTransaction = person.Account.Transactions.Last();
                return lastTransaction;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("DeleteLastTransaction")]
        public ActionResult DeleteLastTransaction([FromQuery] string name, string confirmation)
        {
            Person person = users.GetPersonByName(name);
            bool verification = person.Account.Transactions.Count > 0;

            if (person != null && verification)
            {
                if (confirmation.ToLower() == "yes")
                {
                    var lastTransaction = person.Account.Transactions.Last();
                    person.Account.Transactions.Remove(lastTransaction);
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

        [HttpGet("GetAllTransactionsByPerson")]
        public ActionResult<List<Transaction>> GetAllTransactionsByPerson([FromQuery] string name)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
            {
                return person.Account.Transactions;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForThisMonth")]
        public ActionResult<Categories> GetExpensesForThisMonth([FromQuery] string name)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
            {
                DateTime dateTime = DateTime.Now;
                var transactions = person.Account.Transactions.Where(x => x.Time.Month == dateTime.Month);
                var categories = transactions.Select(x => x.Categories);
                if (categories != null)
                {
                    var result = Categories.CategoriesSum(categories);
                    return result;
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForThisYear")]
        public ActionResult<Categories> GetExpensesForThisYear([FromQuery] string name)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
            {
                DateTime dateTime = DateTime.Now;
                var transactions = person.Account.Transactions.Where(x => x.Time.Year == dateTime.Year);
                var categories = transactions.Select(x => x.Categories);
                if (categories != null)
                {
                    var result = Categories.CategoriesSum(categories);
                    return result;
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForSpecificMonth")]
        public ActionResult<Categories> GetExpensesForSpecificMonth([FromQuery] string name, int month, int year)
        {
            Person person = users.GetPersonByName(name);

            DateTime today = DateTime.Now;
            bool checkMonth = 12 >= month && month > 0;
            bool checkYear = today.Year >= year && year > 2021;

            if (checkMonth && checkYear)
            {
                if (person != null)
                {
                    DateTime dateTime = new DateTime(year, month, 1);
                    var transactions = person.Account.Transactions
                        .Where(x => x.Time.Month == dateTime.Month && x.Time.Year == dateTime.Year);
                    var categories = transactions.Select(x => x.Categories);

                    if (categories != null)
                    {
                        var result = Categories.CategoriesSum(categories);
                        return result;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetExpensesForSpecificYear")]
        public ActionResult<Categories> GetExpensesForSpecificYear([FromQuery] string name, int year)
        {
            Person person = users.GetPersonByName(name);

            DateTime today = DateTime.Now;
            bool checkYear = today.Year >= year && year > 2021;
            if (checkYear)
            {
                if (person != null)
                {
                    DateTime dateTime = new DateTime(year, 1, 1);
                    var transactions = person.Account.Transactions
                        .Where(x => x.Time.Year == dateTime.Year);
                    var categories = transactions.Select(x => x.Categories);

                    if (categories != null)
                    {
                        var result = Categories.CategoriesSum(categories);
                        return result;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetExpensesForLastWeek")]
        public ActionResult<Categories> GetExpensesForLastWeek([FromQuery] string name)
        {
            Person person = users.GetPersonByName(name);

            if (person != null)
            {
                DateTime dateTime = DateTime.Now;
                DateTime weekAgo = dateTime.AddDays(-7);
                var transactions = person.Account.Transactions.Where(x => x.Time >= weekAgo);
                var categories = transactions.Select(x => x.Categories);
                if (categories != null)
                {
                    var result = Categories.CategoriesSum(categories);
                    return result;
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForSpecificPeriod")]
        public ActionResult<Categories> GetExpensesForSpecificPeriod
            ([FromQuery] string name, int dayStart, int monthStart, int yearStart, int dayEnd, int monthEnd, int yearEnd)
        {
            Person person = users.GetPersonByName(name);

            DateTime today = DateTime.Now;

            int daysInMonthStart = DateTime.DaysInMonth(yearStart, monthStart);
            bool checkDayStart = daysInMonthStart >= dayStart && dayStart > 0;
            bool checkMonthStart = 12 >= monthStart && monthStart > 0;
            bool checkYearStart = today.Year >= yearStart && yearStart > 2021;

            int daysInMonthEnd = DateTime.DaysInMonth(yearEnd, monthEnd);
            bool checkDayEnd = daysInMonthEnd >= dayEnd && dayEnd > 0;
            bool checkMonthEnd = 12 >= monthEnd && monthEnd > 0;
            bool checkYearEnd = today.Year >= yearEnd && yearEnd > 2021;

            bool yearsComparison = yearStart <= yearEnd;

            if (checkDayStart && checkMonthStart && checkYearStart && checkDayEnd && checkMonthEnd && checkYearEnd && yearsComparison)
            {
                if (person != null)
                {
                    DateTime dateStart = new DateTime(yearStart, monthStart, dayStart);
                    DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);

                    var transactions = person.Account.Transactions
                        .Where(x => x.Time >= dateStart && x.Time <= dateEnd);
                    var categories = transactions.Select(x => x.Categories);

                    if (categories != null)
                    {
                        var result = Categories.CategoriesSum(categories);
                        return result;
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}