using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography.X509Certificates;

namespace MaximumWeightAlgorithm
{
    internal class Program
    {
        private static int _oldNodesCount = 0;
        static Dictionary<int, Node> _nodesDict = new Dictionary<int, Node>();
        static List<Edge> _edges = new List<Edge>();

        private static void Main(string[] args)
        {
            var lines =
                System.IO.File.ReadAllLines(
                    @"C:\Users\ceesj\Documents\Stage\Progetto coccosimeoni\ScaffoldingProject\C_Sharp_Version\Lines.txt");
            const char delimiter = ',';
            var splittedLines = lines.Select(line => line.Split(delimiter)).ToList();
            foreach (var line in splittedLines)
            {
                var startNodeInt = int.Parse(line[0]);
                var endNodeInt = int.Parse(line[1]);
                if (endNodeInt > startNodeInt)
                {
                    var tmp = endNodeInt;
                    endNodeInt = startNodeInt;
                    startNodeInt = tmp;
                }
                if (!_nodesDict.ContainsKey(startNodeInt))
                    _nodesDict.Add(startNodeInt, new Node(startNodeInt + "", startNodeInt));
                if (!_nodesDict.ContainsKey(endNodeInt))
                    _nodesDict.Add(endNodeInt, new Node(endNodeInt + "", endNodeInt));
                var newEdge = new Edge(_nodesDict[startNodeInt], _nodesDict[endNodeInt], float.Parse(line[2]));
                _nodesDict[startNodeInt].AddEdge(newEdge);
                _nodesDict[endNodeInt].AddEdge(newEdge);
                _edges.Add(newEdge);
            }

            _edges.ForEach(Console.WriteLine);
            Console.WriteLine("&&");
            foreach (var VARIABLE in _nodesDict)
            {
                Console.WriteLine(VARIABLE.Value);
            }
            //MaxWeightMatching.MaxWMatching(edges).ForEach(Console.WriteLine);


            var edges2 = new MyList<Edge>(_edges);
            var list = BlossomAlgorithm(edges2);
            for (var i = 0; i < list.Count; i++)
            {
                Console.WriteLine(list[i]);
            }
            Console.WriteLine("***********************************");
//            var resultsMaxWeightMatching = MaxWeightMatching.MaxWMatching(list);
//            foreach (var t in resultsMaxWeightMatching )
//            {
//                Console.WriteLine(t);
//            }
//            Console.WriteLine("***********************************");

//            var shrinked = ShrinkBack(resultsMaxWeightMatching);
//            foreach (var variable in shrinked)
//            {
//                Console.WriteLine(variable.Item1 + ", " + variable.Item2);
//            }
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

        private static List<BlossomEdge> BlossomAlgorithm(MyList<Edge> edges)
        {
            Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            var transformedEdges = new List<BlossomEdge>();
            var connectorNodes = new List<ConnectorNode>();

            foreach (var node in _nodesDict)
            {
                var temporaryConnectorNodes = new List<ConnectorNode>();
                var travelNodes = new List<TravelNode>();
                for (var i = 0; i < node.Value.Degrees; i++)
                {
                    var travelNode = new TravelNode(node.Key + i + "", node.Key);

                    var nodeTo = node.Value.getEdges()[i].End.Id == node.Value.Id
                        ? node.Value.getEdges()[i].Start.Id
                        : node.Value.getEdges()[i].End.Id;
                    var connectorNode = new ConnectorNode(node.Key + "_" + i + "", nodeTo, nodeTo);

                    travelNodes.Add(travelNode);
                    temporaryConnectorNodes.Add(connectorNode);
                }

                foreach (var travelNode in travelNodes)
                {
                    for (var j = 0; j < temporaryConnectorNodes.Count; j++)
                    {
                        var connectorNode = _nodesDict[temporaryConnectorNodes[j].Id];
                        var connectorNodeEdges = connectorNode.getEdges();
                        var edgeBetweenNodes =
                            connectorNodeEdges.Find(
                                edge => edge.Start.Id == connectorNode.Id || edge.End.Id == connectorNode.Id);
                        var blossomEdge = new BlossomEdge
                        {
                            Start = travelNode,
                            End = temporaryConnectorNodes[j],
                            Weight = edgeBetweenNodes.Weight,
                            OldEdge = edgeBetweenNodes
                        };
                        transformedEdges.Add(blossomEdge);
                    }
                }
                temporaryConnectorNodes.ForEach(connectorNodes.Add);
            }

            for (var i = 0; i < edges.Count; i++)
            {
                var edge = edges[i];
                var endnode = connectorNodes.Find(item => item.Connetorid == edge.End.Id);
                var startnode = connectorNodes.Find(item => item.Connetorid == edge.Start.Id);
                var blossomEdge = new BlossomEdge
                {
                    OldEdge = edge,
                    Weight = edge.Weight,
                    Start = startnode,
                    End = endnode
                };
                transformedEdges.Add(blossomEdge);
            }
            return transformedEdges;
        }


        private static void Expand(int amountOfNodes, MyList<Edge> edges)
        {
            var transformedGraph = new List<Edge>();
            var nodeCounter = amountOfNodes * 2;
            Console.WriteLine(amountOfNodes);
            edges.ForEach(Console.WriteLine);

            for (var i = 0; i < edges.Size(); i++)
            {
//
            }
            Console.WriteLine(transformedGraph.Count);
            //TransformedGraph.ForEach(Console.WriteLine);
        }
    }
}