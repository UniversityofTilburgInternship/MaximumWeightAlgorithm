using System.Collections.Generic;
using System.Linq;

namespace MaximumWeightAlgorithm
{
    public class MyList<T> : List<T>
    {
        public List<T> List;

        public MyList(int size)
        {
            List = CreateList<T>(size);
        }

        public MyList(List<T> oldlist)
        {
            List = oldlist;
        }

        public MyList()
        {
            List = new List<T>();
        }

        public int Size()
        {
            return List.Count;
        }

        public new void Add(T elem)
        {
            List.Add(elem);
        }

        public void RemoveLast()
        {
            List.RemoveAt(List.Count - 1);
        }

        public new void Reverse()
        {
            List.Reverse();
        }

        public new int Count => List.Count;

        public List<T> ToList()
        {
            return List;
        }

        private static List<T> CreateList<T>(int capacity)
        {
            return Enumerable.Repeat(default(T), capacity).ToList();
        }


        public T this[int i]
        {
            get { return List[i]; }
            set { List[i] = value; }
        }

        public override string ToString()
        {
            return List.Aggregate("", (current, variable) => current + (variable + "\t"));
        }
    }
}