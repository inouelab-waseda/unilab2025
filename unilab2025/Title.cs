using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static unilab2025.Program;

namespace unilab2025
{
    public partial class Title : Form
    {
        public Title()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            string worldName;
            int worldNumber;
            int level;
            switch (CurrentFormState.FormName)
            {
                case "Prologue":
                    Program.Func.CreatePrologue(this);
                    break;

            }
        }
    }
}
