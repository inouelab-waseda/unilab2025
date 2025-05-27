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
            this.listBox_Input.Click += new System.EventHandler(this.listBox_Input_Click);
            this.listBox_Car.Click += new System.EventHandler(this.listBox_Car_Click);

            #region ボタン表示
            Arrow();//矢印の表示設定

            #endregion

            //pictureBoxの設定
            pictureBox_Map.Parent = pictureBox_Background;
            //pictureBox1.Location = new Point(600, 50);
            pictureBox_Map.Location = new Point(30, 100);

            bmp1 = new Bitmap(pictureBox_Map.Width, pictureBox_Map.Height);
            pictureBox_Map.Image = bmp1;

            //ボタンを有効にする
            pictureBox0.Click += PictureBox0_Click;
            pictureBox1.Click += PictureBox1_Click;
            pictureBox2.Click += PictureBox2_Click;
            pictureBox3.Click += PictureBox3_Click;
            pictureBox4.Click += PictureBox4_Click;
            pictureBox5.Click += PictureBox5_Click;
            pictureBox6.Click += PictureBox6_Click;
            pictureBox7.Click += PictureBox7_Click;

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

        public static List<ListBox> ListBoxes = new List<ListBox>();
        public static ListBox InputListBox;   //入力先のリストボックス
        public static bool isChange = false;  //入力変更状態かどうか
        public static bool isFor = false;     //For文入力中かどうか
        public static int Change_Item_Number;
        public static int[,] map = new int[12, 12]; //map情報
        public static string stageName;

        //listBoxに入れられる行数の制限
        public static int limit_LB_Input=10;
        public static int limit_LB_Car=10;

        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成
                                          //button1.Visible = false; // 非表示にする

            InputListBox = listBox_Input;
            ListBoxes.Add(listBox_Input);
            ListBoxes.Add(listBox_Car);



        }

        private int[,] CreateStage(string stageName)     //ステージ作成
        {
            return map;
        }
        #region 各コントロール機能設定

        private void listBox_Input_Click(object sender, EventArgs e)
        {
            InputListBox = listBox_Input;
            listBox_Input.Focus();
            ShowListBox();
        }

        private void listBox_Car_Click(object sender, EventArgs e)
        {
            InputListBox = listBox_Car;
            listBox_Car.Focus();
            ShowListBox();
        }

        public void ShowListBox()
        {
            foreach (ListBox listbox in ListBoxes)
            {                
                listbox.BackColor = Color.Gray;
                listbox.ForeColor = Color.Black;
            }
            InputListBox.BackColor = Color.White;
            InputListBox.ForeColor = Color.Black;
                      

        }

        #endregion

            #region ボタンの処理
        private void PictureBox0_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            listBox_Input.Items.Add("↑");
            //if (isChange) Item_Change();
            //else Left_Availabel_Input();
        }
        private void PictureBox1_Click(object sender, EventArgs e) { }
        private void PictureBox2_Click(object sender, EventArgs e) { }
        private void PictureBox3_Click(object sender, EventArgs e) { }
        private void PictureBox4_Click(object sender, EventArgs e) { }
        private void PictureBox5_Click(object sender, EventArgs e) { }
        private void PictureBox6_Click(object sender, EventArgs e) { }
        private void PictureBox7_Click(object sender, EventArgs e) { }

        bool Input_check()
        {
            bool result = false;
            switch (InputListBox.Name)
            {
                case "listBox_Input":
                    if (InputListBox.Items.Count < limit_LB_Input) break;
                    else goto default;
                case "listBox_Car":
                    if (InputListBox.Items.Count < limit_LB_Car) break;
                    else goto default;
                default:
                    //label_Info.Text = "これ以上入力できないよ";
                    //label_Info.Visible = true;
                    //DisplayMessage("Overflow");
                    result = true;
                    break;
            }
            return result;
        }



        #endregion


        #region ボタンの表示
        private Image RotateImage(Image img, float angle)
        {
            Bitmap bmp = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.TranslateTransform((float)img.Width / 2, (float)img.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-(float)img.Width / 2, -(float)img.Height / 2);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, new Point(0, 0));
            }
            return bmp;
        }

        private void Arrow()//矢印の表示
        {
            Image original = Dictionaries.Img_Button["arrow1"];
            pictureBox0.Image= RotateImage(original, 0f);
            pictureBox1.Image = RotateImage(original, 45f);
            pictureBox2.Image = RotateImage(original, 90f);
            pictureBox3.Image = RotateImage(original, 135f);
            pictureBox4.Image = RotateImage(original, 180f);
            pictureBox5.Image = RotateImage(original, 225f);
            pictureBox6.Image = RotateImage(original, 270f);
            pictureBox7.Image = RotateImage(original, 315f);
        }

        #endregion

        
    }
}
