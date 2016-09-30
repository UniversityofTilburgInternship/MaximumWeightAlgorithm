using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MaximumWeightAlgorithm
{
    public class MaxWeightMatching
    {
        private static int _amountOfEdges;
        private static int _amountOfNodes;
        private static int _maxWeight;
        private static List<int> _endpoint = new List<int>();
        private static List<List<int>> _neightbend = new List<List<int>>();
        private static List<int> _mate = new List<int>();
        private static List<int> _label = new List<int>();
        private static List<int> _labelend;
        private static List<int> _inblossom;
        private static List<int> _blossomparent;
        private static List<List<int>> _blossomchilds;
        private static List<int> _blossombase;
        private static List<List<int>> _blossomdps;
        private static List<int> _bestedge;
        private static List<List<int>> _blossombestedges;
        private static List<int> _unusedblossoms;
        private static List<int> _dualvar;
        private static List<bool> _allowedge;
        private static List<int> _queue;

        public static void MaxWMatching(List<Edge> edges)
        {
            // if edges is empty
            if (edges.Count <= 0)
                return;

            // Count nodes
            _amountOfEdges = edges.Count;
            _amountOfNodes = 0;

            foreach (var edge in edges)
            {
                if (!(edge[0] >= 0 && edge[1] >= 0 && edge[0] != edge[1]))
                    throw new Exception("Assert Error");
                if (edge[0] >= _amountOfNodes) _amountOfNodes = edge[0] + 1;
                if (edge[1] >= _amountOfNodes) _amountOfNodes = edge[1] + 1;
            }
            Console.WriteLine("Applying Edmonds Algorithm with {0} nodes transformed by Siloach reducer\n",
                _amountOfNodes);

            for (var i = 0; i < _amountOfEdges; i++)
            {
                _maxWeight = Math.Max(0, Math.Max(_maxWeight, edges[i][2]));
            }

            for (var i = 0; i < 2 * _amountOfEdges; i++)
            {
                _endpoint.Add(edges[DivideAndFloor(i, 2)][i % 2]);
            }

            for (var i = 0; i < _amountOfNodes; i++)
            {
                _neightbend.Add(new List<int>());
            }

            for (var k = 0; k < _amountOfEdges; k++)
            {
                var i = edges[k][0];
                var j = edges[k][1];

                _neightbend[i].Add(2 * k + 1);
                _neightbend[j].Add(2 * k);
            }

            for (var i = 0; i < _amountOfNodes; i++)
            {
                _mate.Add(-1);
                _inblossom.Add(i);
                _blossombase.Add(i);
                _bestedge.Add(-1);
                _blossomdps.Add(new List<int>());
                _dualvar.Add(_maxWeight);
            }

            for (var i = 0; i < 2 * _amountOfNodes; i++)
            {
                _label.Add(0);
                _labelend.Add(-1);
                _blossomparent.Add(-1);
                _blossomchilds.Add(new List<int>());
                _blossombestedges.Add(new List<int>());
            }
            for (var i = 0; i < _amountOfNodes; i++)
            {
                _blossombase.Add(-1);
                _dualvar.Add(0);
            }
            for (var i = _amountOfNodes; i < 2 * _amountOfNodes; i++)
            {
                _unusedblossoms.Add(i);
            }
            for (var i = 0; i < _amountOfEdges; i++)
            {
                _allowedge.Add(false);
            }

            


//            foreach (var edge in edges)
//            {
//                if (edge.Start >= nodesLenght)
//                    nodesLenght = edge.Start + 1;
//                if (edge.End >= nodesLenght)
//                    nodesLenght = edge.End + 1;
//            }
//
//            // Find maximum weight
//            var maxWeight = Math.Max(0, edges.Max(edge => edge.Weight));
//
//            //
//            var endpoints = new List<int>();
//            for (var i = 0; i < _amountOfEdges; i++)
//            {
//                endpoints.Add(edges[i].Start);
//                endpoints.Add(edges[i].End);
//            }
//


//            // Neighbend[Node] is the list of nodes of the edges to node
//            Console.WriteLine(nodesLenght);
//            var neighbend = CreateList<List<int>>(nodesLenght);
//            for (var i = 0; i < neighbend.Count; i++)
//                neighbend[i] = new List<int>();
//
//            for (var i = 0; i < _amountOfEdges; i++)
//            {
//                var edge = edges[i];
//                neighbend[edge.Start].Add(2 * i + 1);
//                neighbend[edge.End].Add(2 * i);
//            }
//
//            // mate[v] is the remote endpoint of its matched edge, or -1 if it is single
//            // (i.e. endpoint[mate[v]] is v's partner vertex).
//            var mate = new List<int>();
//            for (var i = 0; i < nodesLenght; i++)
//            {
//                mate.Add(-1);
//            }
        }

        private static int DivideAndFloor(int i, int j)
        {
            return (int) Math.Floor((float) i / j);
        }

        private static List<T> CreateList<T>(int capacity)
        {
                return Enumerable.Repeat(default(T), capacity).ToList();
        }
    }
}