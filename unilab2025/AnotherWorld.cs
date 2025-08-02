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
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.KeyDown += new KeyEventHandler(AnotherWorldMap_KeyDown);
            this.KeyPreview = true;
            this.DoubleBuffered = true;
        }

        
        private void AnotherWorld_Load(object sender, EventArgs e)
        {
            // buttonに対する処理
            foreach (Control control in this.Controls)
            {
                if (control is CustomButton button)
                {
                    string NameWithoutButton = button.Name.Replace("button", "");
                    if (int.TryParse(NameWithoutButton, out int i))
                    {
                        button.ForeImage = null;
                        if (ClearCheck.IsNew[i, 0])
                        {
                            button.ConditionImage = Dictionaries.Img_Button["New"];
                        }
                        else if (ClearCheck.IsCleared[i, 0])
                        {
                            //button.BackColor = Color.FromArgb(255, 128, 128);
                            button.ConditionImage = Dictionaries.Img_Button["Clear"];
                        }
                        else
                        {
                            button.ConditionImage = null;
                        }

                    }

                }
            }


        }

        private void button_Japan_Click(object sender, EventArgs e)
        {
            Func.CreateWorldMap(this);
        }

        private void buttonI_Click(object sender, EventArgs e)
        {
            CustomButton button = sender as CustomButton;
            if (button != null)
            {
                string NameWithoutButton = button.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {                    
                    Func.CreateStageSelect(this, i.ToString(), i);
                }
            }
        }

        private void button_MiniGame_Click(object sender, EventArgs e)
        {
            Func.CreateMiniGame(this);
        }

        #region クリアチェックスキップ用
        private void AnotherWorldMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                for (int i = 5; i < (int)ConstNum.numWorlds; i++)
                {
                    for (int j = 0; j < (int)ConstNum.numStages; j++)
                    {
                        ClearCheck.IsNew[i, j] = false;
                        ClearCheck.IsCleared[i, j] = true;
                        ClearCheck.IsButtonEnabled[i, j] = true;
                    }
                }
               
                Func.CreateAnotherWorld(this);
            }
        }
        #endregion

    }
}
