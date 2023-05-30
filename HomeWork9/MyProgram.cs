using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json;

Pirate pirate = new Pirate("Jack", 21, "married");

Console.WriteLine("Get property values:");
GetProp(pirate, "Name");
GetProp(pirate, "Age");
GetProp(pirate, "Status");
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
string pairsPropertyAndValue = "<Name>:<Orest>;<Age>:<66>;<Status>:<alcoholic>;";
Deserializer(pairsPropertyAndValue);
pirate.Print();
Console.WriteLine(new string('-', 40));

Console.WriteLine("GenericDeserializer:");
string pairsPropertyAndValue2 = "<Name>:<Arseniy>;<Age>:<77>;<Status>:<bastard>;";
GenericDeserializer(pairsPropertyAndValue2);
pirate.Print();
Console.WriteLine(new string('-', 40));


Pirate GenericDeserializer<T>(T theThing)
{
    Type myType = typeof(Pirate);

    char[] delimiterChars = { '<', '>', ':', ';' };

    Type str = typeof(String);
    Type thing = typeof(T);

    if (str.Equals(thing))
    {
        pairsPropertyAndValue = theThing.ToString();

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

            if (Attribute.IsDefined(titleProp, typeof(DisplayAttribute)))
            {
                if (int.TryParse(propValues[i], out number))
                {
                    titleProp?.SetValue(pirate, number);
                }
                else
                {
                    titleProp?.SetValue(pirate, propValues[i]);
                }
            }

        }

        return pirate;
    }

    else
    {
        Console.WriteLine("Incorrect parameter!");
        return pirate;
    }
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
  
        if (Attribute.IsDefined(titleProp, typeof(DisplayAttribute)))
        {
            if (int.TryParse(propValues[i], out number))
            {
                titleProp?.SetValue(pirate, number);
            }
            else
            {
                titleProp?.SetValue(pirate, propValues[i]);
            }
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
        if (Attribute.IsDefined(prop, typeof(DisplayAttribute)))
        {
            result += $"<{prop.Name}>:<{valueProp}>;";
        }
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
    [Display]
    public string Name { get; set; }
    [Display]
    public int Age { get; set; }
    public string Status { get; set; } = "unknown";

    public Pirate() { }

    public Pirate(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public Pirate(string name, int age, string status) : this(name, age)
    {
        Status = status;
    }

    public void Print() => Console.WriteLine($"{Name} is {Age} years old. His status is {Status}.");
}

[AttributeUsage(AttributeTargets.Property)]
class DisplayAttribute : Attribute
{

}