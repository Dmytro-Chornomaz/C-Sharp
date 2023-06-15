namespace Finance_Organizer
{
    public class Categories : ICategories
    {
        public double Meal { get; set; } = 0;
        public double CommunalServices { get; set; } = 0;
        public double Medicine { get; set; } = 0;
        public double Transport { get; set; } = 0;
        public double Purchases { get; set; } = 0;
        public double Leisure { get; set; } = 0;
        public double SummaryExpenses => Meal + CommunalServices + Medicine + Transport + Purchases + Leisure;
        public double Savings { get; set; } = 0;        
    }
}
