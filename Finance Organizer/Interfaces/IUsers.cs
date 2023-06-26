namespace Finance_Organizer
{
    public interface IUsers
    {
        List<Person> ListOfUsers { get; set; }

        public Person GetPersonByName(string name);
    }
}