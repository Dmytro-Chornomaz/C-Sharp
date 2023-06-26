namespace Finance_Organizer
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction> { };
    }
}