﻿using System;
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
    public partial class WorldMap : Form
    {
        public WorldMap()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.KeyDown += new KeyEventHandler(WorldMap_KeyDown);
            this.KeyPreview = true;

        }

        #region 読み込み時
        private void WorldMap_Load(object sender, EventArgs e)
        {
            if (!(ClearCheck.IsCleared[1, 0])) button2.Visible = false;
            if (!(ClearCheck.IsCleared[2, 0])) button3.Visible = false;
            if (!(ClearCheck.IsCleared[3, 0])) button4.Visible = false;
            if (!(ClearCheck.IsCleared[4,0])) button5.Visible = false;
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            //CustomButton button = sender as CustomButton;
            if (button1 != null)
            {
                string NameWithoutButton = button1.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {
                    //if (!ClearCheck.IsButtonEnabled[i, 0]) return;
                    Func.CreateStageSelect(this, button1.Text, i);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2 != null)
            {
                string NameWithoutButton = button2.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {
                    //if (!ClearCheck.IsButtonEnabled[i, 0]) return;
                    Func.CreateStageSelect(this, button2.Text, i);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3 != null)
            {
                string NameWithoutButton = button3.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {
                    //if (!ClearCheck.IsButtonEnabled[i, 0]) return;
                    Func.CreateStageSelect(this, button3.Text, i);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4 != null)
            {
                string NameWithoutButton = button4.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {
                    //if (!ClearCheck.IsButtonEnabled[i, 0]) return;
                    Func.CreateStageSelect(this, button4.Text, i);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Func.CreateAnotherWorld(this);
        }

        #region クリアチェックスキップ用
        private void WorldMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 0; j < (int)ConstNum.numStages; j++)
                    {
                        ClearCheck.IsNew[i, j] = false;
                        ClearCheck.IsCleared[i, j] = true;
                        ClearCheck.IsButtonEnabled[i, j] = true;
                    }
                }

                ClearCheck.PlayAfterChapter4Story = true;

                for (int i = 5; i < (int)ConstNum.numWorlds; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        ClearCheck.IsNew[i, j] = true;
                        ClearCheck.IsButtonEnabled[i, j] = true;
                    }
                }

                Func.CreateWorldMap(this);
            }
        }
        #endregion






    }
}
