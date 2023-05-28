using System;
using System.Linq;
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

Console.WriteLine("Serializer:");
Console.WriteLine(Serializer(pirate));
Console.WriteLine(new string('-', 40));

Console.WriteLine("Deserializer:");
string pairsPropertyAndValue = "<Name>:<Orest>;<Age>:<66>;";
Deserializer(pairsPropertyAndValue);
pirate.Print();
Console.WriteLine(new string('-', 40));


Pirate GenericDeserializer(string pairsPropertyAndValue)
{
    Type myType = typeof(Pirate);

    char[] delimiterChars = { '<', '>', ':', ';' };

    string[] words = pairsPropertyAndValue.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

    List<string> propTitles = new List<string>();
    List<string> propValues = new List<string>();

    for (int i = 0; i < words.Length; i++)
    {
        if (i == 0 || i % 2 == 0)
        {
            propTitles.Add(words[i]);
        }
        else
        {
            propValues.Add(words[i]);
        }
    }

    for (int i = 0; i < propTitles.Count; i++)
    {
        var titleProp = myType.GetProperty(propTitles[i]);
        int number = 0;
        if (int.TryParse(propValues[i], out number))
        {
            titleProp?.SetValue(pirate, number);
        }
        else
        {
            titleProp?.SetValue(pirate, propValues[i]);
        }

    }

    return pirate;
}

Pirate Deserializer(string pairsPropertyAndValue)
{
    Type myType = typeof(Pirate);

    char[] delimiterChars = { '<', '>', ':', ';' };

    string[] words = pairsPropertyAndValue.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);

    List<string> propTitles = new List<string>();
    List<string> propValues = new List<string>();

    for (int i = 0; i < words.Length; i++)
    {
        if (i == 0 || i % 2 == 0)
        {
            propTitles.Add(words[i]);
        }
        else
        {
            propValues.Add(words[i]);
        }
    }

    for (int i = 0; i < propTitles.Count; i++)
    {
        var titleProp = myType.GetProperty(propTitles[i]);
        int number = 0;
        if (int.TryParse(propValues[i], out number))
        {
            titleProp?.SetValue(pirate, number);
        }
        else
        {
            titleProp?.SetValue(pirate, propValues[i]);
        }

    }

    return pirate;
}

string Serializer(Pirate pirate)
{
    Type myType = typeof(Pirate);
    string? result = null;
    foreach (PropertyInfo prop in myType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
    {
        var titleProp = myType.GetProperty(prop.Name);
        var valueProp = titleProp?.GetValue(pirate);
        result += $"<{prop.Name}>:<{valueProp}>;";
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

