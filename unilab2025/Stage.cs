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
            

            this.KeyPreview = true;

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
        Bitmap bmp1, bmp2;

        public static string stageName;

        public static int[,] map = new int[12, 12]; //map情報

        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成
            MakeArrowUpButton(button_up);
            MakeArrowRightButton(button_right);
            MakeArrowLeftButton(button_left);
            MakeArrowDownButton(button_down);


        }

        private int[,] CreateStage(string stageName)     //ステージ作成
        {
            return map;
        }


        //ボタンの編集

        private void MakeArrowUpButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();
            Point[] arrowPoints = new Point[]
            {
        new Point(btn.Width / 2, 0),
        new Point(btn.Width, btn.Height / 2),
        new Point(btn.Width * 3 / 4, btn.Height / 2),
        new Point(btn.Width * 3 / 4, btn.Height),
        new Point(btn.Width / 4, btn.Height),
        new Point(btn.Width / 4, btn.Height / 2),
        new Point(0, btn.Height / 2),
            };
            path.AddPolygon(arrowPoints);
            btn.Region = new Region(path);
        }

        private void MakeArrowRightButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();
            Point[] arrowPoints = new Point[]
            {
        new Point(0, btn.Height / 2),
        new Point(btn.Width - btn.Height / 2, btn.Height / 2),
        new Point(btn.Width - btn.Height / 2, 0),
        new Point(btn.Width, btn.Height / 2),
        new Point(btn.Width - btn.Height / 2, btn.Height),
        new Point(btn.Width - btn.Height / 2, btn.Height / 2),
            };
            path.AddPolygon(arrowPoints);
            btn.Region = new Region(path);
        }

        private void MakeArrowLeftButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();
            Point[] arrowPoints = new Point[]
            {
        new Point(0, btn.Height / 2),
        new Point(btn.Height / 2, 0),
        new Point(btn.Height / 2, btn.Height / 3),
        new Point(btn.Width, btn.Height / 3),
        new Point(btn.Width, btn.Height * 2 / 3),
        new Point(btn.Height / 2, btn.Height * 2 / 3),
        new Point(btn.Height / 2, btn.Height),
            };
            path.AddPolygon(arrowPoints);
            btn.Region = new Region(path);
        }


        private void MakeArrowDownButton(Button btn)
        {
            GraphicsPath path = new GraphicsPath();
            Point[] arrowPoints = new Point[]
            {
        new Point(0, btn.Height / 2),
        new Point(btn.Width / 4, btn.Height / 2),
        new Point(btn.Width / 4, 0),
        new Point(btn.Width * 3 / 4, 0),
        new Point(btn.Width * 3 / 4, btn.Height / 2),
        new Point(btn.Width, btn.Height / 2),
        new Point(btn.Width / 2, btn.Height)
            };
            path.AddPolygon(arrowPoints);
            btn.Region = new Region(path);
        }


    }
}
