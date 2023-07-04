using HomeWork7.Controllers;

namespace HomeWork7
{
    public interface IPiratesRepository
    {
        ApplicationContext Context { get; }

        PiratesController.Pirate? GetByName(string name);
    }
}