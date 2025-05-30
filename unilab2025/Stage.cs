using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security;
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

            

            pictureBox0.Click += PictureBox0_Click;
            pictureBox1.Click += PictureBox1_Click;
            pictureBox2.Click += PictureBox2_Click;
            pictureBox3.Click += PictureBox3_Click;
            pictureBox4.Click += PictureBox4_Click;
            pictureBox5.Click += PictureBox5_Click;
            pictureBox6.Click += PictureBox6_Click;
            pictureBox7.Click += PictureBox7_Click;
            
            pictureBox1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox5.Visible = false;
            pictureBox7.Visible = false;

            
            #region ボタン表示
            Arrow();//矢印の表示設定

            #endregion

            //pictureBoxの設定
            pictureBox_Map.Parent = pictureBox_Background;
            //pictureBox1.Location = new Point(600, 50);
            pictureBox_Map.Location = new Point(30, 100);

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

        public static List<ListBox> ListBoxes = new List<ListBox>();
        public static ListBox InputListBox;   //入力先のリストボックス
        public static bool isChange = false;  //入力変更状態かどうか
        public static bool isFor = false;     //For文入力中かどうか
        public static int Change_Item_Number;
        public static int[,] map = new int[12, 12]; //map情報
        public static string stageName;

        //表示される絵文字
        public static Dictionary<string, string> Emoji = new Dictionary<string, string>()
        {
            //絵文字の追加
            { "walk", "🚶‍" },
            { "car", "🚗" },
            { "balloon" ,"🎈" },
            { "plane","✈️" }

        };
        public static string picture;
        

        //listBoxに入れられる行数の制限
        public static int limit_LB_Input=10;
        public static int limit_LB_Car=10;

        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成
                                                               

            InputListBox = listBox_Input;
            ListBoxes.Add(listBox_Input);
            ListBoxes.Add(listBox_Car);
            picture = "walk";            
            listBox_Input.Focus();
            ShowListBox();




        }

        private int[,] CreateStage(string stageName)     //ステージ作成
        {
            return map;
        }
        #region 各コントロール機能設定

        private void listBox_Input_Click(object sender, EventArgs e)
        {

            pictureBox0.Visible = true;
            pictureBox2.Visible = true;
            pictureBox4.Visible = true;
            pictureBox6.Visible = true;
            pictureBox1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox5.Visible = false;
            pictureBox7.Visible = false;

            button1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = true;
            button4.Enabled = true;

            picture = "walk";
            InputListBox = listBox_Input;
            listBox_Input.Focus();
            ShowListBox();
        }

        private void listBox_Car_Click(object sender, EventArgs e)
        {
            //ボタンを有効にする           
            pictureBox0.Visible = true;
            pictureBox2.Visible = true;
            pictureBox4.Visible = true;
            pictureBox6.Visible = true;
            pictureBox1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox5.Visible = false;
            pictureBox7.Visible = false;

            button1.Enabled = false;
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;


            picture = "car";
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

        #region リセット関連        

        private void button_one_Reset_Click(object sender, EventArgs e)
        {
            if (InputListBox.SelectedIndex > -1)
            {
                InputListBox.Items.RemoveAt(InputListBox.SelectedIndex);
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            InputListBox.Items.Clear();
        }

        #endregion

        #region ボタンの処理
        private void PictureBox0_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] +"  "+"↑");
            //if (isChange) Item_Change();
            //else Left_Availabel_Input();
        }
        private void PictureBox1_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↗");
        }
        private void PictureBox2_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "→");
        }
        private void PictureBox3_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↘");
        }
        private void PictureBox4_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↓");
        }
        private void PictureBox5_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↙");
        }
        private void PictureBox6_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "←");
        }
        private void PictureBox7_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↖");
        }

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

        

        private void button1_Click(object sender, EventArgs e)
        {
            picture = "walk";
            //ボタンを有効にする            
            pictureBox0.Visible = true;
            pictureBox2.Visible = true;
            pictureBox4.Visible = true;
            pictureBox6.Visible = true;
            pictureBox1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox5.Visible = false;
            pictureBox7.Visible = false;
           

        }
        private void button2_Click(object sender, EventArgs e)
        {            
            if (listBox_Car.Items.Count < 1)
            {
                MessageBox.Show("車の入力をしてね");
                pictureBox0.Visible = true;
                pictureBox2.Visible = true;
                pictureBox4.Visible = true;
                pictureBox6.Visible = true;
                pictureBox1.Visible = false;
                pictureBox3.Visible = false;
                pictureBox5.Visible = false;
                pictureBox7.Visible = false;

                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
                button4.Enabled = false;
                picture = "car";
                InputListBox = listBox_Car;
                listBox_Car.Focus();
                ShowListBox();
                return;
            }
            if (Input_check()) return;
            string combined = "";
            foreach (var item in listBox_Car.Items)
            {
                string text = item.ToString();

                // 先頭の1文字（または2文字）を抽出（絵文字によっては2文字以上）
                string emoji = text.Substring(text.Length - 2, 2); // 1〜2文字目を仮に絵文字として取り出す
                combined += emoji;
            }

            InputListBox.Items.Add("🚗 ("+combined+")");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            picture = "balloon";            
            //ボタンを有効にする

            pictureBox1.Visible = true;
            pictureBox3.Visible = true;
            pictureBox5.Visible = true;
            pictureBox7.Visible = true;
            pictureBox0.Visible = false;
            pictureBox2.Visible = false;
            pictureBox4.Visible = false;
            pictureBox6.Visible = false;
            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            picture = "plane";
            
            //ボタンを有効にする

            pictureBox0.Visible = true;
            pictureBox2.Visible = true;
            pictureBox4.Visible = true;
            pictureBox6.Visible = true;
            pictureBox1.Visible = false;
            pictureBox3.Visible = false;
            pictureBox5.Visible = false;
            pictureBox7.Visible = false;
            

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
