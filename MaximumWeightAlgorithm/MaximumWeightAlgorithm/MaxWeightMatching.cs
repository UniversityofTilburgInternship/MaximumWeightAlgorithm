using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MaximumWeightAlgorithm
{
    public class MaxWeightMatching
    {
        public static void MaxWMatching(List<Edge>  edges )
        {


            // if edges is empty
            if (edges.Count<= 0)
                return;

            // Count nodes
            var edgesLenght = edges.Count;
            var nodesLenght = 0;
            foreach (var edge in edges)
            {
                if (edge.Start >= nodesLenght)
                    nodesLenght = edge.Start +1;
                if (edge.End >= nodesLenght)
                    nodesLenght = edge.End + 1;
            }

            // Find maximum weight
            var maxWeight = Math.Max(0, edges.Max(edge => edge.Weight));

            //
            var endpoints = new List<int>();
            for (var i = 0; i < edgesLenght; i++)
            {
                endpoints.Add(edges[i].Start);
                endpoints.Add(edges[i].End);
            }

            foreach (var VARIABLE in endpoints)
            {
                Console.Write(VARIABLE + ", ");
            }
            Console.WriteLine();

//
//            // Neighbend[Node] is the list of nodes of the edges to node
//            var neighbend = new
//            for (var i = 0; i < edgesLenght; i++)
//            {
//                var edge = edges[i];
//
//            }



        }
    }
}