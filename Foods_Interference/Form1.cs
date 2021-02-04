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
                    for (int i = 0; i < Line.Lenght(); i++)
                    {
                        if (Line[i] != ',')
                        {
                            word += Line[i];
                            if (i+1 == Line.Lenght())
                            {
                                newNode.Ingredients.Add(word);
                            }
                        }
                        else
                        {
                            if (word[0] == 'F')
                            {
                                newNode.Food = word;
                            }
                            else
                            {
                                newNode.Ingredients.Add(word);
                            }
                            word = "";
                            newNode.number = counter;
                            counter++;
                        }
                    }
                    V.add(newNode);
                }
                FI_File.Close();

                /*Handling Food-Interference File*/
                List<List<string>> Adjacents = new List<List<string>>();
                StreamReader FFE_File = new StreamReader("yechizi");
                string Line;
                while (FFE_File.Peek() >= 0)
                {
                    Line = FFE_File.ReadLine().ToString();
                    string word = "";
                    int i = -1, j = -1;
                    for (int i = 0; i < Line.Lenght(); i++)
                    {
                        if (Line[i] != ',')
                        {
                            word += Line[i];
                            if (i+1 == Line.Lenght())
                            {
                                if (i == -1 || j == -1)
                                {
                                    Console.WriteLine("Error From Index i j");
                                    Close();
                                }
                                Adjacents[i][j] = word;
                                Adjacents[j][i] = word;
                            }
                        }
                        else
                        {
                            if (word[0] == 'F')
                            {
                                if (i == -1)
                                {
                                    i = FindIndex(V, word);
                                }
                                else
                                {
                                    j = FindIndex(V, word);
                                }
                            }
                            word = "";
                        }
                    }                   
                }
                FFE_File.Close();

            }
        }
    }
}
