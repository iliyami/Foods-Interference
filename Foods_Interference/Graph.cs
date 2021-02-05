using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods_Interference
{

    public static class Graph
    {
        List<Node> V = new List<Node>();
        List<List<string>> Adjacents = new List<List<string>>();
        Dictionary<string, int> HashTable = new Dictionary<string, int>();
    }
}
