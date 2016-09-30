namespace MaximumWeightAlgorithm
{
    public class Edge
    {
        public Edge(int start, int end, double weight)
        {
            Start = start;
            End = end;
            Weight = weight;
        }

        public int Start { get; set; }

        public double Weight { get; set; }

        public int End { get; set; }
    }
}