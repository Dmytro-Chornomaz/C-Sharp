using EntityFramework_PlayGround;

//Creating data
using (ApplicationContext db = new ApplicationContext())
{
    User tom = new User() { Name = "Tommy", Age = 21 };
    User alice = new User() { Name = "Alice", Age = 18 };

    db.Users.Add(tom);
    db.Users.Add(alice);
    db.SaveChanges();
}

//Reading data
using (ApplicationContext db = new ApplicationContext())
{
    var users = db.Users.ToList();

    Console.WriteLine("Data after adding:");

    foreach (var u in users)
    {
        Console.WriteLine($"{u.Id}: {u.Name} - {u.Age} years");
    }
}

//Updating data
using (ApplicationContext db = new ApplicationContext())
{
    var user = db.Users.FirstOrDefault(a => a.Name == "Alice");

    if (user != null)
    {
        user.Age = 20;
        db.SaveChanges();
    }

    var users = db.Users.ToList();

    Console.WriteLine("Data after updating:");

    foreach (var u in users)
    {
        Console.WriteLine($"{u.Id}: {u.Name} - {u.Age} years");
    }
}

//Deleting data
using (ApplicationContext db = new ApplicationContext())
{
    var user = db.Users.FirstOrDefault(a => a.Name == "Tommy");

    if (user != null)
    {
        db.Users.Remove(user);
        db.SaveChanges();
    }

    var users = db.Users.ToList();

    Console.WriteLine("Data after deleting:");

    foreach (var u in users)
    {
        Console.WriteLine($"{u.Id}: {u.Name} - {u.Age} years");
    }
}