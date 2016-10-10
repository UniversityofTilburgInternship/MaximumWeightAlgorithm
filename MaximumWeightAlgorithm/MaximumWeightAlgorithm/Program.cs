using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    internal class Program
    {
        private static int _oldNodesCount = 0;
        static Dictionary<int, Node> _nodesDict = new Dictionary<int, Node>();
        static List<Edge> _edges = new List<Edge>();
        static HashSet<Edge> copiedEdges = new HashSet<Edge>();

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
                if (endNodeInt < startNodeInt)
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


            //_edges.ForEach(Console.WriteLine);
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
            int x = 0;
            //keys start at 1
            for (int i = 1; i <= _nodesDict.Count; i++)
            {
                var temporaryConnectorNodes = new List<ConnectorNode>();
                var travelNodes = new List<TravelNode>();
                var currentNode = _nodesDict[i];

                for (var j = 0; j < currentNode.Degrees; j++)
                {
                    //var travelNode = new TravelNode(currentNode.Id + j + "", currentNode.Id * 100 + j);
                    var travelNode = new TravelNode(currentNode.Id + j + "", x++);

                    var nodeTo = currentNode.getEdges()[j].End.Id == currentNode.Id
                        ? currentNode.getEdges()[j].Start.Id
                        : currentNode.getEdges()[j].End.Id;
                    //var connectorNode = new ConnectorNode(currentNode.Id + "_" + j + "", nodeTo* 1000 + j , nodeTo);
                    var connectorNode = new ConnectorNode(currentNode.Id + "_" + j + "", x++, nodeTo, i);
                    connectorNode.OldNode = currentNode;

                    travelNodes.Add(travelNode);
                    temporaryConnectorNodes.Add(connectorNode);
                }

                foreach (var travelNode in travelNodes)
                {
                    for (var j = 0; j < temporaryConnectorNodes.Count; j++)
                    {
                        var connectorNode = _nodesDict[temporaryConnectorNodes[j].Connetorid];
                        var connectorNodeEdges = connectorNode.getEdges();
                        var edgeBetweenNodes =
                            connectorNodeEdges.Find(
                                edge => edge.Start.Id == currentNode.Id || edge.End.Id == currentNode.Id);
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
            for (int i = 0; i < connectorNodes.Count; i++)
            {
                for (int j = 0; j < connectorNodes.Count; j++)
                {
                    if (connectorNodes[i].Connetorid == connectorNodes[j].CurrentNode &&
                        connectorNodes[j].Connetorid == connectorNodes[i].CurrentNode)
                    {

                        var startedges = connectorNodes[i].OldNode.getEdges();
                        var endedges = connectorNodes[j].OldNode.getEdges();
                        var edge = FindNotConnectedEdge(startedges, endedges);
                        if (edge != null)
                        {
                            var blossomEdge = new BlossomEdge
                            {
                                Start = connectorNodes[i],
                                End = connectorNodes[j],
                                OldEdge = edge,
                                Weight = edge.Weight
                            };
                            transformedEdges.Add(blossomEdge);
                        }
                    }
                }
            }
            return transformedEdges;
        }

        private static Edge FindNotConnectedEdge(List<Edge> node1edges, List<Edge> node2edges)
        {
            var returnedge = new Edge();
            for (int i = 0; i < node1edges.Count; i++)
            {
                for (int j = 0; j < node2edges.Count; j++)
                {
                    if (node1edges[i].Start == node2edges[j].Start && node1edges[i].End == node2edges[j].End)
                        returnedge =  node1edges[i];

                }
            }
            if (copiedEdges.Contains(returnedge))
                returnedge = null;
            copiedEdges.Add(returnedge);
            return returnedge;
        }
    }
}