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
            button_Try.Text = "チュートリアル" + _worldNumber;
            button_Stage.Text = "ステージ" + _worldNumber;
        }

        private void button_ToMap_Click(object sender, EventArgs e)
        {
            if (_worldNumber <= 4) Func.CreateWorldMap(this);
            else Func.CreateAnotherWorld(this);
        }
    }
}
