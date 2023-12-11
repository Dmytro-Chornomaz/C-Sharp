using Finance_Organizer.Model;
using Finance_Organizer.Database;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Finance_Organizer.Validators;

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
        private readonly DateValidator _dateValidator;
        private readonly IValidator<LoginModel> _LoginModelValidator;
        private readonly SaltedHash _SaltedHash;

        public FinanceOrganizerController(ApplicationDbContext context, ILogger<FinanceOrganizerController> logger,
            IValidator<Transaction> transactionValidator, IValidator<Categories> categoriesValidator,
            IValidator<Person> personValidator, DateValidator dateValidator, IValidator<LoginModel> loginModelValidator,
            SaltedHash saltedHash)
        {
            _Context = context;
            _Logger = logger;
            _TransactionValidator = transactionValidator;
            _CategoriesValidator = categoriesValidator;
            _PersonValidator = personValidator;
            _dateValidator = dateValidator;
            _LoginModelValidator = loginModelValidator;
            _SaltedHash = saltedHash;
        }

        // The function that creates a new user.
        [HttpPost("CreatePerson")]
        public async Task<ActionResult> CreatePersonAsync([FromBody] LoginModel request)
        {
            _Logger.LogInformation("*** Method CreatePersonAsync started. ***");

            ValidationResult loginModelResult = await _LoginModelValidator.ValidateAsync(request);

            if (!loginModelResult.IsValid)
            {
                foreach (var error in loginModelResult.Errors)
                {
                    _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                }

                return BadRequest("Incorrect login or/and password!");
            }

            if (_Context.Users.Any(x => x.Name == request.Login))
            {
                _Logger.LogWarning($"*** This name user exists. ***");
                return BadRequest("This name user exists!");
            }
            else
            {                
                string password = _SaltedHash.ComputeSaltedHash(request.Password);

                Person person = new Person
                {
                    Name = request.Login,
                    Password = password
                };
                
                await _Context.Users.AddAsync(person);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation($"*** Creation of a user with the name {request.Login} ***");
                return Ok();
            }
        }

        // The function that returns a list of all existing users. It is used for development and testing purposes.
        [HttpGet("GetAllPersons")]
        [Authorize]
        public async Task<ActionResult<List<Person>>> GetAllPersonsAsync()
        {
            if (_Context.Users.Count() != 0)
            {
                _Logger.LogInformation($"*** Creation of users list. ***");
                return await _Context.Users.ToListAsync();
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
        public async Task<ActionResult<Person?>> GetPersonAsync([FromQuery] string name)
        {
            Person? person = await _Context.GetPersonByNameAsync(name);

            if (person != null)
            {
                _Logger.LogInformation($"*** Getting a user by the name of {name}. ***");
                return person;
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return BadRequest();
            }
        }

        // The function that deletes the specific user. It uses the confirmation word "yes".
        [HttpDelete("DeletePerson")]
        [Authorize]
        public async Task<ActionResult> DeletePersonAsync([FromQuery] string name, string confirmation)
        {
            Person? person = await _Context.GetPersonByNameAsync(name);

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

                    var categoriesForDeleting = await _Context.Categories
                                                   .Where(x => x.PersonId == person.Id).ToListAsync();

                    foreach (var cat in categoriesForDeleting)
                    {
                        _Context.Categories.Remove(cat);
                    }

                    await _Context.SaveChangesAsync();
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
                return BadRequest();
            }
        }

        // It is used for development and testing purposes only!
        [HttpPost("AddTransactionFromBody")]
        [Authorize]
        public async Task<ActionResult<Transaction>> AddTransactionFromBodyAsync([FromQuery] string name,
            [FromBody] Transaction transaction)
        {
            _Logger.LogInformation("*** Method AddTransactionFromBodyAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            ValidationResult categoriesResult = await _CategoriesValidator.ValidateAsync(transaction.Categories);

            if (!categoriesResult.IsValid)
            {
                foreach (var error in categoriesResult.Errors)
                {
                    _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                }

                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

            if (person != null)
            {
                transaction.PersonId = person.Id;
                transaction.Categories.PersonId = person.Id;

                person.Transactions.Add(transaction);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation($"*** Addition a new transaction for a user by the name of {name}. ***");
                return transaction;
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return BadRequest();
            }
        }

        // The function that adds transaction for the specific user.
        [HttpPost("AddTransaction")]
        [Authorize]
        public async Task<ActionResult> AddTransactionAsync([FromQuery] string name,
            [FromBody] Categories categories)
        {
            _Logger.LogInformation("*** Method AddTransactionAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            ValidationResult categoriesResult = await _CategoriesValidator.ValidateAsync(categories);

            if (!categoriesResult.IsValid)
            {
                foreach (var error in categoriesResult.Errors)
                {
                    _Logger.LogWarning($"The property {error.PropertyName} has the error: {error.ErrorMessage}");
                }

                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

            if (person != null)
            {
                Transaction transaction = new Transaction();
                transaction.PersonId = person.Id;
                transaction.Categories = categories;
                transaction.Categories.PersonId = person.Id;

                person.Transactions.Add(transaction);
                await _Context.SaveChangesAsync();
                _Logger.LogInformation($"*** Addition a new transaction for a user by the name of {name}. ***");
                return Ok();
            }
            else
            {
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return BadRequest();
            }
        }

        // The function that returns the last realized transaction for the specific user.
        [HttpGet("GetLastTransaction")]
        [Authorize]
        public async Task<ActionResult<Categories>> GetLastTransactionAsync([FromQuery] string name)
        {
            _Logger.LogInformation("*** Method GetLastTransactionAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    var lastTransaction = person.Transactions.LastOrDefault();
                    _Logger.LogInformation($"*** Getting a last transaction for a user by the name of {name}. ***");
                    var categories = lastTransaction!.Categories;
                    return categories;
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
                return BadRequest();
            }
        }

        // The function that deletes the last realized transaction for the specific user.
        [HttpDelete("DeleteLastTransaction")]
        [Authorize]
        public async Task<ActionResult> DeleteLastTransactionAsync([FromQuery] string name)
        {
            _Logger.LogInformation("*** Method DeleteLastTransactionAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

            if (person != null)
            {
                if (person.Transactions.Count() != 0)
                {
                    var lastTransaction = person.Transactions.LastOrDefault();
                    _Context.Transactions.Remove(lastTransaction!);
                    var categoriesFromLastTransaction = lastTransaction!.Categories;
                    _Context.Categories.Remove(categoriesFromLastTransaction);
                    await _Context.SaveChangesAsync();
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
                _Logger.LogWarning($"*** No user by the name of {name} in the list. ***");
                return BadRequest();
            }
        }

        // The function that returns all realized transactions for the specific user.
        [HttpGet("GetAllTransactionsByPerson")]
        [Authorize]
        public async Task<ActionResult<List<Transaction>>> GetAllTransactionsByPersonAsync([FromQuery] string name)
        {
            Person? person = await _Context.GetPersonByNameAsync(name);

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
                return BadRequest();
            }
        }

        // The function that returns the specific user expenses for the current month.
        [HttpGet("GetExpensesForThisMonth")]
        [Authorize]
        public async Task<ActionResult<Categories>> GetExpensesForThisMonthAsync([FromQuery] string name, bool giveInPercents)
        {
            _Logger.LogInformation("*** Method GetExpensesForThisMonthAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

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
                return BadRequest();
            }
        }

        // The function that returns the specific user expenses for the current year.
        [HttpGet("GetExpensesForThisYear")]
        [Authorize]
        public async Task<ActionResult<Categories>> GetExpensesForThisYearAsync([FromQuery] string name, bool giveInPercents)
        {
            _Logger.LogInformation("*** Method GetExpensesForThisYearAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

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
                _Logger.LogWarning($"*** No user by the name of {name} in the DB. ***");
                return BadRequest();
            }
        }

        // The function that returns the specific user expenses for the specific month.
        [HttpGet("GetExpensesForSpecificMonth")]
        [Authorize]
        public async Task<ActionResult<Categories>> GetExpensesForSpecificMonthAsync([FromQuery] string name, int month,
            int year, bool giveInPercents)
        {
            _Logger.LogInformation("*** Method GetExpensesForSpecificMonthAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            if (_dateValidator.ValidateDate(month, year))
            {
                Person? person = await _Context.GetPersonByNameAsync(name);

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
                    return BadRequest();
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
        public async Task<ActionResult<Categories>> GetExpensesForSpecificYearAsync([FromQuery] string name, int year,
            bool giveInPercents)
        {
            _Logger.LogInformation("*** Method GetExpensesForSpecificYearAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            if (_dateValidator.ValidateDate(year))
            {
                Person? person = await _Context.GetPersonByNameAsync(name);

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
                    return BadRequest();
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
        public async Task<ActionResult<Categories>> GetExpensesForLastWeekAsync([FromQuery] string name, bool giveInPercents)
        {
            _Logger.LogInformation("*** Method GetExpensesForLastWeekAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            Person? person = await _Context.GetPersonByNameAsync(name);

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
                return BadRequest();
            }
        }

        // The function that returns the specific user expenses for a specific period.
        [HttpGet("GetExpensesForSpecificPeriod")]
        [Authorize]
        public async Task<ActionResult<Categories>> GetExpensesForSpecificPeriodAsync
            ([FromQuery] string name, int dayStart, int monthStart, int yearStart,
            int dayEnd, int monthEnd, int yearEnd, bool giveInPercents)
        {
            _Logger.LogInformation("*** Method GetExpensesForSpecificPeriodAsync started. ***");

            if (string.IsNullOrEmpty(name))
            {
                _Logger.LogWarning("*** Name is null or empty string. ***");
                return BadRequest();
            }

            if (_dateValidator.ValidateDate(dayStart, monthStart, yearStart, dayEnd, monthEnd, yearEnd))
            {
                Person? person = await _Context.GetPersonByNameAsync(name);

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
                    return BadRequest();
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