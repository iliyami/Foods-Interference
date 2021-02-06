using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods_Interference
{
    public struct Key
    {
        public string key1;
        public string key2;
    }
    public class Graph
    {
        public int Size;
        public List<Node> V = new List<Node>();
        public Dictionary<Key, string> Adjacents = new Dictionary<Key, string>();
        public Dictionary<string, int> HashTable = new Dictionary<string, int>();
    }
}
