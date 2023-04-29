using HomeWork4;
using System.Runtime.CompilerServices;

internal partial class Program
{
    private static void Main(string[] args)
    {
        //1. Create an array of numbers from one to 100.

        int[] myArray = Enumerable.Range(1, 100).ToArray();

        //2. Sample only odd LINQ values.

        int[] oddNumbers = myArray.Where(x => x % 2 == 0).ToArray();

        Console.WriteLine("Sample only odd LINQ values");

        foreach (var item in oddNumbers)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();

        //3. Sample the squares of these numbers using LINQ.

        int[] squaresOfOddNumbers = oddNumbers.Select(x => x * x).ToArray();

        Console.WriteLine("Sample the squares of these numbers using LINQ");

        foreach (var item in squaresOfOddNumbers)
        {
            Console.WriteLine(item);
        }

        Console.WriteLine();

        //4. Find the sum of all elements of an array.

        Console.WriteLine("Find the sum of all elements of an array");

        int sum = squaresOfOddNumbers.Sum(x => x);

        Console.WriteLine(sum);

        Console.WriteLine();

        //6. Create a list of instances from step 5 for at least 20 pieces.

        List<Person> peoples = new List<Person>
        {
            new Person (1, "John", 33),
            new Person (2, "Bill", 27),
            new Person (3, "Anna", 8),
            new Person (4, "Jerry", 5),
            new Person (5, "Tom", 7),
            new Person (6, "Bob", 48),
            new Person (7, "Alice", 55),
            new Person (8, "Dora", 30),
            new Person (9, "Lina", 21),
            new Person (10, "Evald", 14),
            new Person (11, "Udo", 19),
            new Person (12, "Mary", 42),
            new Person (13, "Otto", 73),
            new Person (14, "Artur", 36),
            new Person (15, "Olivia", 44),
            new Person (16, "Max", 49),
            new Person (17, "Anna", 55),
            new Person (18, "Paul", 57),
            new Person (19, "Frank", 32),
            new Person (20, "Richard", 25),

        };

        //7. Filter people over 20 years old and display their names.

        var peoplesOver20 = peoples.Where(x => x.Age > 20);

        Console.WriteLine("Filter people over 20 years old and display their names");

        foreach (var item in peoplesOver20)
        {
            Console.WriteLine($"{item.Id} - {item.Name} - {item.Age}");
        }

        Console.WriteLine();

        //8. Filter people over the age of 20 and transform them into
        //anonymous objects with Id, Name fields and alphabetically sorted names.

        var anon = peoplesOver20.Select(x => new {x.Id, x.Name}).OrderBy(x => x.Name);

        Console.WriteLine("Anonymous objects");

        foreach (var item in anon)
        {
            Console.WriteLine($"{item.Name} - {item.Id}");
        }

        Console.WriteLine();


        //9. Filter people who are over 20 years old and transform them into
        //anonymous objects with Id, Name fields by grouping them by age.
        //Write the result to the Dictionary.

        var anonInDictionary = peoplesOver20.GroupBy(x => x.Age)
        .Select(x => new { Age = x.Key, Persons = x.Select(y => new { y.Age, y.Name })})
        .ToDictionary(x => x.Age);

        
        

        
}







}


