using HomeWork1;
using System;
using System.ComponentModel;
class Person : ISkill, IRequest
{
    public static double SalaryRate = 20000;
    public static double RobustRate = 1;
    public static double VacationTerm = 30;
    FavoriteFruits favoriteFruits;

    public void Skill(string skill)
    {

        Console.WriteLine($"My skill: {skill}");
    }

    public void Request(string request)
    {

        Console.WriteLine($"My request: {request}");
    }

    public void TheCounter(double SalaryRate, double RobustRate, double VacationTerm,
                        double ModSR, double ModRR, double ModVT)
    {
        Console.WriteLine($"My salary: {SalaryRate * ModSR}");
        Console.WriteLine($"My robust: {RobustRate * ModRR}");
        Console.WriteLine($"My vacation term: {VacationTerm * ModVT}");
        
    }

    public virtual void Bonus()
    {
        Console.WriteLine($"My bonus is {(int)BonusEnum.BigBonus} UAH");
    }

    public void FavoriteFruits()
    {
        Random randomQuantity = new Random();

        int quantity = randomQuantity.Next(0, 10);

        Random randomFruit = new Random();

        int fruit = randomFruit.Next(0, 3);

        string fruitName = ((FruitsEnum)fruit).ToString();

        favoriteFruits = new FavoriteFruits(quantity, fruitName);

        Console.WriteLine($"I like to eat {favoriteFruits.Quantity} {favoriteFruits.Fruits} at time!");
    }


    public void WeightHandler(int w)
    {
        Weight workerWeight = new Weight(w);

        Console.WriteLine($"I have a weight of {workerWeight.weight} kg");
    }
}
