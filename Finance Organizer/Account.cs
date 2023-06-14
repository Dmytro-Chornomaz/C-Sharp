namespace Finance_Organizer
{
    public class Account : IAccount
    {
        public int Id { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction> { };
    }
}
