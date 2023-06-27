namespace Finance_Organizer
{
    public class Categories
    {
        //public int PersonId { get; set; }
        //public int Id { get; set; }

        private double meal = 0;
        private double communalServices = 0;
        private double medicine = 0;
        private double transport = 0;
        private double purchases = 0;
        private double leisure = 0;
        private double savings = 0;

        public double Meal { get => meal; set => meal = Math.Round(value, 2); }
        public double CommunalServices { get => communalServices; set => communalServices = Math.Round(value, 2); }
        public double Medicine { get => medicine; set => medicine = Math.Round(value, 2); }
        public double Transport { get => transport; set => transport = Math.Round(value, 2); }
        public double Purchases { get => purchases; set => purchases = Math.Round(value, 2); }
        public double Leisure { get => leisure; set => leisure = Math.Round(value, 2); }
        public double SummaryExpenses => Meal + CommunalServices + Medicine + Transport + Purchases + Leisure;
        public double Savings { get => savings; set => savings = Math.Round(value, 2); }


        public static Categories CategoriesSum(IEnumerable<Categories> categories)
        {
            Categories result = new Categories();

            foreach (var cat in categories)
            {
                result.Meal += cat.Meal;
                result.CommunalServices += cat.CommunalServices;
                result.Medicine += cat.Medicine;
                result.Transport += cat.Transport;
                result.Purchases += cat.Purchases;
                result.Leisure += cat.Leisure;
                result.Savings += cat.Savings;
            }
            return result;
        }
    }
}
