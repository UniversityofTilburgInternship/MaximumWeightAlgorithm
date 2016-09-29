using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Security.Cryptography;

namespace MaximumWeightAlgorithm
{
    public class Node
    {
        string Name;
        List<Edge> Edges = new List<Edge>();
        List<Node> Neighbours;
        bool flag;
        Edge first;
        Edge second;
        private int _sum;
        private bool IsRepeated;

        //Name(V_Name), flag(0), sum(0), first(NULL), second(NULL), isRepeated(false)
        public Node(string name)
        {
            Name = name;
            flag = false;
            IsRepeated = false;
            Neighbours = new List<Node>();
        }

        public void AddEdge(Edge edge)
        {
            Edges.Add(edge);
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
            Neighbours.Add(edge.GetOther(this));
            _sum += weight;
        }

        public int GetEdgesSize()
        {
            return Edges.Count;
        }

        public string GetName()
        {
            return Name;
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
            return Edges;
        }

        private Edge GetThirdEdge()
        {
            if (Edges.Count < 3)
                return null;
            Edge thirdEdge = Edges.First();
            var index = 0;
            while (Edges[index] == first || Edges[index] == second)
                index++;

            thirdEdge = Edges[index];

            for (int i = 0; i < Edges.Count; i++)
            {
                var edge = Edges[i];
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
            Edges.RemoveAll(x => x == e);
            Neighbours.RemoveAll(x => x == comparison);
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