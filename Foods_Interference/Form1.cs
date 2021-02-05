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

        public int FindIndex(string Food)
        {
            foreach (Node item in V)
            {
                if (item.Food == Food)
                {
                    return item.number;
                }
            }
            return -1;
        }

        public void Foods()
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

        public void Effects()
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

        public void Ingredients()
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
                // List<Node> V = new List<Node>();
                Foods(V);
                int NumberOfFoods = V.Count();

                /*Handling Food-Interference File*/
                // List<List<string>> Adjacents = new List<List<string>>();
                List<string> temp = new List<string>();
                for (int i = 0; i < NumberOfFoods; i++)
                {
                    temp.Add(null);
                }
                for (int i = 0; i < NumberOfFoods; i++)
                {
                    Adjacents.Add(temp);
                }
                Effects(V, Adjacents);

                /*Handling Ingredient-Price File*/
                // Dictionary<string, int> HashTable = new Dictionary<string, int>();
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

        /*Commands Functions*/

        public int GetBill(string food)
        {
            int32 Sum = 0;
            foreach (Node item in V)
            {
                if (V.Food == food)
                {
                    foreach (string item in V.Ingredients)
                    {
                        if(HashTable.ContainsKey(item))
                        {
                            Sum += HashTable[item];
                        }
                        else
                        {
                            MessageBox.Show("Error 101: No Ingredient found!");
                        }
                    }
                    return Sum;
                }
            }

            return -1;
        }

        public string IsEffect(string food1, string food2)
        {
            int i = FindIndex(V, food1);
            int j = FindIndex(V, food2);
            if (i == -1 || j == -1)
            {
                //NFD: No Food in the Database
                return "NFD";
            }
            if (Adjacents[i][j] != null)
            {
                return Adjacents[i][j];
            }
            return "No";
        }

        public bool IngredientExist(List<string> Ingredients)
        {
            foreach (string item in Ingredients)
            {
                if (HashTable.ContainsKey(item) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public int NewFood(string food, List<string> ingredients)
        {
            if (IngredientExist(ingredients) == true)
            {
                if (FindIndex(V, food) == -1)
                {
                    Node newNode = new Node();
                    NumberOfFoods++;
                    newNode.number = NumberOfFoods;
                    newNode.Food = food;
                    newNode.Ingredients = ingredients;
                    List<string> temp = new List<string>();
                    for (int i = 0; i < NumberOfFoods; i++)
                    {
                        temp.Add(null);
                    }
                    Adjacents.Add(temp);
                    return 0;
                }
                else
                {
                    //The food is already in the database
                    return -1;
                }
            }

            //Do not have necessary ingredients!
            retunr -2;
        }

        public bool NewIngredient(string Ingredient, int price)
        {
            if (HashTable.ContainsKey(name) == false)
            {
                HashTable.Add(Ingredient, price);
                //Successful
                return True;
            }
            
            //The ingredient is already in the database!
            return False;
        }

        public bool NewEffect(string food1, string food2, string effect)
        {
            int i = FindIndex(V, food1);
            int j = FindIndex(V, food2);
            if ( i == -1 || j == -1)
            {
                //Food {food_name} is not in the database!
                return false;
            }

            Adjacents[i][j] = effect;
            Adjacents[j][i] = effect;
            //Successful
            return true;
        }

        public void Delete(string food)
        {
            int16 i = FindIndex(food);
            if (i != -1)
            {
                V[i] = null;
                V.Ingredients.Clear();
                
                for (int j = 0; j < Adjacents.Count(); j++)
                {
                    if (Adjacents[i][j] != null)
                    {
                        Adjacents[i][j] = null;
                        Adjacents[j][i] = null;
                    }
                }
            }
            else
            {
                MessageBox.Show($"Food {food} is not in the database!");
            }
        }

        public void Delete(string food1, string food2)
        {
            int16 i = FindIndex(food1);
            int16 j = FindIndex(food2);
            if (i == -1 || j == -1)
            {
                MessageBox.Show("Input is not in the database!");
            }
            else
            {
                if (Adjacents[i][j] == null)
                {
                    MessageBox.Show($"There is no food interference for {food1} and {food2}");
                }
                else
                {
                    Adjacents[i][j] = null;
                    Adjacents[j][i] = null;
                }
            }
        }
    }
}
