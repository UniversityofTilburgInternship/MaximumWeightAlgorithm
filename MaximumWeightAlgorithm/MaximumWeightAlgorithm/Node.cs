using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class Node
    {
        string _name;
        private List<Edge> _edges = new List<Edge>();
        private List<Node> _neighbours;
        bool flag;
        Edge first;
        Edge second;
        private int _sum;
        private bool IsRepeated;

        //Name(V_Name), flag(0), sum(0), first(NULL), second(NULL), isRepeated(false)
        public Node(string name)
        {
            _name = name;
            flag = false;
            IsRepeated = false;
            _neighbours = new List<Node>();
        }

        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
            var weight = edge.GetWeight();
            if (first == null || weight > first.GetWeight())
            {
                second = first;
                first = edge;
            }
            else if (this.second == null || weight > second.GetWeight())
            {
                second = edge;
            }
            _neighbours.Add(edge.GetOther(this));
            _sum += weight;
        }

        public int GetEdgesSize()
        {
            return _edges.Count;
        }

        public string GetName()
        {
            return _name;
        }

        public int GetWeightSum()
        {
            return _sum;
        }

        public int GetFirstWeight()
        {
            return first.GetWeight();
        }

        public int GetSecondWeight()
        {
            return second.GetWeight();
        }

        public List<Edge> GetEdges()
        {
            return _edges;
        }

        private Edge GetThirdEdge()
        {
            if (_edges.Count < 3)
                return null;
            var thirdEdge = _edges.First();
            var index = 0;
            while (_edges[index] == first || _edges[index] == second)
                index++;

            thirdEdge = _edges[index];

            for (var i = 0; i < _edges.Count; i++)
            {
                var edge = _edges[i];
                if (edge != first && edge != second && edge.GetWeight() > thirdEdge.GetWeight())
                    thirdEdge = edge;
            }

            return thirdEdge;
        }

        public void RemoveEdge(Edge e)
        {
            if (first != null && first == e)
            {
                var third = GetThirdEdge();
                first = second;
                second = third;
            }
            else if (second != null && second == e)
            {
                second = GetThirdEdge();
            }
            var comparison = e.GetOther(this);
            _edges.RemoveAll(x => x == e);
            _neighbours.RemoveAll(x => x == comparison);
            _sum -= e.GetWeight();
        }

        public bool Repeated()
        {
            return IsRepeated;
        }

        public void SetRepeated(bool value)
        {
            IsRepeated = value;
        }
    }
}