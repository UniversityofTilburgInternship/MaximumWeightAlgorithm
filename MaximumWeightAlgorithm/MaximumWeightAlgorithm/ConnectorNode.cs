using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class ConnectorNode : Node
    {
        public int Connetorid { get; set; }
        public int CurrentNode { get; set; }
        public Node OldNode { get; set; }

        public ConnectorNode(string name, int id, int connetorid, int currentNode) : base(name, id)
        {
            Connetorid = connetorid;
            CurrentNode = currentNode;

        }

        public override string ToString()
        {
            var neighboursString = Neighbours.Aggregate("", (current, n) => current + (n.Id + ", "));
            return Id + "\t"  + neighboursString + "\t\t" + Connetorid + "ci from cn" + CurrentNode;
        }
    }
}