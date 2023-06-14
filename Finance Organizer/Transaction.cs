namespace Finance_Organizer
{
    public class Transaction
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime Time { get; set; } = DateTime.Now;
		public ICategories Categories { get; set; }
		public string Comment { get; set; } = "No comment";		
	}
}
