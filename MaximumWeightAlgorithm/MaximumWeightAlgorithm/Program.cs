using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography.X509Certificates;

namespace MaximumWeightAlgorithm
{
    class Program
    {
        private static int _oldNodesCount = 0;

        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines(@"C:\Users\ceesj\Documents\Stage\Progetto coccosimeoni\ScaffoldingProject\C_Sharp_Version\Lines.txt");
            const char delimiter = ',';
            var edges =
                lines.Select(line => line.Split(delimiter)).Select(splitline =>
                    new Edge(new Node(int.Parse(splitline[0]) + "", int.Parse(splitline[0])),
                        new Node(int.Parse(splitline[1]) + "", int.Parse(splitline[1])),
                        float.Parse(splitline[2]))).ToList();


            edges.ForEach(Console.WriteLine);

            //MaxWeightMatching.MaxWMatching(edges).ForEach(Console.WriteLine);


            Console.WriteLine("***********************************");
            var edges2 = new MyList<Edge>(edges);
            var list = BlossomAlgorithm(edges2);
            for (var i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
            Console.WriteLine("***********************************");
            var resultsMaxWeightMatching = MaxWeightMatching.MaxWMatching(list);
            foreach (var t in resultsMaxWeightMatching )
            {
                Console.WriteLine(t);
            }
            Console.WriteLine("***********************************");

            var shrinked = ShrinkBack(resultsMaxWeightMatching);
            foreach (var variable in shrinked)
            {
                Console.WriteLine(variable.Item1 + ", " + variable.Item2);
            }
        }

        private static List<Tuple<int, int>> ShrinkBack(List<int> results)
        {
            var alreadyVisited = new MyList<bool>(results.Count);
            var result = new List<Tuple<int, int>>();

            for (var i = 0; i < result.Count; i++)
            {
                alreadyVisited[i] = false;
            }

            for (var i = 0; i < _oldNodesCount; i++)
            {
                if (alreadyVisited[i])
                    continue;
                if (results[i] == -1)
                    continue;
                var v = Math.Min(i, results[i]);
                var endpoint = Math.Max(results[i], i);
                var otherEndPoint = endpoint % 2 == 0 ? endpoint + 1 : endpoint - 1;
                if (results[otherEndPoint] == -1)
                    continue;
                var lastV = results[otherEndPoint];

                alreadyVisited[i] = true;
                alreadyVisited[results[i]] = true;
                alreadyVisited[endpoint] = true;
                alreadyVisited[otherEndPoint] = true;
                alreadyVisited[lastV] = true;

                result.Add(new Tuple<int, int>(v / 2, lastV / 2));
            }
            return result;
        }

        private static MyList<BlossomEdge> BlossomAlgorithm(MyList<Edge> edges)
        {
            var newList = new MyList<BlossomEdge>();
            var nodes = new HashSet<int>();
            for (var i = 0; i < edges.Size(); i++)
            {
                var edge = edges[i];

                if (!nodes.Contains(edge.Start.Id))
                    nodes.Add(edge.Start.Id);
                if (!nodes.Contains(edge.End.Id))
                    nodes.Add(edge.End.Id);
            }
            _oldNodesCount = nodes.Count;
            var nodeCounter = nodes.Count * 2;

            for (var i = 0; i < edges.Size(); i++)
            {
                var startNode = edges[i].Start;
                var endNode = edges[i].End;
                var min = ++nodeCounter;
                var max = ++nodeCounter;
                var blossomEdge1 = new BlossomEdge
                {
                    OldEdge = edges[i],
                    Weight = edges[i].Weight,
                    Start = new Node(startNode.Id * 2 + "", startNode.Id * 2),
                    End = new Node(min + "", min)
                };
                var blossomEdge2 = new BlossomEdge
                {
                    OldEdge = edges[i],
                    Weight = edges[i].Weight,
                    Start = new Node(startNode.Id * 2 + 1 + "", startNode.Id * 2 + 1),
                    End = new Node(min + "", min)
                };
                var blossomEdge3 = new BlossomEdge
                {
                    OldEdge = edges[i],
                    Weight = edges[i].Weight,
                    Start = new Node(endNode.Id * 2 + "", endNode.Id * 2),
                    End = new Node(max + "", max)
                };
                var blossomEdge4 = new BlossomEdge
                {
                    OldEdge = edges[i],
                    Weight = edges[i].Weight,
                    Start = new Node(endNode.Id * 2 + 1 + "", endNode.Id * 2 + 1),
                    End = new Node(max + "", max)
                };
                var blossomEdge5 = new BlossomEdge
                {
                    OldEdge = edges[i],
                    Weight = edges[i].Weight,
                    Start = new Node(min + "", min),
                    End = new Node(max + "", max)
                };

                newList.Add(blossomEdge1);
                newList.Add(blossomEdge2);
                newList.Add(blossomEdge3);
                newList.Add(blossomEdge4);
                newList.Add(blossomEdge5);
            }
            return newList;
        }


        private static void Expand(int amountOfNodes, MyList<Edge> edges)
        {
            var transformedGraph = new List<Edge>();
            var nodeCounter = amountOfNodes * 2;
            Console.WriteLine(amountOfNodes);
            edges.ForEach(Console.WriteLine);

            for (var i = 0; i < edges.Size(); i++)
            {
//                int weight = edges[i][2];
//                int min = NodeCounter++;
//                int max = NodeCounter++;
//                int u = edges[i][0];
//                int v = edges[i][1];
//
//                if (u > v)
//                {
//                    var tmp = u;
//                    u = v;
//                    v = tmp;
//                }
//
//                TransformedGraph.Add(new Edge(u * 2, min, weight));
//                TransformedGraph.Add(new Edge(u * 2 + 1, min, weight));
//                TransformedGraph.Add(new Edge(v * 2, max, weight));
//                TransformedGraph.Add(new Edge(v * 2 + 1, max, weight));
//                TransformedGraph.Add(new Edge(min, max, weight));
            }
            Console.WriteLine(transformedGraph.Count);
            //TransformedGraph.ForEach(Console.WriteLine);
        }
    }
}