using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class Node
    {
        public List<Node> Neighbours;
        public GenericVector PersonalityValues;
        private List<Edge> _edges;
        public Node(string name, int id)
        {
            Neighbours = new List<Node>();
            _edges = new List<Edge>();
            Name = name;
            Id = id;
        }

        public Node()
        {
            _edges = new List<Edge>();
            Neighbours = new List<Node>();
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public int Degrees => Neighbours.Count;

        public List<Node> GetNeighbours()
        {
            return Neighbours;
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
            Neighbours.Add(neighbour);
        }
        public override string ToString()
        {
            var neighboursString = Neighbours.Aggregate("", (current, n) => current + (n.Id + ", "));
            return Id + "\t"  + neighboursString;
        }

    }
}