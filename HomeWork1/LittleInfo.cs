static class LittleInfo
{
    public static void Salary()
    {
        Console.WriteLine($"As of {DateTime.Today.ToString()}, the basic salary rate is {Person.SalaryRate} UAH");
    }

    public static void Vacation()
    {
        Console.WriteLine($"The current basic term of vacation is {Person.VacationTerm * 24 * 3600} seconds");
    }
}
