using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
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
            
            this.KeyPreview = true;

            #region ボタン表示
            button_up.Size = new Size(80, 80);
            button_left.Size = new Size(80, 80);
            button_right.Size = new Size(80, 80);
            button_down.Size = new Size(80, 80);
            #endregion

            //pictureBoxの設定
            pictureBox_Map.Parent = pictureBox_Background;
            //pictureBox1.Location = new Point(600, 50);
            pictureBox_Map.Location = new Point(0, 0);

            bmp1 = new Bitmap(pictureBox_Map.Width, pictureBox_Map.Height);
            pictureBox_Map.Image = bmp1;



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
        Bitmap bmp1;

        public static string stageName;

        public static int[,] map = new int[12, 12]; //map情報

        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成
                        
        }

        private int[,] CreateStage(string stageName)     //ステージ作成
        {
            return map;
        }
                
    }
}
