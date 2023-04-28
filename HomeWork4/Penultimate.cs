using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HomeWork4
{
    public static class EnumerableExtension
    {
        //10. Create your own LINQ method that will return the penultimate element from the end.
        public static IEnumerable<TSource> Penultimate<TSource>(this IEnumerable<TSource> source)
        {
            //int index = 0;

            //foreach (var element in source)
            //{
            //    index++;
            //}

            //int penultimate = index - 1;

            //yield return source.ToList().ElementAt(penultimate);

            source = source.ToList();
            
            int index = source.Count() - 1;

            var answer = source.ElementAt(index);

            yield return answer;

            
        }
        
    }
}
