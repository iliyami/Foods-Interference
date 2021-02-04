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
                /*Handling Food-Ingredients File*/
                StreamReader ReadFoods = new StreamReader("E:\\foods.txt");
                string Line;
                int counter = 0;
                while (ReadFoods.Peek() >= 0)
                {
                    Line = ReadFoods.ReadLine().ToString();
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

                    /*Handling Food-Interference File*/
                    StreamReader FI_File = new StreamReader("E:\\foods.txt");


                }
                ReadFoods.Close();
            }
        }
    }
}
