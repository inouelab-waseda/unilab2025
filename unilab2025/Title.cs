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
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            pictureBox1.Click += pictureBox1_Click;
            this.KeyDown += new KeyEventHandler(Title_KeyDown);
            this.KeyPreview = true;
            this.DoubleBuffered = true;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string worldName;
            int worldNumber;
            int level;
            switch (CurrentFormState.FormName)
            {
                case "Prologue":
                    Func.CreatePrologue(this);
                    break;

                case "Stage":
                    worldName = (string)CurrentFormState.StateData["WorldName"];
                    worldNumber = (int)CurrentFormState.StateData["WorldNumber"];
                    level = (int)CurrentFormState.StateData["Level"];
                    Func.CreateStage(this, worldName, worldNumber, level);
                    break;

                case "WorldMap":
                    Func.CreateWorldMap(this);
                    break;

                case "AnotherWorld":
                    Func.CreateAnotherWorld(this);
                    break;
            }


        }

        private void Title_Load(object sender, EventArgs e)
        {

        }
        private void Title_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                Func.CreateMiniGame(this);
            }
        }
    }
}
