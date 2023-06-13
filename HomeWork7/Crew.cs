using static HomeWork7.Controllers.PiratesController;

namespace HomeWork7
{
    public class Crew : ICrew
    {
        public List<Pirate> Pirates { get; set; } = new List<Pirate>
        {
            new(){Id = 1, Name = "Jack the Sparrow", Age = "40", Description = "Eccentric with a couple of pistols"},
            new(){Id = 2, Name = "Billy Bounce", Age = "Old stump", Description = "Pale alcoholic"}
        };

        public Pirate? GetByName(string name)
        {
            return Pirates.FirstOrDefault(x => x.Name == name);
        }
    }
}
