using System;
using System.Collections.Generic;

namespace Core.Core
{
    public static class Extensions
    {
        public static void Shuffle<T> (this Random rng, T[] array)
        {
            int n = array.Length;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }
        }
    
        public static List<T> Shuffle<T> (this Random rng, List<T> array)
        {
            array = new List<T>(array);
            int n = array.Count;
            while (n > 1) 
            {
                int k = rng.Next(n--);
                (array[n], array[k]) = (array[k], array[n]);
            }

            return array;
        }
    }
}