using System;
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
                    UserFiles.CeesJan);
            const char delimiter = ',';
            var splittedLines = lines.Select(line => line.Split(delimiter)).ToList();
            foreach (var line in splittedLines)
            {
                if (line[0].Equals("") || line[1].Equals("") || line[2].Equals("")) continue;
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

            _edges.ForEach(Console.WriteLine);
            var blossomEdges = BlossomAlgorithm(new MyList<Edge>(_edges));
            blossomEdges.ForEach(Console.WriteLine);
            Console.WriteLine("***********************************");
            var resultsMaxWeightMatching = MaxWeightMatching.MaxWMatching(new MyList<BlossomEdge>(blossomEdges));
            foreach (var node in resultsMaxWeightMatching)
            {
                Console.WriteLine(node);
            }
            Console.WriteLine("************************************************************************");
            var shrinked = ShrinkBack(resultsMaxWeightMatching, blossomEdges);
            foreach (var variable in shrinked)
            {
                Console.WriteLine(variable);
            }
        }

        private static IEnumerable<Edge> ShrinkBack(List<Node> results, List<BlossomEdge> edges)
        {
            var listtoreturn =
            (from edge in
                results.Select(
                    (node, iterator) =>
                        edges.Find(
                            item =>
                                (item.Start.Id == iterator && item.End.Id == node.Id) ||
                                (item.Start.Id == node.Id && item.End.Id == iterator)))
                where edge != null
                select edge.OldEdge).ToList();
            var path =
                listtoreturn.GroupBy(j => j).Where(g => g.Count() >= 4).Select(t => t.Key);
            return path;
        }

        private static List<BlossomEdge> BlossomAlgorithm(MyList<Edge> edges)
        {
            Console.WriteLine("&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&");
            var transformedEdges = new List<BlossomEdge>();
            var connectorNodes = new List<ConnectorNode>();
            var nodeIdIterator = 0;

            //keys start at 1
            for (var i = 1; i <= _nodesDict.Count; i++)
            {
                var temporaryConnectorNodes = new List<ConnectorNode>();
                var travelNodes = new List<TravelNode>();
                var currentRealNode = _nodesDict[i];

                var degree = currentRealNode.Degrees;
                if (currentRealNode.Degrees >= 3)
                    degree = 2;
                //Creating travelnodes and connectornodes
                for (var j = 0; j < currentRealNode.Degrees; j++)
                {
                    if (j < 2)
                    {
                        //var travelNode = new TravelNode(currentNode.Id + j + "", currentNode.Id * 100 + j);
                        var travelNode = new TravelNode(currentRealNode.Id + j + "", nodeIdIterator++);
                        travelNodes.Add(travelNode);
                    }
                    var nodeTo = currentRealNode.getEdges()[j].End.Id == currentRealNode.Id
                        ? currentRealNode.getEdges()[j].Start.Id
                        : currentRealNode.getEdges()[j].End.Id;
                    //var connectorNode = new ConnectorNode(currentNode.Id + "_" + j + "", nodeTo* 1000 + j , nodeTo);
                    var connectorNode = new ConnectorNode(currentRealNode.Id + "_" + j + "", nodeIdIterator++, nodeTo, i);
                    connectorNode.OldNode = currentRealNode;

                    temporaryConnectorNodes.Add(connectorNode);
                }

                //connecting the travelnodes
                foreach (var travelNode in travelNodes)
                {
                    for (var j = 0; j < temporaryConnectorNodes.Count; j++)
                    {
                        var connectorNode = _nodesDict[temporaryConnectorNodes[j].ConnectorId];
                        var connectorNodeEdges = connectorNode.getEdges();

                        var edgeBetweenConnectorNodes =
                            connectorNodeEdges.Find(
                                edge => edge.Start.Id == currentRealNode.Id || edge.End.Id == currentRealNode.Id);

                        var blossomEdge = new BlossomEdge
                        {
                            Start = travelNode,
                            End = temporaryConnectorNodes[j],
                            Weight = edgeBetweenConnectorNodes.Weight,
                            OldEdge = edgeBetweenConnectorNodes
                        };
                        transformedEdges.Add(blossomEdge);
                    }
                }
                temporaryConnectorNodes.ForEach(connectorNodes.Add);
            }

            //Connecting the connectorNodes (badum tss)
            for (int i = 0; i < connectorNodes.Count; i++)
            {
                for (int j = 0; j < connectorNodes.Count; j++)
                {
                    if (connectorNodes[i].ConnectorId == connectorNodes[j].CurrentNode &&
                        connectorNodes[j].ConnectorId == connectorNodes[i].CurrentNode)
                    {
                        var startEdges = connectorNodes[i].OldNode.getEdges();
                        var endEdges = connectorNodes[j].OldNode.getEdges();

                        var edge = FindNotConnectedEdge(startEdges, endEdges);

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


        private static Edge FindNotConnectedEdge(List<Edge> edgesNodeA, List<Edge> edgesNodeB)
        {
            var returnedge = new Edge();

            foreach (var edgeNodeA in edgesNodeA)
            {
                foreach (var edgeNodeB in edgesNodeB)
                {
                    if (edgeNodeA.Start == edgeNodeB.Start && edgeNodeA.End == edgeNodeB.End)
                        returnedge = edgeNodeA;
                }
            }

            if (copiedEdges.Contains(returnedge))
                returnedge = null;

            copiedEdges.Add(returnedge);
            return returnedge;
        }
    }
}