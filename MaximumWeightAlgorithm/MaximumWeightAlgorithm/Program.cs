using System;
using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"C:\Lines.txt");
            const char delimiter = ',';
            var edges =
                lines.Select(line => line.Split(delimiter)).Select(splitline =>
                        new Edge(int.Parse(splitline[0]), int.Parse(splitline[1]), int.Parse(splitline[2]))).ToList();

            foreach (var VARIABLE in edges)
            {
                Console.WriteLine(VARIABLE);
            }
            var mates = MaxWeightMatching.MaxWMatching(edges);


            Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            foreach (var VARIABLE in mates)
            {
                Console.WriteLine(VARIABLE);
            }

        }
    }
}