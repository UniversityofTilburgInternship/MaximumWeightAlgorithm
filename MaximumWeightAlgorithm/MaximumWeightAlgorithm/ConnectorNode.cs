namespace MaximumWeightAlgorithm
{
    public class ConnectorNode : Node
    {
        public int Connetorid { get; set; }

        public ConnectorNode(string name, int id, int connetorid) : base(name, id)
        {
            Connetorid = connetorid;

        }
    }
}