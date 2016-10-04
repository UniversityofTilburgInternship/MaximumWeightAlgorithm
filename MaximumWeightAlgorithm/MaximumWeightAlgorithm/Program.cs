using System;
using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"C:\users\ceesj\documents\Lines.txt");
            const char delimiter = ',';
            var edges =
                lines.Select(line => line.Split(delimiter)).Select(splitline =>
                        new Edge(int.Parse(splitline[0]), int.Parse(splitline[1]), int.Parse(splitline[2]))).ToList();


            edges.ForEach(Console.WriteLine);

            var amountOfNodes = new HashSet<int>();

            foreach (var edge in edges)
            {
                if (!(edge[0] >= 0 && edge[1] >= 0 && edge[0] != edge[1]))
                    throw new Exception("Assert Error");
                if (!amountOfNodes.Contains(edge[0]))
                    amountOfNodes.Add(edge[0]);
                if (!amountOfNodes.Contains(edge[1]))
                    amountOfNodes.Add(edge[1]);
            }

            var edges2 = new MyList<Edge>(edges);
            Console.WriteLine(amountOfNodes.Count);
            Expand(amountOfNodes.Count, edges2);
            //Amount of nodes | edges |


            //MaxWeightMatching.MaxWMatching(edges).ForEach(Console.WriteLine);
        }

        private static void Expand(int amountOfNodes, MyList<Edge> edges)
        {
            List<Edge> TransformedGraph = new List<Edge>();
            var NodeCounter = amountOfNodes * 2;
            Console.WriteLine(amountOfNodes);
            edges.ForEach(Console.WriteLine);

            for (int i = 0; i < edges.Size(); i++)
            {
                int weight = edges[i][2];
                int min = NodeCounter++;
                int max = NodeCounter++;
                int u = edges[i][0];
                int v = edges[i][1];

                if (u > v)
                {
                    var tmp = u;
                    u = v;
                    v = tmp;
                }

                TransformedGraph.Add(new Edge(u * 2, min, weight));
                TransformedGraph.Add(new Edge(u * 2 + 1, min, weight));
                TransformedGraph.Add(new Edge(v * 2, max, weight));
                TransformedGraph.Add(new Edge(v * 2 + 1, max, weight));
                TransformedGraph.Add(new Edge(min, max, weight));
            }
            Console.WriteLine(TransformedGraph.Count);
            //TransformedGraph.ForEach(Console.WriteLine);
        }
    }
}