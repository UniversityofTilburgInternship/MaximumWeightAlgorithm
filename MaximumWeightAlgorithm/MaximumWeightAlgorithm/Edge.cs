using System;

namespace MaximumWeightAlgorithm
{
    public class Edge
    {
        public Node Start { get; set; }

        public float Weight { get; set; }

        public Node End { get; set; }

        public Edge()
        {
        }

        public Edge(Node start, Node end, float weight)
        {
            if (start.Id == end.Id)
            {
                Start = start;
                End = end;
                Weight = weight;
            }
            if (start.Id < end.Id)
            {
                Start = start;
                End = end;
            }
            else
            {
                Start = end;
                End = start;
            }
            Weight = weight;
        }


        public Node this [int i]
        {
            get
            {
                if (i == 0)
                    return Start;
                if (i == 1)
                    return End;
                throw new Exception("index out of Range");
            }
        }

        public override string ToString()
        {
            return "(" + Start + "," + End + ")\t" + Weight ;
        }
    }
}