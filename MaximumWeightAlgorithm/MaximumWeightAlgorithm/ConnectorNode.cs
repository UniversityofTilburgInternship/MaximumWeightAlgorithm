using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class ConnectorNode : Node
    {
        public int ConnectorId { get; set; }
        public int CurrentNode { get; set; }
        public Node OldRealNode { get; set; }

        public ConnectorNode(string name, int id, int connetorid, int currentNode) : base(name, id)
        {
            ConnectorId = connetorid;
            CurrentNode = currentNode;
        }

        public override string ToString()
        {
            var neighboursString = Neighbours.Aggregate("", (current, n) => current + (n.Id + ", "));
            return Id + "\t"  + neighboursString + "\t\t" + ConnectorId + "ci from cn" + CurrentNode;
        }
    }
}