namespace Finance_Organizer
{
    public interface IUsersRepository
    {
        ApplicationDbContext Context { get; }

        Person GetPersonByName(string name);
    }
}