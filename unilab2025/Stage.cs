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
    public partial class Stage : Form
    {
        public Stage()
        {
            InitializeComponent();
        }

        #region メンバー変数定義
        private string _worldName;
        private int _worldNumber;
        private int _level;
        public int WorldNumber     //StageSelectからの呼び出し用
        {
            get { return _worldNumber; }
            set { _worldNumber = value; }
            //別フォームからのアクセス例
            //Stage form = new Stage();
            //form.StageName = "ステージ名";
        }
        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }
        public string WorldName
        {
            get { return _worldName; }
            set { _worldName = value; }
        }
        #endregion


        #region グローバル変数定義
        //ここに必要なBitmapやImageを作っていく
        public static string stageName;



        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];



        }
    }
}
