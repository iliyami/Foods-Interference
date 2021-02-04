using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Foods_Interference
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool CheckFiles()
        {
            if (!File.Exists("E:\\effects.txt"))
            {
                MessageBox.Show("No effects.txt found!");
                return false;

            }
            if (!File.Exists("E:\\ingredients.txt"))
            {
                MessageBox.Show("No ingredients.txt found!");
                return false;
            }
            if (!File.Exists("E:\\foods.txt"))
            {
                MessageBox.Show("No ingredients.txt found!");
                return false;
            }

            return true;
        }

        public int FindIndex(List<Node> V, string Food)
        {
            foreach (Node item in V)
            {
                if (item.Food == Food)
                {
                    return item.number;
                }
            }
            Console.WriteLine("Error1 - No Vertex with given input found!");
            return -1;
        }

        public void Foods(List<Node> V)
        {
            StreamReader FI_File = new StreamReader("E:\\foods.txt");
            string Line;
            int counter = 0;
            while (FI_File.Peek() >= 0)
            {
                Line = FI_File.ReadLine().ToString();
                string[] strArray;
                strArray = Line.split(',');
                Node newNode = new Node();
                newNode.Food = strArray[0];
                foreach (string item in strArray)
                {
                    newNode.Ingredients = item;
                }
                newNode.number = counter;
                counter++;
                V.add(newNode);
            }
            FI_File.Close();
        }

        public void Effects(List<Node> V, List<List<string>> Adjacents)
        {
            StreamReader FFE_File = new StreamReader("yechizi");
            string Line;
            while (FFE_File.Peek() >= 0)
            {
                Line = FFE_File.ReadLine().ToString();
                string strArray = Line.split(',');
                int i = FindIndex(V, strArray[0]);
                int j = FindIndex(V, strArray[1]);
                if (i == -1 || j == -1)
                {
                    Console.WriteLine("Error From Indices i j - equal to -1");
                    Close();
                }
                else
                {
                    Adjacents[i][j] = strArray[2];
                    Adjacents[j][i] = strArray[2];
                }
            }
            FFE_File.Close();
        }

        public void Ingredients(Dictionary<string, int> HashTable)
        {
            StreamReader IP_File = new StreamReader("Address");
            string strArray;
            while(IP_File.Peek() >= 0)
            {
                strArray = IP_File.ReadLine().ToString().split(',');
                HashTable.Add(strArray[0], Int32.Parse(strArray[1]));
            }
            IP_File.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (CheckFiles() == false)
            {
                Close();
            }
            else
            {
                /*Handling Food-Ingredients File*/
                List<Node> V = new List<Node>();
                Foods(V);
                int NumeberOfFoods = V.lenght();
                /*Handling Food-Interference File*/
                List<List<string>> Adjacents = new List<List<string>>();
                Effects(V, Adjacents);

                /*Handling Ingredient-Price File*/
                Dictionary<string, int> HashTable = new Dictionary<string, int>();
                Ingredients(HashTable);
            }
        }

        // public List<string> DFS (List<Node> V, string food, int i = 0)
        // {
        //     visited[v] = true;
            
        //     foreach(var n in vList)
        //     {
        //         if (!visited[n])
        //             DFS(n, visited);
        //     }
        // }

        public int32 CalculatePrices(List<string> Ingredients, Dictionary<string, int> HashTable)
        {
            Int32 Sum;
            foreach (string item in Ingredients)
            {
                if(HashTable.ContainsKey(item))
                {
                    Sum += HashTable[item];
                }
                else
                {
                    MessageBox.Show("Error Dictionary202");
                }
            }
            return Sum;
        }

        public int32 GetBill(List<Node> V ,string food, int quantity, Dictionary<string, int> HashTable)
        {
            int32 Sum = -1;
            foreach (Node item in V)
            {
                if (V.Food == food)
                {
                    Sum = CalculatePrices(V.Ingredients, HashTable);
                }
            }

            if (Sum == -1)
            {
                MessageBox.Show("No Food found with this name in the database");
            }
            else
            {
                return Sum;
            }
        }
    }
}
