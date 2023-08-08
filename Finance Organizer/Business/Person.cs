using System.ComponentModel.DataAnnotations;

namespace Finance_Organizer.Business
{
    // This class contains the data of a specific user.
    public class Person
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Transaction> Transactions { get; set; } = new List<Transaction> { };
    }
}