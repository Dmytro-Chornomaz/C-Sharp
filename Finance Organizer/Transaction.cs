namespace Finance_Organizer
{
    public class Transaction
	{
		public int AccountId { get; set; }
		public int Id { get; set; }
		public string Name { get; set; } = "No name";
		public DateTime Time { get; set; } = DateTime.Now;
		public Categories Categories { get; set; }
		public string Comment { get; set; } = "No comment";		
	}
}
