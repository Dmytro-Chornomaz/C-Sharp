using Finance_Organizer.Business;
using Finance_Organizer.Database;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Finance_Organizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinanceOrganizerController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        private readonly ILogger<FinanceOrganizerController> _Logger;
        private readonly IValidator<Person> _PersonValidator;
        private readonly IValidator<Transaction> _TransactionValidator;
        private readonly IValidator<Categories> _CategoriesValidator;

        public FinanceOrganizerController(ApplicationDbContext context, ILogger<FinanceOrganizerController> logger, 
            IValidator<Transaction> transactionValidator, IValidator<Categories> categoriesValidator, 
            IValidator<Person> personValidator)
        {
            _Context = context;
            _Logger = logger;
            _TransactionValidator = transactionValidator;
            _CategoriesValidator = categoriesValidator;
            _PersonValidator = personValidator;
        }

        // The function that creates a new user.
        [HttpPost("CreatePerson")]
        [Authorize]
        public ActionResult<Person> CreatePerson([FromQuery] string name)
        {
            if (name != null)
            {
                if (_Context.Users.Any(x => x.Name == name))
                {
                    _Logger.LogWarning($"*** This name user exists. ***");
                    return BadRequest();
                }
                else
                {
                    int id;

                    if (_Context.Users.Count() == 0)
                    {
                        id = 1;
                    }
                    else
                    {
                        id = _Context.Users.Max(x => x.Id) + 1;
                    }

                    Person person = new Person
                    {
                        Id = id,
                        Name = name
                    };

                    ValidationResult result = _PersonValidator.Validate(person);

                    if (!result.IsValid)
                    {
                        foreach (var error in result.Errors)
                        {
                            _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                        }

                        return BadRequest();
                    }

                    _Context.Users.Add(person);
                    _Context.SaveChanges();
                    _Logger.LogInformation($"*** Creation of a user with the name {name} ***");
                    return person;
                }

            }
            else
            {
                _Logger.LogWarning($"*** Did not enter a user name. ***");
                return BadRequest();
            }
        }

        // The function that returns a list of all existing users. It is used for development and testing purposes.
        [HttpGet("GetAllPersons")]
        [Authorize]
        public ActionResult<List<Person>> GetAllPersons()
        {
            if (_Context.Users.Count() != 0)
            {
                _Logger.LogInformation($"*** Creation of users list. ***");
                return _Context.Users.ToList();
            }
            else
            {
                _Logger.LogWarning($"*** No users in a list. ***");
                return NotFound();
            }

        }

        // The function that returns the specific user. It is used for development and testing purposes.
        [HttpGet("GetPerson")]
        [Authorize]
        public ActionResult<Person?> GetPerson([FromQuery] string name)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                _Logger.LogInformation($"*** Getting a user by the name of {name}. ***");
                return person;
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that deletes the specific user. It uses the confirmation word "yes".
        [HttpDelete("DeletePerson")]
        [Authorize]
        public ActionResult DeletePerson([FromQuery] string name, string confirmation)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (confirmation.ToLower() == "yes")
                {
                    _Context.Users.Remove(person);

                    var transactionsForDeleting = person.Transactions.ToList();

                    foreach (var transaction in transactionsForDeleting)
                    {
                        _Context.Transactions.Remove(transaction);
                    }

                    var categoriesForDeleting = _Context.Categories
                                                   .Where(x => x.PersonId == person.Id).ToList();

                    foreach (var cat in categoriesForDeleting)
                    {
                        _Context.Categories.Remove(cat);
                    }

                    _Context.SaveChanges();
                    _Logger.LogInformation($"*** Deleting a user by the name of {name}. ***");
                    return NoContent();
                }
                else
                {
                    _Logger.LogInformation($"*** Entered the wrong confirmation word. ***");
                    return Ok();
                }
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        /* The function that adds transaction for the specific user from the request body. 
         * It is used for development and testing purposes.*/
        [HttpPost("AddTransactionFromBody")]
        [Authorize]
        public ActionResult<Transaction> AddTransactionFromBody([FromQuery] string name, [FromBody] Transaction transaction)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                transaction.PersonId = person.Id;
                transaction.Categories.PersonId = transaction.PersonId;

                ValidationResult transactionResult = _TransactionValidator.Validate(transaction);

                if (!transactionResult.IsValid)
                {
                    foreach (var error in transactionResult.Errors)
                    {
                        _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                    }

                    return BadRequest();
                }

                ValidationResult categoriesResult = _CategoriesValidator.Validate(transaction.Categories);

                if (!categoriesResult.IsValid)
                {
                    foreach (var error in categoriesResult.Errors)
                    {
                        _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                    }

                    return BadRequest();
                }

                person.Transactions.Add(transaction);
                _Context.SaveChanges();
                _Logger.LogInformation($"*** Addition a new transaction for a user by the name of {name}. ***");
                return transaction;
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that adds transaction for the specific user.
        [HttpPost("AddTransaction")]
        [Authorize]
        public ActionResult<Transaction> AddTransaction
            ([FromQuery] string name, double meal, double communalServices, double medicine,
            double transport, double purchases, double leisure, double savings)
        {
            Person? person = _Context.GetPersonByName(name);
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

                ValidationResult transactionResult = _TransactionValidator.Validate(transaction);

                if (!transactionResult.IsValid)
                {
                    foreach (var error in transactionResult.Errors)
                    {
                        _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                    }

                    return BadRequest();
                }

                ValidationResult categoriesResult = _CategoriesValidator.Validate(transaction.Categories);

                if (!categoriesResult.IsValid)
                {
                    foreach (var error in categoriesResult.Errors)
                    {
                        _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                    }

                    return BadRequest();
                }

                person.Transactions.Add(transaction);
                _Context.SaveChanges();
                _Logger.LogInformation($"*** Addition a new transaction for a user by the name of {name}. ***");
                return transaction;
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that returns the last realized transaction for the specific user.
        [HttpGet("GetLastTransaction")]
        [Authorize]
        public ActionResult<Transaction> GetLastTransaction([FromQuery] string name)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    var lastTransaction = person.Transactions.LastOrDefault();
                    _Logger.LogInformation($"*** Getting a last transaction for a user by the name of {name}. ***");
                    return lastTransaction!;
                }
                else
                {
                    _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that deletes the last realized transaction for the specific user.
        [HttpDelete("DeleteLastTransaction")]
        [Authorize]
        public ActionResult DeleteLastTransaction([FromQuery] string name, string confirmation)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (confirmation.ToLower() == "yes")
                {
                    if (person.Transactions.Count() != 0)
                    {
                        var lastTransaction = person.Transactions.LastOrDefault();
                        _Context.Transactions.Remove(lastTransaction!);
                        var categoriesFromLastTransaction = lastTransaction!.Categories;
                        _Context.Categories.Remove(categoriesFromLastTransaction);
                        _Context.SaveChanges();
                        _Logger.LogInformation($"*** Deleting a last transaction for a user by the name of {name}. ***");
                        return NoContent();
                    }
                    else
                    {
                        _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    _Logger.LogInformation($"*** Entered the wrong confirmation word. ***");
                    return Ok();
                }

            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that returns all realized transactions for the specific user.
        [HttpGet("GetAllTransactionsByPerson")]
        [Authorize]
        public ActionResult<List<Transaction>> GetAllTransactionsByPerson([FromQuery] string name)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    _Logger.LogInformation($"*** Getting all transactions for a user by the name of {name}. ***");
                    return person.Transactions;
                }
                else
                {
                    _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }

            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that returns the specific user expenses for the current month.
        [HttpGet("GetExpensesForThisMonth")]
        [Authorize]
        public ActionResult<Categories> GetExpensesForThisMonth([FromQuery] string name, bool giveInPercents)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    DateTime dateTime = DateTime.Now;
                    var transactions = person.Transactions.Where(x => x.Time.Month == dateTime.Month);
                    var categories = transactions.Select(x => x.Categories);
                    var result = Categories.CategoriesSum(categories);
                    if (giveInPercents)
                    {
                        result = Categories.CategoriesSumInPercents(result);
                    }
                    _Logger.LogInformation($"*** Getting expenses for this month for a user by the name of {name}. ***");
                    return result;
                }
                else
                {
                    _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that returns the specific user expenses for the current year.
        [HttpGet("GetExpensesForThisYear")]
        [Authorize]
        public ActionResult<Categories> GetExpensesForThisYear([FromQuery] string name, bool giveInPercents)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    DateTime dateTime = DateTime.Now;
                    var transactions = person.Transactions.Where(x => x.Time.Year == dateTime.Year);
                    var categories = transactions.Select(x => x.Categories);
                    var result = Categories.CategoriesSum(categories);
                    if (giveInPercents)
                    {
                        result = Categories.CategoriesSumInPercents(result);
                    }
                    _Logger.LogInformation($"*** Getting expenses for this year for a user by the name of {name}. ***");
                    return result;
                }
                else
                {
                    _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that returns the specific user expenses for the specific month.
        [HttpGet("GetExpensesForSpecificMonth")]
        [Authorize]
        public ActionResult<Categories> GetExpensesForSpecificMonth([FromQuery] string name, int month, int year,
            bool giveInPercents)
        {
            Person? person = _Context.GetPersonByName(name);

            DateTime today = DateTime.Now;
            bool checkMonth = 12 >= month && month > 0;
            bool checkYear = today.Year >= year && year > 2021;

            if (checkMonth && checkYear)
            {
                if (person != null)
                {
                    if (person.Transactions.Count() != 0)
                    {
                        DateTime dateTime = new DateTime(year, month, 1);
                        var transactions = person.Transactions
                            .Where(x => x.Time.Month == dateTime.Month && x.Time.Year == dateTime.Year);
                        var categories = transactions.Select(x => x.Categories);
                        var result = Categories.CategoriesSum(categories);
                        if (giveInPercents)
                        {
                            result = Categories.CategoriesSumInPercents(result);
                        }
                        _Logger.LogInformation($"*** Getting expenses for {month}/{year} date for a user by the name of {name}. ***");
                        return result;

                    }
                    else
                    {
                        _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** Wrong date. ***");
                return BadRequest();
            }
        }

        // The function that returns the specific user expenses for the specific year.
        [HttpGet("GetExpensesForSpecificYear")]
        [Authorize]
        public ActionResult<Categories> GetExpensesForSpecificYear([FromQuery] string name, int year, bool giveInPercents)
        {
            Person? person = _Context.GetPersonByName(name);

            DateTime today = DateTime.Now;
            bool checkYear = today.Year >= year && year > 2021;
            if (checkYear)
            {
                if (person != null)
                {
                    if (person.Transactions.Count() != 0)
                    {
                        DateTime dateTime = new DateTime(year, 1, 1);
                        var transactions = person.Transactions
                            .Where(x => x.Time.Year == dateTime.Year);
                        var categories = transactions.Select(x => x.Categories);
                        var result = Categories.CategoriesSum(categories);
                        if (giveInPercents)
                        {
                            result = Categories.CategoriesSumInPercents(result);
                        }
                        _Logger.LogInformation($"*** Getting expenses for {year} year for a user by the name of {name}. ***");

                        return result;
                    }
                    else
                    {
                        _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** Wrong date. ***");
                return BadRequest();
            }
        }

        // The function that returns the specific user expenses for the last 7 days.
        [HttpGet("GetExpensesForLastWeek")]
        [Authorize]
        public ActionResult<Categories> GetExpensesForLastWeek([FromQuery] string name, bool giveInPercents)
        {
            Person? person = _Context.GetPersonByName(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    DateTime dateTime = DateTime.Now;
                    DateTime weekAgo = dateTime.AddDays(-7);
                    var transactions = person.Transactions.Where(x => x.Time >= weekAgo);
                    var categories = transactions.Select(x => x.Categories);
                    var result = Categories.CategoriesSum(categories);
                    if (giveInPercents)
                    {
                        result = Categories.CategoriesSumInPercents(result);
                    }
                    _Logger.LogInformation($"*** Getting expenses for last week for a user by the name of {name}. ***");
                    return result;
                }
                else
                {
                    _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return NotFound();
            }
        }

        // The function that returns the specific user expenses for a specific period.
        [HttpGet("GetExpensesForSpecificPeriod")]
        [Authorize]
        public ActionResult<Categories> GetExpensesForSpecificPeriod
            ([FromQuery] string name, int dayStart, int monthStart, int yearStart, 
            int dayEnd, int monthEnd, int yearEnd, bool giveInPercents)
        {
            Person? person = _Context.GetPersonByName(name);

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
                    if (person.Transactions.Count() != 0)
                    {
                        DateTime dateStart = new DateTime(yearStart, monthStart, dayStart);
                        DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);

                        var transactions = person.Transactions
                            .Where(x => x.Time >= dateStart && x.Time <= dateEnd);
                        var categories = transactions.Select(x => x.Categories);
                        var result = Categories.CategoriesSum(categories);
                        if (giveInPercents)
                        {
                            result = Categories.CategoriesSumInPercents(result);
                        }
                        _Logger.LogInformation($"*** Getting expenses for specific period for a user by the name of {name}. ***");
                        return result;
                    }
                    else
                    {
                        _Logger.LogInformation($"*** The user by the name of {name} has not transactions yet. ***");
                        return NotFound();
                    }
                }
                else
                {
                    _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                    return NotFound();
                }
            }
            else
            {
                _Logger.LogWarning($"*** Wrong date. ***");
                return BadRequest();
            }
        }
    }
}