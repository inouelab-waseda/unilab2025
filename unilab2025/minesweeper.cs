using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class minesweeper : Form
    {
        public minesweeper()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }


        public static int[,] map=new int [20,20];
        public static int[,] mine = new int[20, 20];
        public static int[,] flag = new int[20, 20];


        private void minesweeper_Load(object sender, EventArgs e)
        {

        }

        private void button_Start_Click(object sender, EventArgs e)
        {
            int NMine = 20 * 20/10;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = 0;
                }
            }
            Random rnd = new Random();
            while (true)
            {
                
                int random_x = rnd.Next(0, 20);
                int random_y = rnd.Next(0, 20);
                if (map[random_x, random_y]==0)
                {
                    map[random_x, random_y] = 1;
                    NMine -= 1;
                    if (NMine == 0) break;
                }

            }

        }




    }
}
