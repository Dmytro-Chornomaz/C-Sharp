using static HomeWork7.Controllers.PiratesController;

namespace HomeWork7
{
    public class PiratesRepository : IPiratesRepository
    {
        public ApplicationContext Context { get; }

        public PiratesRepository(ApplicationContext context)
        {
            Context = context;
        }

        public Pirate? GetByName(string name)
        {
            return Context.PiratesDB.FirstOrDefault(x => x.Name == name);
        }
    }
}
