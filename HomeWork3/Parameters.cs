using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3
{
    internal static class Parameters
    {
        public static void TheParameters<R>(this R gun) where R : IInfo, IName
        {
            Console.WriteLine("This example of a weapon has the following parameters:");
            gun.ShowInfo();
        }
    }
}
