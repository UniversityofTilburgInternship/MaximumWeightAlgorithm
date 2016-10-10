using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class Node
    {
        private List<Node> _neighbours;
        public Node(string name, int id)
        {
            _neighbours = new List<Node>();
            Name = name;
            Id = id;
        }

        public Node()
        {
            _neighbours = new List<Node>();
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public int Degrees => _neighbours.Count;

        public List<Node> GetNeighbours()
        {
            return _neighbours;
        }

        public void AddNeighbour(Node neighbour)
        {
            _neighbours.Add(neighbour);
        }
        public override string ToString()
        {
            var neighboursString = _neighbours.Aggregate("", (current, n) => current + (n.Id + ",\t"));
            return Id + "\t"  + neighboursString;
        }

    }
}