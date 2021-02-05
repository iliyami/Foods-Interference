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
using System.Diagnostics;

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
            while (IP_File.Peek() >= 0)
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
                        if (HashTable.ContainsKey(item))
                        {
                            Sum += HashTable[item];
                        }
                        else
                        {
                            //In baraye debug khodame. bar midaram bade debug
                            MessageBox.Show("Error 101: No Ingredient found!");
                        }
                    }
                    //Successful
                    return Sum;
                }
            }
            //Food {food_name} is not in the database!
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
                //Return The effect name
                return Adjacents[i][j];
            }
            //No Effect
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
                    //Successful
                    return 0;
                }
                else
                {
                    //The food is already in the database
                    return -1;
                }
            }

            //Do not have necessary ingredients!
            return -2;
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
            if (i == -1 || j == -1)
            {
                //Food {food_name} is not in the database!
                return false;
            }

            Adjacents[i][j] = effect;
            Adjacents[j][i] = effect;
            //Successful
            return true;
        }

        public bool Delete(string food)
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
                return True;
            }

            //Food {food_name} is not in the database!
            return False;
        }

        public int Delete(string food1, string food2)
        {
            int16 i = FindIndex(food1);
            int16 j = FindIndex(food2);
            if (i == -1 || j == -1)
            {
                //Input is not in the database!
                return -1;
            }
            else
            {
                if (Adjacents[i][j] == null)
                {
                    //There is no food effect for {food1_name} and {food2_name}
                    return -2;
                }
                else
                {
                    Adjacents[i][j] = null;
                    Adjacents[j][i] = null;
                }
            }
            //Successful
            return 0;
        }








        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        //------------------my funcs(az inja be paiin man zadam)--------------------------
        
        
        public void FullClear()
        {
        total_Price_lbl.Text = "";
        add_btn_f1.Enabled=false;                
        food_name_txt_f1.Enabled=false;
        food_name_txt_f1.Text = "";
        number_of_food_txt_f1.Enabled=false;
        number_of_food_txt_f1.Text = "";
        end_and_Show_Bill_btn_f1.Enabled=false;          
        richTextBox_food_f1.Enabled=false;
        richTextBox_food_f1.Text = "";
        Close_bill_btn_f1.Enabled=false;               
        btn_disable_f2.Enabled=false;               
        richTextBox_efects_f2.Enabled=false;
        richTextBox_efects_f2.Text = "";
        richTextBox_foods_f2.Enabled=false;
        richTextBox_foods_f2.Text = "";
        btn_end_f2.Enabled=false;                    
        btn_add_f2.Enabled=false;                    
        food_name_txt_f2.Enabled=false;
        food_name_txt_f2.Text = "";
        txt_foodname2_f3_2.Enabled=false;
        txt_foodname2_f3_2.Text = "";
        txt_foodname1_f3_2.Enabled=false;
        txt_foodname1_f3_2.Text = "";
        txt_efect_f3_2.Enabled=false;
        txt_efect_f3_2.Text = "";
        btn_add_food_last_f3_1.Enabled=false;             
        btn_add_ingredient_f3_1.Enabled=false;             
        richTextBox_ingredients_f3_1.Enabled=false;
        richTextBox_ingredients_f3_1.Text = "";
        txt_ingredient_f3_1.Enabled=false;
        txt_ingredient_f3_1.Text = "";
        txt_foodname_f3_1.Enabled=false;
        txt_foodname_f3_1.Text = "";
        btn_add_ingredient_last_f3_3.Enabled=false;       
        txt_ingredient_price_f3_3.Enabled=false;
        txt_ingredient_price_f3_3.Text = "";
        txt_ingredient_name_f3_3.Enabled=false;
        txt_ingredient_name_f3_3.Text = "";
        btn_add_efect_last_f3_2.Enabled=false;             
        btn_remove_just_efect_last_f4_2.Enabled=false;   
        txt_food_1_name_f4_2.Enabled=false;
        txt_food_1_name_f4_2.Text = "";
        txt_food_2_name_f4_2.Enabled=false;
        txt_food_2_name_f4_2.Text = "";
        btn_remove_food_last_f4_1.Enabled=false;        
        txt_food_name_f4_1.Enabled=false;
        txt_food_name_f4_1.Text = "";
        richTextBox_total_price_f1.Enabled=false;
        richTextBox_total_price_f1.Text = "";
        richTextBox_price_f1.Enabled=false;
        richTextBox_price_f1.Text = "";
        richTextBox_number_f1.Enabled = false;
        richTextBox_number_f1.Text = "";
        total_Price_lbl.Visible = false;
        lbl_price_header_f1.Visible = false;
        total = 0;
        foods_for_efect.Clear();
        ingredientslist_f3_1.Clear();
        }

        private void calculateTheBill_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"calculateTheBill_ToolStripMenuItem_Click");
            Log.Close();




            FullClear();
            food_name_txt_f1.Enabled = true;
            number_of_food_txt_f1.Enabled = true;
            add_btn_f1.Enabled = true;
            end_and_Show_Bill_btn_f1.Enabled = true;
            Close_bill_btn_f1.Enabled = true;
            richTextBox_food_f1.Enabled = true;
            richTextBox_number_f1.Enabled = true;
            richTextBox_price_f1.Enabled = true;
            richTextBox_total_price_f1.Enabled = true;
        }

        private void foodEfectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"foodEfectToolStripMenuItem_Click");
            Log.Close();

            FullClear();
            btn_add_f2.Enabled = true;
            btn_disable_f2.Enabled = true;
            btn_end_f2.Enabled = true;
            food_name_txt_f2.Enabled = true;
            richTextBox_efects_f2.Enabled = true;
            richTextBox_foods_f2.Enabled = true;
        }

        private void addFoodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"addFoodToolStripMenuItem_Click");
            Log.Close();

            FullClear();
            btn_add_food_last_f3_1.Enabled = true;
            btn_add_ingredient_f3_1.Enabled = true;
            richTextBox_ingredients_f3_1.Enabled = true;
            txt_foodname_f3_1.Enabled = true;
            txt_ingredient_f3_1.Enabled = true;
            
        }

        private void addEfectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"addEfectToolStripMenuItem_Click");
            Log.Close();

            FullClear();
            btn_add_efect_last_f3_2.Enabled = true;
            txt_efect_f3_2.Enabled = true;
            txt_foodname1_f3_2.Enabled = true;
            txt_foodname2_f3_2.Enabled = true;
        }

        private void addIngredientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"addIngredientToolStripMenuItem_Click");
            Log.Close();

            FullClear();
            btn_add_ingredient_last_f3_3.Enabled = true;
            txt_ingredient_name_f3_3.Enabled = true;
            txt_ingredient_price_f3_3.Enabled = true;
        }

        private void removeFoodOrEfectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"removeFoodOrEfectToolStripMenuItem_Click");
            Log.Close();

            FullClear();
            btn_remove_food_last_f4_1.Enabled = true;
            txt_food_name_f4_1.Enabled = true;
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now+" --> "+"removeToolStripMenuItem_Click");
            Log.Close();

            FullClear();
            btn_remove_just_efect_last_f4_2.Enabled = true;
            txt_food_1_name_f4_2.Enabled = true;
            txt_food_2_name_f4_2.Enabled = true;
        }



        //btn_funcs-------------------------------------------------------------------------------------





        //fffffffffffff1111111111111111111111111
        //fffffffffffff1111111111111111111111111
        //fffffffffffff1111111111111111111111111
        //fffffffffffff1111111111111111111111111

        int total = 0;
        private void add_btn_f1_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            

            if(food_name_txt_f1.Text=="")
            {
                MessageBox.Show("Pleas enter food name!");
                watch.Stop();
            }
            else if(number_of_food_txt_f1.Text=="")
            {
                MessageBox.Show("Pleas enter number of foods!");
                watch.Stop();
            }
            else
            {
                int number = 0;
                bool ok = true;
                int food_total=0;
                string food_name=food_name_txt_f1.Text;



                int price=GetBill(food_name);
                //edit
                //int price = 200;
                watch.Stop();
                food_name_txt_f1.Text="";
                try
                {
                    number = int.Parse(number_of_food_txt_f1.Text);
                    number_of_food_txt_f1.Text = "";
                }
                catch
                {
                    number_of_food_txt_f1.Text = "";
                    ok = false;
                    
                    MessageBox.Show("pleas enter number in correct foramt !");
                }
                

                if(price!=-1&&ok)
                {
                    richTextBox_food_f1.Text+=(food_name+"\n");
                    richTextBox_number_f1.Text+=(number.ToString()+"\n");
                    richTextBox_price_f1.Text+=(price.ToString()+"\n");
                    food_total = number * price;
                    richTextBox_total_price_f1.Text += (food_total.ToString() + "\n");
                    total=food_total+total;
                    total_Price_lbl.Text = total.ToString();
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " +number+" "+food_name+" Aded to bill with total price : "+food_total);
                    Log.Close();
                    
                }
               


            }

            watch.Stop();
            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + (watch.ElapsedMilliseconds)*1000);
            TIME.Close();
        }

        private void end_and_Show_Bill_btn_f1_Click(object sender, EventArgs e)
        {
            total_Price_lbl.Visible = true;
            lbl_price_header_f1.Visible = true;
            end_and_Show_Bill_btn_f1.Enabled = false;
            add_btn_f1.Enabled = false;
            food_name_txt_f1.Enabled = false;
            number_of_food_txt_f1.Enabled = false;
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now + " --> " + "a bill Was issued whit price : "+total);
            Log.Close();
        }

        private void Close_bill_btn_f1_Click(object sender, EventArgs e)
        {
            FullClear();
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now + " --> " + "Bill Cleared!");
            Log.Close();
        }









        //fffffffffffff2222222222222222222222222
        //fffffffffffff2222222222222222222222222
        //fffffffffffff2222222222222222222222222
        //fffffffffffff2222222222222222222222222



        List<string> foods_for_efect=new List<string>();
        private void btn_add_f2_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            if (food_name_txt_f2.Text == "")
            {
                watch.Stop();
                MessageBox.Show("pleas enter food name !");
            }
            else
            {
                string food_name=food_name_txt_f2.Text;
                food_name_txt_f2.Text = "";
                int food = FindIndex(food_name);
                //edit
                //int food = 2;
                if (food!=-1)
                {
                    richTextBox_foods_f2.Text += food_name + "\n";
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + "food "+food_name+" aded for check efects.");
                    Log.Close();
                    if (foods_for_efect.Count > 0)
                    {
                        foreach (string fd in foods_for_efect)
                        {
                            //string efect = IsEffect(food_name, fd);
                            //edit
                            string efect = "Bad";
                            richTextBox_efects_f2.Text += food_name + " has " + efect + " efect with " + fd + "\n";
                            
                        }

                    }
                    StreamWriter Log2 = File.AppendText("logs.txt");
                    Log2.WriteLine(DateTime.Now + " --> " + "food " + food_name + " efects with other foods aded to efect list.");
                    Log2.Close();
                    foods_for_efect.Add(food_name);
                    watch.Stop();
                    
                }
                else
                {
                    watch.Stop();
                    MessageBox.Show("Food Not Found In data base !");
                }

            }

            watch.Stop();

            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + (watch.ElapsedMilliseconds) * 1000);
            TIME.Close();
        }

        private void btn_end_f2_Click(object sender, EventArgs e)
        {
            btn_add_f2.Enabled = false;
            food_name_txt_f2.Enabled = false;
            btn_end_f2.Enabled = false;
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now + " --> " + "end of checking efects.");
            Log.Close();
        }

        private void btn_disable_f2_Click(object sender, EventArgs e)
        {
            FullClear();
            StreamWriter Log = File.AppendText("logs.txt");
            Log.WriteLine(DateTime.Now + " --> " + "clear and exit from ckeck efect section.");
            Log.Close();
        }






        //fffffffffffff3333333333333333333333-1
        //fffffffffffff3333333333333333333333-1
        //fffffffffffff3333333333333333333333-1




        List<string> ingredientslist_f3_1 = new List<string>();
        private void btn_add_ingredient_f3_1_Click(object sender, EventArgs e)
        {


            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();






            if (txt_ingredient_f3_1.Text == "")
            {
                watch.Stop();
                MessageBox.Show("Pleas enter ingredient !");
            }
            else
            {
                ingredientslist_f3_1.Add(txt_ingredient_f3_1.Text);
                richTextBox_ingredients_f3_1.Text += txt_ingredient_f3_1.Text + "\n";
                
                StreamWriter Log = File.AppendText("logs.txt");
                watch.Stop();
                Log.WriteLine(DateTime.Now + " --> " + txt_ingredient_f3_1.Text + " aded to ingredient list for Add a food");
                Log.Close();
                txt_ingredient_f3_1.Text = "";
            }







            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + watch.ElapsedMilliseconds);
            TIME.Close();
        }

        private void btn_add_food_last_f3_1_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();


            if (txt_foodname_f3_1.Text == "")
            {
                watch.Stop();
                MessageBox.Show("Pleas enter Food Name !");
            }
            else
            {
                
                int pos = NewFood(txt_foodname_f3_1.Text, ingredientslist_f3_1);
                //edit
                //int pos = 0;
                watch.Stop();
                if (pos == 0)
                {
                    MessageBox.Show("Successful!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + txt_foodname_f3_1+" food aded to Foods!");
                    Log.Close();
                }
                if(pos == -1)
                {
                    MessageBox.Show("The food is already in the database!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + txt_foodname_f3_1 + " food adding confront with Error.");
                    Log.Close();
                }
                if (pos == -2)
                {
                    MessageBox.Show("Do not have necessary ingredients!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + txt_foodname_f3_1 + " food adding confront with Error.");
                    Log.Close();
                }
                txt_foodname_f3_1.Text = "";
                ingredientslist_f3_1.Clear();
                richTextBox_ingredients_f3_1.Text = "";
                txt_ingredient_f3_1.Text = "";

            }
            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + watch.ElapsedMilliseconds);
            TIME.Close();
        }








        //fffffffffffff3333333333333333333333-2
        //fffffffffffff3333333333333333333333-2
        //fffffffffffff3333333333333333333333-2

        private void btn_add_efect_last_f3_2_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();


            if (txt_foodname1_f3_2.Text=="")
            {
                watch.Stop();
                MessageBox.Show("Please enter Food1 Name !");
            }
            else if (txt_foodname2_f3_2.Text == "")
            {
                watch.Stop();
                MessageBox.Show("Please enter Food2 Name !");
            }
            else if(txt_efect_f3_2.Text=="")
            {
                watch.Stop();
                MessageBox.Show("Please enter efect !");
            }
            else
            {
                bool done=NewEffect(txt_foodname1_f3_2.Text,txt_foodname2_f3_2.Text,txt_efect_f3_2.Text);
                //edit
                //bool done = true;
                watch.Stop();
                if(!done)
                {
                    txt_foodname1_f3_2.Text = "";
                    txt_foodname2_f3_2.Text = "";
                    MessageBox.Show("Atleast One of foods is not in the database!");

                }
                else
                {
                    MessageBox.Show("Successful!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + txt_efect_f3_2.Text + " efect added between " + txt_foodname1_f3_2.Text + " and " + txt_foodname2_f3_2.Text);
                    Log.Close();
                    txt_foodname1_f3_2.Text = "";
                    txt_foodname2_f3_2.Text = "";
                    txt_efect_f3_2.Text = "";
                }

            }
            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + watch.ElapsedMilliseconds);
            TIME.Close();

        }









        //fffffffffffff3333333333333333333333-3
        //fffffffffffff3333333333333333333333-3
        //fffffffffffff3333333333333333333333-3


        private void btn_add_ingredient_last_f3_3_Click(object sender, EventArgs e)
        {

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();


            if (txt_ingredient_name_f3_3.Text=="")
            {
                watch.Stop();
                MessageBox.Show("Please enter ingredient Name !");
            }
            else if (txt_ingredient_price_f3_3.Text=="")
            {
                watch.Stop();
                MessageBox.Show("Please enter ingredient Price !");
            }
            else
            {
                bool done = NewIngredient(txt_ingredient_name_f3_3.Text, txt_ingredient_price_f3_3.Text);
                //edit
                //bool done = true;
                watch.Stop();
                if (done)
                {
                    MessageBox.Show("Successful!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + "ingredient " + txt_ingredient_name_f3_3.Text + " wtih price " + txt_ingredient_price_f3_3+" aded to data base.");
                    Log.Close();
                    txt_ingredient_name_f3_3.Text = "";
                    txt_ingredient_price_f3_3.Text = "";
                }
                else
                {
                    MessageBox.Show("Fail!  The ingredient is already in the database!");
                    txt_ingredient_name_f3_3.Text = "";
                    txt_ingredient_price_f3_3.Text = "";
                }
            }

            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + watch.ElapsedMilliseconds);
            TIME.Close();
        }









        //fffffffffffff44444444444444444444-1
        //fffffffffffff44444444444444444444-1
        //fffffffffffff44444444444444444444-1



        private void btn_remove_food_last_f4_1_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            if (txt_food_name_f4_1.Text == "")
            {
                watch.Stop();
                MessageBox.Show("Please enter Food Name !");
            }
            else
            {
                bool done = Delete(txt_food_name_f4_1.Text);
                //edit
                //bool done=false;
                watch.Stop();
                if (done)
                {
                    MessageBox.Show("Successful!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + "Food " + txt_food_name_f4_1+" removed from database.");
                    Log.Close();
                    txt_food_name_f4_1.Text = "";
                }
                else
                {
                    MessageBox.Show("Fail!   Food " + txt_food_name_f4_1.Text + " is not in the database!");
                    txt_food_name_f4_1.Text = "";
                }
            }
            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + watch.ElapsedMilliseconds);
            TIME.Close();
        }




        //fffffffffffff44444444444444444444-2
        //fffffffffffff44444444444444444444-2
        //fffffffffffff44444444444444444444-2



        private void btn_remove_just_efect_last_f4_2_Click(object sender, EventArgs e)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            if (txt_food_1_name_f4_2.Text=="")
            {
                watch.Stop();
                MessageBox.Show("Please enter Food1 Name !");
            }
            else if (txt_food_2_name_f4_2.Text=="")
            {
                watch.Stop();
                MessageBox.Show("Please enter Food2 Name !");
            }
            else
            {
                int pos = Delete(txt_food_1_name_f4_2.Text, txt_food_2_name_f4_2.Text);
                //edit
                //int pos = -2;
                watch.Stop();
                if (pos == 0)
                {
                    MessageBox.Show("Successful!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + "Food efect between " + txt_food_1_name_f4_2.Text + " and " + txt_food_2_name_f4_2.Text + " removed from database.");
                    Log.Close();
                    txt_food_1_name_f4_2.Text = "";
                    txt_food_2_name_f4_2.Text = "";
                }
                else if (pos == -1)
                {
                    MessageBox.Show("atleast one of foods is not in the database!");
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + "removing Food efect between " + txt_food_1_name_f4_2.Text + " and " + txt_food_2_name_f4_2.Text + "confront with ERROR.");
                    Log.Close();
                    txt_food_1_name_f4_2.Text = "";
                    txt_food_2_name_f4_2.Text = "";

                }
                else if (pos == -2)
                {
                    MessageBox.Show("There is no food effect for " + txt_food_1_name_f4_2.Text + " and " + txt_food_2_name_f4_2.Text);
                    StreamWriter Log = File.AppendText("logs.txt");
                    Log.WriteLine(DateTime.Now + " --> " + "removing Food efect between " + txt_food_1_name_f4_2.Text + " and " + txt_food_2_name_f4_2.Text + "confront with ERROR.");
                    Log.Close();
                    txt_food_1_name_f4_2.Text = "";
                    txt_food_2_name_f4_2.Text = "";
                }



            }
            StreamWriter TIME = File.AppendText("time.txt");
            TIME.WriteLine(DateTime.Now + "process time --> " + watch.ElapsedMilliseconds);
            TIME.Close();
        }                                 













        /*
        StreamWriter Log = File.AppendText("logs.txt");
        Log.WriteLine(DateTime.Now+" --> "+log);
        Log.Close();
        */



        /*
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        watch.Stop();
        
        StreamWriter TIME = File.AppendText("time.txt");
        TIME.WriteLine(DateTime.Now+"process time --> "+watch.ElapsedMilliseconds);
        TIME.Close();
        */

    }
}
