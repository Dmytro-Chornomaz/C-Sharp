using Finance_Organizer;
using System;
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
    Time = new DateTime(2022, 11, 29, 19, 30, 25),
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
    Time = new DateTime(2022, 12, 11, 15, 37, 37),
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
Transaction transactionD4 = new()
{
    Id = 4,
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
Transaction transactionD5 = new()
{
    Id = 5,
    Name = "yahoo",
    Time = new DateTime(2023, 6, 10, 14, 52, 52),
    Categories = new()
    {
        Meal = 23,
        CommunalServices = 0,
        Medicine = 0,
        Transport = 154,
        Purchases = 0,
        Leisure = 220,
        Savings = 0
    }
};
Transaction transactionD6 = new()
{
    Id = 6,
    Name = "incredible",
    Time = new DateTime(2023, 6, 13, 15, 15, 10),
    Categories = new()
    {
        Meal = 45,
        CommunalServices = 0,
        Medicine = 23,
        Transport = 64,
        Purchases = 0,
        Leisure = 15,
        Savings = 500
    }
};

Transaction transactionA1 = new()
{
    Id = 1,
    Name = "ho-ho-ho",
    Time = new DateTime(2022, 8, 15, 17, 12, 25),
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
    Time = new DateTime(2022, 10, 21, 12, 36, 33),
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
Transaction transactionA4 = new()
{
    Id = 4,
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
Transaction transactionA5 = new()
{
    Id = 5,
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
Transaction transactionA6 = new()
{
    Id = 6,
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



    Transactions = new() { transactionD1, transactionD2, transactionD3,
            transactionD4, transactionD5, transactionD6 }

};
Person adolf = new Person()
{
    Id = 2,
    Name = "Adolf",



    Transactions = new() { transactionA1, transactionA2, transactionA3,
            transactionA4, transactionA5, transactionA6 }

};

List<Person> people = new List<Person>() { };

people.Add(dimon);
people.Add(adolf);


GetExpensesForSpecificPeriod("Dimon", 1, 1, 2022, 15, 6, 2023);


void GetExpensesForSpecificPeriod
    (string name, int dayStart, int monthStart, int yearStart, int dayEnd, int monthEnd, int yearEnd)
{
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

        DateTime dateStart = new DateTime(yearStart, monthStart, dayStart);
        DateTime dateEnd = new DateTime(yearEnd, monthEnd, dayEnd);


        var person = people.FirstOrDefault(x => x.Name == name);
        var transactions = person.Transactions
            .Where(x => x.Time >= dateStart && x.Time <= dateEnd);
        var categories = transactions.Select(x => x.Categories);

        var result = CategoriesSum(categories);

        string jsonDimonRequest = JsonSerializer.Serialize(result, options);
        Console.WriteLine(jsonDimonRequest);
    }
    else
    {
        Console.WriteLine("Incorrect data!");
    }
}



void GetExpensesForLastWeek(string name)
{
    var person = people.FirstOrDefault(x => x.Name == name);
    DateTime dateTime = DateTime.Now;
    DateTime weekAgo = dateTime.AddDays(-7);
    var transactions = person.Transactions.Where(x => x.Time >= weekAgo);
    var categories = transactions.Select(x => x.Categories);

    var result = CategoriesSum(categories);

    string jsonDimonRequest = JsonSerializer.Serialize(result, options);
    Console.WriteLine(jsonDimonRequest);
}

void GetExpensesForThisMonth(string name)
{
    var person = people.FirstOrDefault(x => x.Name == name);
    DateTime dateTime = DateTime.Now;
    var transactions = person.Transactions.Where(x => x.Time.Month == dateTime.Month);
    var categories = transactions.Select(x => x.Categories);

    var result = CategoriesSum(categories);

    string jsonDimonRequest = JsonSerializer.Serialize(result, options);
    Console.WriteLine(jsonDimonRequest);
}

void GetExpensesForSpecificMonth(string name, int month, int year)
{
    var person = people.FirstOrDefault(x => x.Name == name);
    DateTime dateTime = new DateTime(year, month, 1);
    var transactions = person.Transactions
        .Where(x => x.Time.Month == dateTime.Month && x.Time.Year == dateTime.Year);
    var categories = transactions.Select(x => x.Categories);

    var result = CategoriesSum(categories);

    string jsonDimonRequest = JsonSerializer.Serialize(result, options);
    Console.WriteLine(jsonDimonRequest);
}

Categories CategoriesSum(IEnumerable<Categories> categories)
{
    Categories result = new Categories();

    foreach (var cat in categories)
    {
        result.Meal += cat.Meal;
        result.CommunalServices += cat.CommunalServices;
        result.Medicine += cat.Medicine;
        result.Transport += cat.Transport;
        result.Purchases += cat.Purchases;
        result.Leisure += cat.Leisure;
        result.Savings += cat.Savings;
    }
    return result;
}


static bool DaysInMonthValidator(int year, int month, int date)
{
    switch (month)
    {
        case 1: return date <= 31;
        case 2:
            if (DateTime.IsLeapYear(year)) { return date <= 29; }
            else { return date <= 28; }
        case 3: return date <= 31;
        case 4: return date <= 30;
        case 5: return date <= 31;
        case 6: return date <= 30;
        case 7: return date <= 31;
        case 8: return date <= 31;
        case 9: return date <= 30;
        case 10: return date <= 31;
        case 11: return date <= 30;
        case 12: return date <= 31;
        default: return false;
    }
}


//var options = new JsonSerializerOptions
//{
//    WriteIndented = true
//};
//string jsonDimon = JsonSerializer.Serialize(dimon, options);
//string jsonAdolf = JsonSerializer.Serialize(adolf, options);

//Console.WriteLine(jsonAdolf);
