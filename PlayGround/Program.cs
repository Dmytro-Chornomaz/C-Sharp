
decimal value = 104.64257m;

var count = BitConverter.GetBytes(decimal.GetBits(value)[3])[2];

Console.WriteLine(count);

decimal a = 123.11m;

if ((a - decimal.Round(a, 2)) != 0)
{
    Console.WriteLine("Oops!");
}