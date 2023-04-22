public class FavoriteFruits
{
    public FavoriteFruits(int quantity, string fruits)
    {
        Quantity = quantity;

        Fruits = fruits;
    }

    public string Fruits { get; set; }
    public int Quantity { get; set; }
}
