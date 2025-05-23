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
            this.WindowState = FormWindowState.Maximized;
            Form1_Resize(this, EventArgs.Empty); // 初期化時に1回反映

            this.KeyPreview = true;

            #region ボタン表示
            button_up.Size = new Size(80, 80);
            button_left.Size = new Size(80, 80);
            button_right.Size = new Size(80, 80);
            button_down.Size = new Size(80, 80);
            #endregion

            pictureBox_Map.Location = new Point(10, 20);

            bmp1 = new Bitmap(pictureBox_Map.Width, pictureBox_Map.Height);
            pictureBox_Map.Image = bmp1;



        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // 画面の幅・高さを取得
            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            // PictureBox を画面の中央に表示（横60%、縦80%サイズ）
            int margin = Math.Min((w) / 2, (h) / 2);            
            pictureBox_Map.Size = new Size((int)(w * 0.6), (int)(h * 0.8));
            pictureBox_Map.Location = new Point(margin, margin);

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
        Bitmap bmp1, bmp2;

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
