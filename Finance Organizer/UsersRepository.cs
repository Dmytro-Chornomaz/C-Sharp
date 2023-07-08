using Microsoft.EntityFrameworkCore;

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
            Person person = Context.Users
                .Include(x => x.Transactions).ThenInclude(y => y.Categories).FirstOrDefault(x => x.Name == name)!;
            return person;
        }
    }
}
