﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foods_Interference
{

    public class Node
    {
        public string Food { get; set; }
        public int number = -1;
        public List<string> Ingredients = new List<string>();
    }
}
