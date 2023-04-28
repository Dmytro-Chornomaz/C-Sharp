using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork4
{
    public static class EnumerableExtension
    {
        //10. Create your own LINQ method that will return the penultimate element from the end.
        public static int Penultimate(this IEnumerable<int> source)
        {
            
             int[] arr = (int[])source;

             int element = arr[arr.Length - 2];

             return element;
            
        }
        
    }
}
