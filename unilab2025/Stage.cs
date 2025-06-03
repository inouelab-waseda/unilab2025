using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Emit;
using System.Drawing.Drawing2D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace unilab2025
{
    public partial class Stage : Form
    {
        public Stage()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //pictureBox_Conv = ConversationsFunc.CreatePictureBox_Conv(this);
            //pictureBox_Conv.Click += new EventHandler(pictureBox_Conv_Click);
            
            this.KeyPreview = true;
            this.listBox_Order.Click += new System.EventHandler(this.listBox_Order_Click);
            this.listBox_Car.Click += new System.EventHandler(this.listBox_Car_Click);

            

            pictureBox_buttonUp.Click += PictureBox_buttonUp_Click;
            pictureBox_upperRight.Click += PictureBox_upperRight_Click;
            pictureBox_buttonRight.Click += PictureBox_buttonRight_Click;
            pictureBox_lowerRight.Click += PictureBox_lowerRight_Click;
            pictureBox_buttonDown.Click += PictureBox_buttonDown_Click;
            pictureBox_lowerLeft.Click += PictureBox_lowerLeft_Click;
            pictureBox_buttonLeft.Click += PictureBox_buttonLeft_Click;
            pictureBox_upperLeft.Click += PictureBox_upperLeft_Click;

            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;

            
            #region ボタン表示
            Arrow();//矢印の表示設定

            #endregion

            bmp1 = new Bitmap(pictureBox_Map1.Width, pictureBox_Map1.Height);
            bmp2 = new Bitmap(pictureBox_Map2.Width, pictureBox_Map2.Height);
            pictureBox_Map1.Image = bmp1;
            pictureBox_Map2.Image = bmp2;

            g1 = Graphics.FromImage(bmp1);
            g2 = Graphics.FromImage(bmp2);

            pictureBox_Map2.BackColor = Color.Transparent;
            pictureBox_Map2.Parent = pictureBox_Map1;
            pictureBox_Map2.Location = new Point(0, 0); // 親コントロールの左上を基準に0,0に配置

            pictureBox_Map2.BringToFront(); //
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
        Bitmap bmp2;

        Brush goalBackgroundColor = new SolidBrush(Color.Yellow);
        Brush startBackgroundColor = new SolidBrush(Color.Blue);


        Image character_me = Dictionaries.Img_DotPic["銀髪ドット"];

        public static List<ListBox> ListBoxes = new List<ListBox>();
        public static ListBox InputListBox;   //入力先のリストボックス
        public static bool isChange = false;  //入力変更状態かどうか
        public static bool isFor = false;     //For文入力中かどうか
        public static int Change_Item_Number;
        public static int[,] map; //ステージのマップデータ
        public static int map_width; //マップの横幅
        public static string stageName;
        public static int x_start; //スタート位置ｘ
        public static int y_start; //スタート位置ｙ
        public static int x_goal; //ゴール位置ｘ
        public static int y_goal; //ゴール位置ｙ
        public static int x_now; //現在位置ｘ
        public static int y_now; //現在位置 y
        public static int extra_length = 7;
        public static int cell_length;
        public static int For_count = 1; //for文のループ回数を保存
        public static bool isEndfor = true; //forの最後が存在するか→ない場合はError

        public static int count = 0; //試行回数カウント
        public static int miss_count = 0; //ミスカウント

        public static int count_walk = 0; //歩数カウント

        public static List<int[]> move;  //プレイヤーの移動指示を入れるリスト

        //listBoxに入れられる行数の制限
        public static int limit_LB_Input=10;
        public static int limit_LB_Car=10;

        public static string hint;
        public static string hint_character;
        public static string hint_name;

        public static string grade;    //学年
        public static int gradenum;


        //public static List<Conversation> Conversations = new List<Conversation>();  //会話文を入れるリスト
        PictureBox pictureBox_Conv;
        byte[] Capt;
        List<Conversation> StartConv;
        List<Conversation> EndConv;
        List<Conversation> Message;
        bool isStartConv;
        bool isMessageMode;
        public Graphics g1;
        public Graphics g2;

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
                       

        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成
                                                               

            InputListBox = listBox_Order;
            ListBoxes.Add(listBox_Order);
            ListBoxes.Add(listBox_Car);
            picture = "walk";            
            listBox_Order.Focus();
            ShowListBox();
            grade = Regex.Replace(stageName, @"[^0-9]", "");
            int chapter_num = int.Parse(grade) / 10;
            

        }



        #region 各コントロール機能設定

        #endregion


        private int[,] CreateStage(string stageName)     //ステージ作成
        {
            //string stagenum = _worldNumber + "-" + _level;
            using (StreamReader sr = new StreamReader($"Map\\{stageName}.csv"))
            {
                int x;
                int y = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');
                    map_width = values.Length; //マップの横幅を取得

                    if (y == 0) map = new int[map_width, map_width]; //マップの初期化
                    x = 0;

                    foreach (var value in values)
                    {
                        // enum 使ったほうが分かりやすそう
                        map[x, y] = int.Parse(value);
                        switch (map[x, y])
                        {
                            case 0:
                                x_start = x;
                                y_start = y;
                                x_now = x;
                                y_now = y;
                                break;
                            case 1:
                                x_goal = x;
                                y_goal = y;
                                break;
                            default:
                                break;
                        }
                        x++;
                    }
                    y++;
                }
            }

            //label_Info.BackgroundImage = Image.FromFile("focus.png");


            cell_length = pictureBox_Map2.Width / map_width;


            for (int y = 0; y < map_width; y++)
            {
                for (int x = 0; x < map_width; x++)
                {
                    int placeX = x * cell_length;
                    int placeY = y * cell_length;
                    g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                    switch (map[x, y])
                    {
                        case 1:
                            x_goal = x;
                            y_goal = y;
                            if (_worldNumber > 4)
                            {
                                int goal = 10 + _worldNumber;
                                g2.DrawImage(Dictionaries.Img_Object[goal.ToString()], placeX, placeY, cell_length, cell_length);
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            // キャラクターの描画をループの外に出す
            g2.DrawImage(character_me, x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);

            this.Invoke((MethodInvoker)delegate
            {
                // pictureBox_Map2を同期的にRefreshする
                pictureBox_Map1.Refresh();
                pictureBox_Map2.Refresh();
            });
            return map;
        }
        #region 各コントロール機能設定

        private void listBox_Order_Click(object sender, EventArgs e)
        {

            pictureBox_buttonUp.Visible = true;
            pictureBox_buttonRight.Visible = true;
            pictureBox_buttonDown.Visible = true;
            pictureBox_buttonLeft.Visible = true;
            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;                     

            button_walk.Enabled = true;
            button_car.Enabled = true;
            button_balloon.Enabled = true;
            button_plane.Enabled = true;

            picture = "walk";
            InputListBox = listBox_Order;
            listBox_Order.Focus();
            ShowListBox();
        }

        private void listBox_Car_Click(object sender, EventArgs e)
        {
            //ボタンを有効にする           
            pictureBox_buttonUp.Visible = true;
            pictureBox_buttonRight.Visible = true;
            pictureBox_buttonDown.Visible = true;
            pictureBox_buttonLeft.Visible = true;
            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;

            button_walk.Enabled = false;
            button_car.Enabled = false;
            button_balloon.Enabled = false;
            button_plane.Enabled = false;


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

        private void button_back_Click(object sender, EventArgs e)
        {
            if (InputListBox.SelectedIndex > -1)
            {
                InputListBox.Items.RemoveAt(InputListBox.SelectedIndex);//1つ消す
            }
        }

        private void button_reset_Click(object sender, EventArgs e)
        {
            InputListBox.Items.Clear();//全て消す
        }

        #endregion

        #region ボタンの処理
        private void PictureBox_buttonUp_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] +"  "+"↑");
            //if (isChange) Item_Change();
            //else Left_Availabel_Input();
        }
        private void PictureBox_upperRight_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↗");
        }
        private void PictureBox_buttonRight_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "→");
        }
        private void PictureBox_lowerRight_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↘");
        }
        private void PictureBox_buttonDown_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↓");
        }
        private void PictureBox_lowerLeft_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↙");
        }
        private void PictureBox_buttonLeft_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "←");
        }
        private void PictureBox_upperLeft_Click(object sender, EventArgs e) {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↖");
        }

        bool Input_check()
        {
            bool result = false;
            switch (InputListBox.Name)
            {
                case "listBox_Order":
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

        

        private void button_walk_Click(object sender, EventArgs e)
        {
            picture = "walk";
            //ボタンを有効にする            
            pictureBox_buttonUp.Visible = true;
            pictureBox_buttonRight.Visible = true;
            pictureBox_buttonDown.Visible = true;
            pictureBox_buttonLeft.Visible = true;
            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;


        }
        private void button_car_Click(object sender, EventArgs e)
        {            
            if (listBox_Car.Items.Count < 1)
            {
                MessageBox.Show("車の入力をしてね");
                pictureBox_buttonUp.Visible = true;
                pictureBox_buttonRight.Visible = true;
                pictureBox_buttonDown.Visible = true;
                pictureBox_buttonLeft.Visible = true;
                pictureBox_upperRight.Visible = false;
                pictureBox_lowerRight.Visible = false;
                pictureBox_lowerLeft.Visible = false;
                pictureBox_upperLeft.Visible = false;

                button_walk.Enabled = false;
                button_car.Enabled = false;
                button_balloon.Enabled = false;
                button_plane.Enabled = false;
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

        private void button_balloon_Click(object sender, EventArgs e)
        {
            picture = "balloon";
            //ボタンを有効にする

            pictureBox_buttonUp.Visible = false;
            pictureBox_buttonRight.Visible = false;
            pictureBox_buttonDown.Visible = false;
            pictureBox_buttonLeft.Visible = false;
            pictureBox_upperRight.Visible = true;
            pictureBox_lowerRight.Visible = true;
            pictureBox_lowerLeft.Visible = true;
            pictureBox_upperLeft.Visible = true;


        }

        private void button_plane_Click(object sender, EventArgs e)
        {
            picture = "plane";

            //ボタンを有効にする

            pictureBox_buttonUp.Visible = true;
            pictureBox_buttonRight.Visible = true;
            pictureBox_buttonDown.Visible = true;
            pictureBox_buttonLeft.Visible = true;
            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;


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
            pictureBox_buttonUp.Image= RotateImage(original, 0f);
            pictureBox_upperRight.Image = RotateImage(original, 45f);
            pictureBox_buttonRight.Image = RotateImage(original, 90f);
            pictureBox_lowerRight.Image = RotateImage(original, 135f);
            pictureBox_buttonDown.Image = RotateImage(original, 180f);
            pictureBox_lowerLeft.Image = RotateImage(original, 225f);
            pictureBox_buttonLeft.Image = RotateImage(original, 270f);
            pictureBox_upperLeft.Image = RotateImage(original, 315f);
        }




        #endregion

        
        
    }
}
