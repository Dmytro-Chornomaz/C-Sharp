using System;
using System.Reflection;
using System.Text.Json;

Pirate pirate = new Pirate("Jack", 21);

Console.WriteLine("Get property values:");
GetProp(pirate, "Name");
GetProp(pirate, "Age");
GetProp(pirate, "Weapon");
Console.WriteLine(new string('-', 40));

Console.WriteLine("Set property values:");
SetProp(pirate, "Name", "Bobby");
SetProp(pirate, "Weapon", "Bobby");
Console.WriteLine(new string('-', 40));

Console.WriteLine("Get pairs name - value:");
Console.WriteLine(Deconstructor(pirate));
Console.WriteLine(new string('-', 40));

Console.WriteLine("Get serialized pairs:");
Console.WriteLine(Serializer(pirate));
Console.WriteLine(new string('-', 40));

Console.WriteLine("Get deserialized pirate:");
var jsonPirate = @"{""Name"":""Oliver"",""Age"":33}";
pirate = Deserializer(jsonPirate);
pirate.Print();
Console.WriteLine(new string('-', 40));

Console.WriteLine("Get deserialized generic pirate:");
var jsonGenericPirate = @"{""Name"":""Brian"",""Age"":52}";
pirate = GenericDeserializer<Pirate>(jsonGenericPirate);
pirate.Print();
Console.WriteLine(new string('-', 40));

T GenericDeserializer<T>(string pairPropAndValue)
{
    T? deserializedPirate = JsonSerializer.Deserialize<T>(pairPropAndValue);
    return deserializedPirate;
}

Pirate Deserializer(string pairPropAndValue)
{
    Pirate? deserializedPirate = JsonSerializer.Deserialize<Pirate>(pairPropAndValue);
    return deserializedPirate;
}

string Serializer(Pirate pirate)
{
    string result = JsonSerializer.Serialize(pirate);
    return result;
}

string Deconstructor(Pirate pirate)
{
    Type myType = typeof(Pirate);
    string? result = null;
    foreach (PropertyInfo prop in myType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
    {
        var titleProp = myType.GetProperty(prop.Name);
        var valueProp = titleProp?.GetValue(pirate);
        result += $"{prop.Name} - {valueProp} \n";
    }
    return result;
}

void SetProp(Pirate pirate, string property, string value)
{
    Type myType = typeof(Pirate);
    var titleProp = myType.GetProperty(property);
    if (titleProp != null)
    {
        titleProp?.SetValue(pirate, value);
        pirate.Print();
    }
    else
    {
        Console.WriteLine($"The property {property} does not exist!");
    }
}

void SetProp2(Pirate pirate, string property, int value)
{
    Type myType = typeof(Pirate);
    var titleProp = myType.GetProperty(property);
    if (titleProp != null)
    {
        titleProp?.SetValue(pirate, value);
        pirate.Print();
    }
    else
    {
        Console.WriteLine($"The property {property} does not exist!");
    }
}

void GetProp(Pirate pirate, string property)
{
    Type myType = typeof(Pirate);
    var titleProp = myType.GetProperty(property);
    if (titleProp != null)
    {
        var valueProp = titleProp?.GetValue(pirate);
        Console.WriteLine(valueProp);
    }
    else
    {
        Console.WriteLine($"The property {property} does not exist!");
    }
}

public class Pirate
{
    public string Name { get; set; }
    public int Age { get; set; }
    

    public Pirate() { }

    public Pirate(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Print() => Console.WriteLine($"{Name} is {Age} years old");
}

