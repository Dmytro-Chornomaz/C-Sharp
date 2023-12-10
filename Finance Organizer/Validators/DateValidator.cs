namespace Finance_Organizer.Validators
{
    public class DateValidator
    {
        DateTime today = DateTime.Now;

        public bool ValidateDate(int year)
        {
            bool checkYear = today.Year >= year && year > 2021;
            return checkYear;
        }

        public bool ValidateDate(int month, int year)
        {
            bool checkMonth = 12 >= month && month > 0;
            bool checkYear = today.Year >= year && year > 2021;
            bool result = checkMonth && checkYear;
            return result;
        }

        public bool ValidateDate(int dayStart, int monthStart, int yearStart, int dayEnd, int monthEnd, int yearEnd)
        {
            int daysInMonthStart = DateTime.DaysInMonth(yearStart, monthStart);
            bool checkDayStart = daysInMonthStart >= dayStart && dayStart > 0;
            bool checkMonthStart = 12 >= monthStart && monthStart > 0;
            bool checkYearStart = today.Year >= yearStart && yearStart > 2021;

            int daysInMonthEnd = DateTime.DaysInMonth(yearEnd, monthEnd);
            bool checkDayEnd = daysInMonthEnd >= dayEnd && dayEnd > 0;
            bool checkMonthEnd = 12 >= monthEnd && monthEnd > 0;
            bool checkYearEnd = today.Year >= yearEnd && yearEnd > 2021;

            bool yearsComparison = yearStart <= yearEnd;

            bool result = checkDayStart && checkMonthStart && checkYearStart && checkDayEnd &&
                checkMonthEnd && checkYearEnd && yearsComparison;

            return result;
        }
    }
}
