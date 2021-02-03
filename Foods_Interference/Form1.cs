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

        private void Form1_Load(object sender, EventArgs e)
        {
            if (CheckFiles() == false)
            {
                Close();
            }
            else
            {
                StreamReader ReadFoods = new StreamReader("E:\\foods.txt");
                string Line;
                while (ReadFoods.Peek() >= 0)
                {
                    Line = ReadFoods.ReadLine().ToString();
                    bool reachFood = false;
                    string word = "";
                    Node newNode = new Node();
                    foreach (char character in Line)
                    {
                        if (character != ',')
                        {
                            word += character;
                        }
                        else
                        {
                            if (reachFood == false)
                            {
                                reachFood = true;
                                newNode.Food = word;
                            }
                            else
                            {
                                newNode.Ingredients.Add(word);
                            }
                            word = "";
                        }
                        if(character == ',' && reachFood == false)
                        {
                            reachFood = true;
                        }
                    }

                }
                ReadFoods.Close();
            }
        }
    }
}
