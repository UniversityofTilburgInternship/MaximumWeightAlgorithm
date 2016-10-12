using System.Collections.Generic;
using System.Linq;
using Casanova.Prelude;

public class HelperFunctions
{
    public static Dictionary<T, K> TupleListToDictionary<T, K>(List<Tuple<T, K>> tupleList)
    {
        return tupleList.ToDictionary(tuple => tuple.Item1, tuple => tuple.Item2);
    }

    public static List<Tuple<T, K>> DictionaryToTupleList<T, K>(Dictionary<T, K> dictionary)
    {
        return dictionary.Select(keyValue => new Tuple<T, K>(keyValue.Key, keyValue.Value)).ToList();
    }

    public static GenericVector DictionaryToGenericVector(Dictionary<int, int> dictionary)
    {
        var newGenericVector = new GenericVector();
        foreach (var keyValue in dictionary)
        {
            newGenericVector.Add(keyValue.Value);
        }
        return newGenericVector;
    }

    
}
                             