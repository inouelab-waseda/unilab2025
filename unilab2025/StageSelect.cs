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
    public partial class StageSelect : Form
    {
        public StageSelect()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        }
        #region 各種メンバ変数の定義など

        private string _worldName;  //WorldMapで選択された学年
        private int _worldNumber;
        public string WorldName     //こう書くと別フォームからアクセスできるっぽい。原理はよくわからん
        {
            get { return _worldName; }
            set { _worldName = value; }
            //別フォームからのアクセス例
            //StageSelect form = new StageSelect();
            //form.WorldName = "学年";
        }
        public int WorldNumber
        {
            get { return _worldNumber; }
            set { _worldNumber = value; }
        }
        #endregion

        private void StageSelect_Load(object sender, EventArgs e)
        {
            this.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];
            if (_worldNumber > 4) return;
            button_Stage1.BackgroundImage = Dictionaries.Img_Button_MapSelect[_worldNumber + "-1"];
            button_Stage2.BackgroundImage = Dictionaries.Img_Button_MapSelect[_worldNumber + "-2"];
            
            if (_worldNumber == 1)
            {
                button_Stage3.Visible = false;                
            }
            else button_Stage3.BackgroundImage = Dictionaries.Img_Button_MapSelect[_worldNumber + "-3"];
            if (!ClearCheck.IsButtonEnabled[_worldNumber, 2])
            {
                button_Stage2.Visible = false;
                
            }
            if (!ClearCheck.IsButtonEnabled[_worldNumber, 3])
            {
                button_Stage3.Visible = false;
            }
            foreach (Control control in this.Controls)
            {
                if (control is CustomButton button)
                {
                    string NameWithoutButton = button.Name.Replace("button_Stage", "");
                    if (int.TryParse(NameWithoutButton, out int j))
                    {
                        if (ClearCheck.IsButtonEnabled[_worldNumber, j])
                        {
                            button.ForeImage = null;
                            button.Cursor = Cursors.Hand;
                            if (ClearCheck.IsNew[_worldNumber, j])
                            {
                                //button.BackColor = Color.FromArgb(255, 128, 128);
                                button.ConditionImage = Dictionaries.Img_Button["New"];
                            }
                        }
                    }
                }
            }

        }

        private void button_ToMap_Click(object sender, EventArgs e)
        {
            if (_worldNumber <= 4) Func.CreateWorldMap(this);
            else Func.CreateAnotherWorld(this);
        }

        private void button_StageI_Click(object sender, EventArgs e)
        {
            //Button button = sender as Button;
            CustomButton button = sender as CustomButton;
            if (button != null)
            {
                string NameWithoutButton = button.Name.Replace("button_Stage", "");
                if (int.TryParse(NameWithoutButton, out int j))
                {
                    if (!ClearCheck.IsButtonEnabled[_worldNumber, j]) 
                    {
                        MessageBox.Show("まだできないよ");
                        return;
                    }
                    
                    ClearCheck.IsNew[_worldNumber, j] = false;
                    Func.UpdateIsNew();
                    Func.CreateStage(this, _worldName, _worldNumber, j);
                }
            }
        }

     
    }
}
