using System.ComponentModel.DataAnnotations;

namespace Finance_Organizer.Business
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }

        public string Name { get; set; } = "No name";
        public DateTime Time { get; set; } = DateTime.Now;
        public Categories Categories { get; set; } = new Categories();

        public string Comment { get; set; } = "No comment";
    }
}
