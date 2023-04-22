using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3
{
    internal class Gun<G> : IName, IInfo
    {
        public string Name { get; set; }

        public G Damage { get; set; }

        public Gun() { }

        public Gun(string name, G damage) 
        {
            Name = name;
            Damage = damage;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Type: {Name} | Damage: {Damage}");
        }
    }
}
