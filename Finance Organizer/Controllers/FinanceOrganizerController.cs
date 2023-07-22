using Microsoft.AspNetCore.Mvc;
using System;

namespace Finance_Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceOrganizerController : ControllerBase
    {
        private readonly ApplicationDbContext Context;
        private readonly ILogger<FinanceOrganizerController> Logger;
        public FinanceOrganizerController(ApplicationDbContext context, ILogger<FinanceOrganizerController> logger)
        {
            Context = context;
            Logger = logger;
        }

        [HttpPost("CreatePerson")]
        public ActionResult<Person> CreatePerson([FromQuery] string name)
        {
            if (name != null)
            {
                if (Context.Users.Any(x => x.Name == name))
                {
                    Logger.LogWarning($"*** This name user exists. ***");
                    return BadRequest();
                }
                else
                {
                    int id;

                    if (Context.Users.Count() == 0)
                    {
                        id = 1;
                    }
                    else
                    {
                        id = Context.Users.Max(x => x.Id) + 1;
                    }

                    Person person = new Person
                    {
                        Id = id,
                        Name = name
                    };
                    Context.Users.Add(person);
                    Context.SaveChanges();
                    Logger.LogInformation($"*** Creation of a user with the name {name} ***");
                    return person;
                }

            }
            else
            {
                Logger.LogWarning($"*** Did not enter a user name. ***");
                return BadRequest();
            }
        }

        [HttpGet("GetAllPersons")]
        public ActionResult<List<Person>> GetAllPersons()
        {
            if (Context.Users.Count() > 0)
            {
                Logger.LogInformation($"*** Creation of users list. ***");
                return Context.Users.ToList();
            }
            else
            {
                Logger.LogWarning($"*** No users in a list. ***");
                return NotFound();
            }

        }

        [HttpGet("GetPerson")]
        public ActionResult<Person?> GetPerson([FromQuery] string name)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                Logger.LogInformation($"*** Getting a user by the name of {name}. ***");
                return person;
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpDelete("DeletePerson")]
        public ActionResult DeletePerson([FromQuery] string name, string confirmation)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (confirmation.ToLower() == "yes")
                {
                    Context.Users.Remove(person);

                    var transactionsForDeleting = person.Transactions.ToList();

                    foreach (var transaction in transactionsForDeleting)
                    {
                        Context.Transactions.Remove(transaction);
                    }

                    var categoriesForDeleting = Context.Categories
                                                   .Where(x => x.PersonId == person.Id).ToList();

                    foreach (var cat in categoriesForDeleting)
                    {
                        Context.Categories.Remove(cat);
                    }

                    Context.SaveChanges();
                    Logger.LogInformation($"*** Deleting a user by the name of {name}. ***");
                    return NoContent();
                }
                else
                {
                    Logger.LogInformation($"*** Entered the wrong confirmation word. ***");
                    return Ok();
                }
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpPost("AddTransactionFromBody")]
        public ActionResult<Transaction> AddTransactionFromBody([FromQuery] string name, [FromBody] Transaction transaction)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                transaction.PersonId = person.Id;
                transaction.Categories.PersonId = transaction.PersonId;

                person.Transactions.Add(transaction);
                Context.SaveChanges();
                Logger.LogInformation($"*** Addition a new transaction for a user by the name of {name}. ***");
                return transaction;
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpPost("AddTransaction")]
        public ActionResult<Transaction> AddTransaction
            ([FromQuery] string name, double meal, double communalServices, double medicine,
            double transport, double purchases, double leisure, double savings)
        {
            Person? person = Context.GetPersonByName(name);
            Transaction transaction = new Transaction();

            if (person != null)
            {
                transaction.PersonId = person.Id;
                transaction.Categories.PersonId = transaction.PersonId;

                transaction.Categories.Meal = meal;
                transaction.Categories.CommunalServices = communalServices;
                transaction.Categories.Medicine = medicine;
                transaction.Categories.Transport = transport;
                transaction.Categories.Purchases = purchases;
                transaction.Categories.Leisure = leisure;
                transaction.Categories.Savings = savings;

                person.Transactions.Add(transaction);
                Context.SaveChanges();
                Logger.LogInformation($"*** Addition a new transaction for a user by the name of {name}. ***");
                return transaction;
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpGet("GetLastTransaction")]
        public ActionResult<Transaction> GetLastTransaction([FromQuery] string name)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() > 0)
                {
                    var lastTransaction = person.Transactions.LastOrDefault();
                    Logger.LogInformation($"*** Getting a last transaction for a user by the name of {name}. ***");
                    return lastTransaction!;
                }
                else
                {
                    Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpDelete("DeleteLastTransaction")]
        public ActionResult DeleteLastTransaction([FromQuery] string name, string confirmation)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (confirmation.ToLower() == "yes")
                {
                    if (person.Transactions.Count() > 0)
                    {
                        var lastTransaction = person.Transactions.LastOrDefault();
                        person.Transactions.Remove(lastTransaction!);
                        var categoriesFromLastTransaction = lastTransaction!.Categories;
                        Context.Categories.Remove(categoriesFromLastTransaction);
                        Context.SaveChanges();
                        Logger.LogInformation($"*** Deleting a last transaction for a user by the name of {name}. ***");
                        return NoContent();
                    }
                    else
                    {
                        Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    Logger.LogInformation($"*** Entered the wrong confirmation word. ***");
                    return Ok();
                }

            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpGet("GetAllTransactionsByPerson")]
        public ActionResult<List<Transaction>> GetAllTransactionsByPerson([FromQuery] string name)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() > 0)
                {
                    Logger.LogInformation($"*** Getting all transactions for a user by the name of {name}. ***");
                    return person.Transactions;
                }
                else
                {
                    Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }

            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForThisMonth")]
        public ActionResult<Categories> GetExpensesForThisMonth([FromQuery] string name)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() > 0)
                {
                    DateTime dateTime = DateTime.Now;
                    var transactions = person.Transactions.Where(x => x.Time.Month == dateTime.Month);
                    var categories = transactions.Select(x => x.Categories);
                    var result = Categories.CategoriesSum(categories);
                    Logger.LogInformation($"*** Getting expenses for this month for a user by the name of {name}. ***");
                    return result;
                }
                else
                {
                    Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForThisYear")]
        public ActionResult<Categories> GetExpensesForThisYear([FromQuery] string name)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() > 0)
                {
                    DateTime dateTime = DateTime.Now;
                    var transactions = person.Transactions.Where(x => x.Time.Year == dateTime.Year);
                    var categories = transactions.Select(x => x.Categories);
                    var result = Categories.CategoriesSum(categories);
                    Logger.LogInformation($"*** Getting expenses for this year for a user by the name of {name}. ***");
                    return result;
                }
                else
                {
                    Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForSpecificMonth")]
        public ActionResult<Categories> GetExpensesForSpecificMonth([FromQuery] string name, int month, int year)
        {
            Person? person = Context.GetPersonByName(name);

            DateTime today = DateTime.Now;
            bool checkMonth = 12 >= month && month > 0;
            bool checkYear = today.Year >= year && year > 2021;

            if (checkMonth && checkYear)
            {
                if (person != null)
                {
                    if (person.Transactions.Count() > 0)
                    {
                        DateTime dateTime = new DateTime(year, month, 1);
                        var transactions = person.Transactions
                            .Where(x => x.Time.Month == dateTime.Month && x.Time.Year == dateTime.Year);
                        var categories = transactions.Select(x => x.Categories);
                        var result = Categories.CategoriesSum(categories);
                        Logger.LogInformation($"*** Getting expenses for {month}/{year} date for a user by the name of {name}. ***");
                        return result;

                    }
                    else
                    {
                        Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** Wrong date. ***");
                return BadRequest();
            }
        }

        [HttpGet("GetExpensesForSpecificYear")]
        public ActionResult<Categories> GetExpensesForSpecificYear([FromQuery] string name, int year)
        {
            Person? person = Context.GetPersonByName(name);

            DateTime today = DateTime.Now;
            bool checkYear = today.Year >= year && year > 2021;
            if (checkYear)
            {
                if (person != null)
                {
                    if (person.Transactions.Count() > 0)
                    {
                        DateTime dateTime = new DateTime(year, 1, 1);
                        var transactions = person.Transactions
                            .Where(x => x.Time.Year == dateTime.Year);
                        var categories = transactions.Select(x => x.Categories);
                        var result = Categories.CategoriesSum(categories);
                        Logger.LogInformation($"*** Getting expenses for {year} year for a user by the name of {name}. ***");

                        return result;
                    }
                    else
                    {
                        Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** Wrong date. ***");
                return BadRequest();
            }
        }

        [HttpGet("GetExpensesForLastWeek")]
        public ActionResult<Categories> GetExpensesForLastWeek([FromQuery] string name)
        {
            Person? person = Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() > 0)
                {
                    DateTime dateTime = DateTime.Now;
                    DateTime weekAgo = dateTime.AddDays(-7);
                    var transactions = person.Transactions.Where(x => x.Time >= weekAgo);
                    var categories = transactions.Select(x => x.Categories);
                    var result = Categories.CategoriesSum(categories);
                    Logger.LogInformation($"*** Getting expenses for last week for a user by the name of {name}. ***");
                    return result;
                }
                else
                {
                    Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        [HttpGet("GetExpensesForSpecificPeriod")]
        public ActionResult<Categories> GetExpensesForSpecificPeriod
            ([FromQuery] string name, int dayStart, int monthStart, int yearStart, int dayEnd, int monthEnd, int yearEnd)
        {
            Person? person = Context.GetPersonByName(name);

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
                    if (person.Transactions.Count() > 0)
                    {
                        DateTime dateStart = new DateTime(yearStart, monthStart, dayStart);
                        DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);

                        var transactions = person.Transactions
                            .Where(x => x.Time >= dateStart && x.Time <= dateEnd);
                        var categories = transactions.Select(x => x.Categories);
                        var result = Categories.CategoriesSum(categories);
                        Logger.LogInformation($"*** Getting expenses for specific period for a user by the name of {name}. ***");
                        return result;
                    }
                    else
                    {
                        Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                    return NotFound();
                }
            }
            else
            {
                Logger.LogWarning($"*** Wrong date. ***");
                return BadRequest();
            }
        }
    }
}