
namespace Finance_Organizer
{
    public class Users : IUsers
    {
        public List<Person> ListOfUsers { get; set; } = new List<Person> { };
        
        public Person GetPersonByName(string name)
        {
            Person person = ListOfUsers.FirstOrDefault(x => x.Name == name);
            return person;
        }
    }
}
