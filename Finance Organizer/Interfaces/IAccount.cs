namespace Finance_Organizer
{
    public interface IAccount
    {
        int Id { get; set; }
        List<Transaction> Transactions { get; set; }
    }
}