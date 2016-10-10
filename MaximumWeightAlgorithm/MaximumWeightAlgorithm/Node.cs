using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class Node
    {
        private List<Node> _neighbours;
        private List<Edge> _edges;
        public Node(string name, int id)
        {
            _neighbours = new List<Node>();
            _edges = new List<Edge>();
            Name = name;
            Id = id;
        }

        public Node()
        {
            _edges = new List<Edge>();
            _neighbours = new List<Node>();
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public int Degrees => _neighbours.Count;

        public List<Node> GetNeighbours()
        {
            return _neighbours;
        }

        public void AddEdge(Edge edge)
        {
            _edges.Add(edge);
            this.AddNeighbour(edge.Start.Id != Id ? edge.Start : edge.End);
        }

        public List<Edge> getEdges()
        {
            return _edges;
        }

        public void AddNeighbour(Node neighbour)
        {
            _neighbours.Add(neighbour);
        }
        public override string ToString()
        {
            var neighboursString = _neighbours.Aggregate("", (current, n) => current + (n.Id + ", "));
            return Id + "\t"  + neighboursString;
        }

    }
}