using System;
using System.Collections.Generic;

namespace MaximumWeightAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> filtered = new Dictionary<string, string>();



            string[] lines = System.IO.File.ReadAllLines(@"C:\Lines.txt");

            char delimiter = ',';
            foreach (var line in lines)
            {
                string[] splitline =  line.Split(delimiter);

            }

//            Console.ReadKey();

        }
    }
}


