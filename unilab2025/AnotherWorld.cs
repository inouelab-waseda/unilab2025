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
    public partial class AnotherWorld : Form
    {
        public AnotherWorld()
        {
            InitializeComponent();            
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //this.KeyDown += new KeyEventHandler(WorldMap_KeyDown);
            this.KeyPreview = true;
        }

        
        private void AnotherWorld_Load(object sender, EventArgs e)
        {
            
        }

        private void button_Japan_Click(object sender, EventArgs e)
        {
            Func.CreateWorldMap(this);
        }

        private void buttonI_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string NameWithoutButton = button.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {                    
                    Func.CreateStageSelect(this, i.ToString(), i);
                }
            }
        }

    }
}
