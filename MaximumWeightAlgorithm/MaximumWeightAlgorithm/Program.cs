using System;
using System.Collections.Generic;

namespace MaximumWeightAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> filtered = new Dictionary<string, string>();


            Graph init = new Graph(3);

            string[] lines = System.IO.File.ReadAllLines(@"C:\Lines.txt");

            char delimiter = ',';
            foreach (var line in lines)
            {
                string[] splitline =  line.Split(delimiter);

                init.AddEdge(splitline[0], splitline[1], int.Parse(splitline[2]));
            }

            Console.WriteLine("Graph builded, size is: " + init.GetNodes().Count);
            Console.WriteLine(init.RemoveIsolatedNodes());
            Console.WriteLine(init.RemoveRepetitions(90));
            Console.WriteLine(init.GetNodes().Count);
//            Console.ReadKey();

        }
    }
}


