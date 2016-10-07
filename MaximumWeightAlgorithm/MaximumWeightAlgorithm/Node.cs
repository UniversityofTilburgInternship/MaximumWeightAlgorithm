namespace MaximumWeightAlgorithm
{
    public class Node
    {
        public Node(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public Node()
        {
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public override string ToString()
        {
            return  Id.ToString() ;
        }
    }
}