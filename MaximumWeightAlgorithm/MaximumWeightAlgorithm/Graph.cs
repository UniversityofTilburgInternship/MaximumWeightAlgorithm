using System;
using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class Graph
    {
        //properties
        private Dictionary<string, Node> Nodes;
        private int repetition_threshold;
        private List<Node> maybe_repited_node;
        private List<Node> SortedNodes = new List<Node>();


        //map<string, ptr<Vertex>> Vertices = NODES;
//        vector<ptr<Vertex>> verticesSorted = SORTEDNODES;
//        list<weak_ptr<Vertex>> maybe_repited_vertex = maybe_repited_vertex;
//        list<ptr<Scaffold>> scaffolds;
//        vector<ptr<EdmondsEdge>> edmondReduction;
//        vector<ptr<EdmondsEdge>> edmondSelected;
//        int repetition_treshold;
//        void SetToEdmonds();
//        void SetFromEdmonds();
//        void InterpreteEdmonds(vector<pair<int, int>> res);
//        vector<pair<int, int>> ApplyEdmondsMaximumMatching();
        //: Vertices(map<string, ptr<Vertex>>()), repetition_treshold(3), maybe_repited_vertex(list<weak_ptr<Vertex>>())

        //functions
        public Graph()
        {
            //Default repetitionThreshold
            new Graph(3);
        }

        public Graph(int repetitionThreshold)
        {
            Nodes = new Dictionary<string, Node>();
            repetition_threshold = repetitionThreshold;
            maybe_repited_node = new List<Node>();
        }

        public void AddEdge(string start, string end, int weight)
        {
            Node startNode, endNode;
            Node maybeStart;

            if (Nodes.TryGetValue(start, out maybeStart))
                startNode = maybeStart;
            else
            {
                startNode = new Node(start);
                Nodes[start] = startNode;
            }

            Node maybeEnd;
            if (Nodes.TryGetValue(end, out maybeEnd))
                endNode = maybeEnd;
            else
            {
                endNode = new Node(end);
                Nodes[end] = endNode;
            }

            new Edge(startNode, endNode, weight);
            Console.WriteLine(startNode.GetEdgesSize()  + " " +  repetition_threshold);
            if (startNode.GetEdgesSize() == repetition_threshold)
                maybe_repited_node.Add(startNode);
            Console.WriteLine(endNode.GetEdgesSize()  + " " +  repetition_threshold);

            if (endNode.GetEdgesSize() == repetition_threshold)
                maybe_repited_node.Add(endNode);
        }

        public Dictionary<string, Node> GetNodes()
        {
            return Nodes;
        }

        public string RemoveIsolatedNodes()
        {
            var res = "List of isolated nodes \n";
            var isolatedNodesToRemove = new Dictionary<string, Node>();

            foreach (var dictionaryItem in Nodes)
            {
                if (dictionaryItem.Value.GetEdgesSize() <= 0)
                    isolatedNodesToRemove[dictionaryItem.Key] = dictionaryItem.Value;
            }
            foreach (var dictionaryItem in isolatedNodesToRemove)
            {
                if (!dictionaryItem.Value.Repeated())
                    res += dictionaryItem.Value.GetName();

                isolatedNodesToRemove.Remove(dictionaryItem.Key);
            }
            foreach (var dictionaryItem in Nodes)
                SortedNodes.Add(dictionaryItem.Value);

            return res;
        }

        public string RemoveRepetitions(int threshold)
        {
            var returnString = "List of repeated nodes:\n";

            var percentageThreshold = threshold / 100.0;
            var repeatedNodesToRemove = new List<Node>();

            foreach (var node in maybe_repited_node)
            {
                var nodeRepetitionThreshold = node.GetWeightSum() * percentageThreshold;
                if (!(node.GetFirstWeight() < nodeRepetitionThreshold) &&
                    !(node.GetSecondWeight() < nodeRepetitionThreshold)) continue;
                repeatedNodesToRemove.Add(node);
                returnString += node.GetName() + "\t";
                var edges = node.GetEdges();
                returnString = edges.Aggregate(returnString, (current, edge) => current + (edge.GetWeight() + " "));
                returnString += "\n";
            }
            foreach (var node in repeatedNodesToRemove)
            {
                node.SetRepeated(true);
                var edges = node.GetEdges();
                foreach (var edge in edges)
                    edge.DestroyEdge();
            }
            returnString += RemoveIsolatedNodes();
            return returnString;
        }
    }
}