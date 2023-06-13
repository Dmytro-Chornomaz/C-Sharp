using HomeWork7.Controllers;

namespace HomeWork7
{
    public interface ICrew
    {
        PiratesController.Pirate GetByName(string name);
    }
}