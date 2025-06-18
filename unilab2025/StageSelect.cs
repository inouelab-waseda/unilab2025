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

        #region 各種メンバ変数の定義など
        public StageSelect()
        {
            InitializeComponent();            

        }

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
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];

            foreach (Control control in this.Controls)
            {
                if (control is Button button && button.Name != "button_ToMap")
                {
                    string NameWithoutButton = button.Name.Replace("button_Stage", "");
                    if (int.TryParse(NameWithoutButton, out int j))
                    {
                        button.Text = "ステージ " + _worldNumber + " - " + j;


                    }
                }
            }                       
            if (_worldNumber == 1)
            {
                button_Stage3.Visible = false;
            }
        }

        private void button_ToMap_Click(object sender, EventArgs e)
        {
            if (_worldNumber <= 4) Func.CreateWorldMap(this);
            else Func.CreateAnotherWorld(this);
        }

        private void button_StageI_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
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
