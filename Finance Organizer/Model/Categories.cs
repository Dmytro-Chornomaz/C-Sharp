using System.ComponentModel.DataAnnotations;

namespace Finance_Organizer.Model
{
    /* This class contains a list of expense categories. An instance of this class is used both to enter 
     * the list of categories into the database and to present financial statements to the user. 
     * It is possible to view financial statements in both currency and percentage.*/
    public class Categories
    {
        [Key]
        public int Id { get; set; }
        public int PersonId { get; set; }

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

        /* This function summarizes the categories of expenses from different transactions, 
         * allowing you to get the total expenses for a certain period of time.*/
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

        // This function allows you to get a percentage representation of expenses.
        public static Categories CategoriesSumInPercents(Categories categories)
        {
            double allMoney = categories.SummaryExpenses + categories.Savings;
            double onePercent = allMoney / 100;

            Categories resultInPercents = new Categories();

            resultInPercents.Meal = categories.Meal / onePercent;
            resultInPercents.CommunalServices = categories.CommunalServices / onePercent;
            resultInPercents.Medicine = categories.Medicine / onePercent;
            resultInPercents.Transport = categories.Transport / onePercent;
            resultInPercents.Purchases = categories.Purchases / onePercent;
            resultInPercents.Leisure = categories.Leisure / onePercent;
            resultInPercents.Savings = categories.Savings / onePercent;

            return resultInPercents;
        }
    }
}
