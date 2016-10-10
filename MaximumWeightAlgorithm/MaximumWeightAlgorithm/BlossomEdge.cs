﻿namespace MaximumWeightAlgorithm
{
    public class BlossomEdge : Edge
    {
        public Edge OldEdge { get; set; }

        public override string ToString()
        {
            return "BlossomEdge "  + "(" + Start.Id + "," + End.Id + ")\t" + Weight  + "\t";
        }
    }
}
