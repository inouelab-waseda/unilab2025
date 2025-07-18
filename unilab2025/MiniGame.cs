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
    public partial class MiniGame : Form
    {
        public MiniGame()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.KeyPreview = true;
        }


        private void MiniGame_Load(object sender, EventArgs e)
        {

        }


        private void button_Mario_Click(object sender, EventArgs e)
        {
            Func.CreateMario(this);
        }

    }
}
