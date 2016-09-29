using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class Edge
    {
        private List<Node> _connectedNodes;
        private int _weight;
        public bool VisitedFlag;

        public Edge(Node start, Node end, int weight)
        {
            _weight = weight;
            VisitedFlag = false;
            _connectedNodes = new List<Node> {start, end};
            start.AddEdge(this);
            end.AddEdge(this);
        }

        public List<Node> GetNodes()
        {
            return _connectedNodes;
        }

        public int GetWeight()
        {
            return _weight;
        }

        public Node GetOther(Node node)
        {
            return _connectedNodes[0] == node ? _connectedNodes.Last() : _connectedNodes.First();
        }

        public void DestroyEdge()
        {
            Node node = _connectedNodes.First();
            
        }
    }
}