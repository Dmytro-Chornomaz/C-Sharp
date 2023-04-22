using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HomeWork3
{
    internal class Pirate<P> where P : class, new()
    {
        public P weapon { get; set; }

        public Pirate(P weapon) { this.weapon = weapon; }

        public P Return<Gun>()
        {
            return weapon;
        }

    }    
}
