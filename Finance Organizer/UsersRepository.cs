namespace Finance_Organizer
{
    public class UsersRepository : IUsersRepository
    {
        public ApplicationDbContext Context { get; }

        public UsersRepository(ApplicationDbContext context)
        {
            Context = context;
        }

        public Person GetPersonByName(string name)
        {
            Person person = Context.Users.FirstOrDefault(x => x.Name == name);
            return person;
        }
    }
}
