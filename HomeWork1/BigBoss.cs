class BigBoss : Person
{
    public string skill = "command and conquer!";
    public string request = "more money!!!";
    public double ModSR = 2;
    public double ModRR = 2;
    public double ModVT = 2;

    public void TheBigBoss()
    {
        Console.WriteLine("I am a BigBoss!");
    }

    public override void Bonus()
    {
        Console.WriteLine($"My bonus is {(int)BonusEnum.BigBonus} UAH and a big shiny diamond");
    }
}
