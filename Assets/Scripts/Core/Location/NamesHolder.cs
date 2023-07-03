using System.Text;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public class NamesHolder
    {
        public static NamesHolder Instance;
        public string[] FirstNames, LastNames;


        public static void Init()
        {
            if (Instance == null)
            {
                Instance = new NamesHolder();
                if (Instance.FirstNames == null)
                {
                    Instance.FirstNames = LoadFromFile("names");
                    Instance.LastNames = LoadFromFile("lastnames");
                }
            }
        }

        public static string GetFirstName(Random rnd)
        {
            Init();
            return Instance.FirstNames[rnd.Next(0, Instance.FirstNames.Length)];
        }
        public static string GetLastName(Random rnd)
        {
            Init();
            return Instance.LastNames[rnd.Next(0, Instance.LastNames.Length)];
        }
        private static string[] LoadFromFile(string nm)
        {
            TextAsset mytxtData = (TextAsset) Resources.Load(nm);
            var wrds = mytxtData.text;
            return wrds.Split('/');
        }
    
        public static string ToUpperFist(string str)
        {
            if (str.Length == 1)
                return char.ToUpper(str[0]).ToString();
            return (char.ToUpper(str[0]) + str.Substring(1).ToLower());
        }

        public static int StringToSeed(string name)
        {
            int seed = 0;
            foreach (var ch in Encoding.ASCII.GetBytes(name))
            {
                seed += ch;
            }

            return seed;
        }
    }
}