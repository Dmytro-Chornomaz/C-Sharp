using Finance_Organizer;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Transactions;
using System.Xml.Linq;
using Transaction = Finance_Organizer.Transaction;

var options = new JsonSerializerOptions
{
    WriteIndented = true
};

Transaction transactionD1 = new()
{
    Id = 1,
    Name = "the name",
    Time = new DateTime(2023, 5, 20, 18, 30, 25),
    Categories = new()
    {
        Meal = 50,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 25,
        Purchases = 0,
        Leisure = 0,
        Savings = 0
    }
};
Transaction transactionD2 = new()
{
    Id = 2,
    Name = "smth",
    Time = new DateTime(2023, 5, 21, 11, 12, 37),
    Categories = new()
    {
        Meal = 0,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 15,
        Purchases = 0,
        Leisure = 0,
        Savings = 0
    }
};
Transaction transactionD3 = new()
{
    Id = 3,
    Name = "yahoo",
    Time = new DateTime(2023, 6, 7, 14, 52, 52),
    Categories = new()
    {
        Meal = 0,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 0,
        Purchases = 0,
        Leisure = 220,
        Savings = 0
    }
};
Transaction transactionD4 = new()
{
    Id = 4,
    Name = "incredible",
    Time = new DateTime(2023, 6, 13, 15, 15, 10),
    Categories = new()
    {
        Meal = 0,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 0,
        Purchases = 0,
        Leisure = 0,
        Savings = 500
    }
};

Transaction transactionA1 = new()
{
    Id = 1,
    Name = "ho-ho-ho",
    Time = new DateTime(2023, 4, 19, 12, 12, 25),
    Categories = new()
    {
        Meal = 76,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 24,
        Purchases = 0,
        Leisure = 0,
        Savings = 0
    }
};
Transaction transactionA2 = new()
{
    Id = 2,
    Name = "Big Boy",
    Time = new DateTime(2023, 4, 21, 10, 18, 33),
    Categories = new()
    {
        Meal = 0,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 0,
        Purchases = 64,
        Leisure = 0,
        Savings = 0
    }
};
Transaction transactionA3 = new()
{
    Id = 3,
    Name = "nothing",
    Time = new DateTime(2023, 5, 13, 17, 15, 52),
    Categories = new()
    {
        Meal = 0,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 74,
        Purchases = 0,
        Leisure = 0,
        Savings = 0
    }
};
Transaction transactionA4 = new()
{
    Id = 4,
    Name = "neverland",
    Time = new DateTime(2023, 6, 15, 10, 15, 36),
    Categories = new()
    {
        Meal = 0,
        CommunalServices = 740,
        Medicine = 0,
        Transport = 0,
        Purchases = 0,
        Leisure = 0,
        Savings = 0
    }
};

Person dimon = new Person()
{
    Id = 1,
    Name = "Dimon",
    Account = new()
    {
        Id = 1,
        Transactions = new() { transactionD1, transactionD2, transactionD3, transactionD4 }
    }
};
Person adolf = new Person()
{
    Id = 2,
    Name = "Adolf",
    Account = new()
    {
        Id = 2,
        Transactions = new() { transactionA1, transactionA2, transactionA3, transactionA4 }
    }
};

List<Person> people = new List<Person>() { };

people.Add(dimon);
people.Add(adolf);

void GetExpensesForThisMonth(string name)
{
    var person = people.FirstOrDefault(x => x.Name == name);
    DateTime dateTime = DateTime.Now;
    var transactions = person.Account.Transactions.Where(x => x.Time.Month == dateTime.Month);
    var categories = transactions.Select(x => x.Categories);   
    string jsonDimonRequest = JsonSerializer.Serialize(categories, options);
    Console.WriteLine(jsonDimonRequest);
}

GetExpensesForThisMonth("Adolf");







//var options = new JsonSerializerOptions
//{
//    WriteIndented = true
//};
//string jsonDimon = JsonSerializer.Serialize(dimon, options);
//string jsonAdolf = JsonSerializer.Serialize(adolf, options);

//Console.WriteLine(jsonAdolf);
