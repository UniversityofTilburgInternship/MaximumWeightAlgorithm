using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters;

namespace MaximumWeightAlgorithm
{
    public class Edge
    {
        private List<int> _elems;

        public Edge(int start, int end, int weight)
        {
            _elems = new List<int>();
            if (start == end)
            {
                _elems.Insert(0, start);
                _elems.Insert(1, end);
                _elems.Insert(2, weight);
            }
            if (start < end)
            {
                _elems.Insert(0, start);
                _elems.Insert(1, end);
            }
            else
            {
                _elems.Insert(0, end);
                _elems.Insert(1, start);
            }
            _elems.Insert(2, weight);

        }

        public int this [int i] => _elems[i];

        public override string ToString()
        {
            var result = "";
            result += _elems[0] + "\t" + _elems[1] + "\t" + _elems[2] + "\n ";
            return result;
        }

        //equivalent (start , end)
        //equivalent (start, end , weight)
    }
}