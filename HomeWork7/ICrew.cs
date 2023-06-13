using HomeWork7.Controllers;

namespace HomeWork7
{
    public interface ICrew
    {
        List<PiratesController.Pirate> Pirates { get; set; }

        PiratesController.Pirate? GetByName(string name);
    }
}