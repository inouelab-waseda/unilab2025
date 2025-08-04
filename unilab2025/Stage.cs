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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using System.Runtime.Remoting.Lifetime;
using static unilab2025.Stage;


namespace unilab2025
{
    public partial class Stage : Form
    {

        public Stage()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.DoubleBuffered = true;
            pictureBox_Conv = Func.CreatePictureBox_Conv(this);
            pictureBox_Conv.Click += new EventHandler(pictureBox_Conv_Click);            
            pictureBox_Conv.Visible = false;

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Arrow_KeyDown);
            this.KeyDown += new KeyEventHandler(Meteo_KeyDown);
            this.KeyDown += new KeyEventHandler(Penguin_KeyDown);
            this.KeyDown += new KeyEventHandler(UFO_KeyDown);
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

            button_meteo.Visible = false;


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

            Func.OnNextHintCommand = () => { _hintCount++; };
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


        Image character_me = Dictionaries.Img_DotPic["正面"];
        public static string SelectCharacter;

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
        //public static bool isEndfor = true; //forの最後が存在するか→ない場合はError
        public static int AllCount = 0;


        public static int count = 0; //試行回数カウント
        public static int miss_count = 0; //ミスカウント

        public static int count_walk = 0; //歩数カウント

        public static List<int[]> move;  //プレイヤーの移動指示を入れるリスト
        public static List<string> Input_arrow = new List<string>();
        

        //listBoxに入れられる行数の制限
        public static int limit_LB_Input = 10;
        public static int limit_LB_walk=10;
        public static int limit_LB_car=10;
        public static int limit_LB_car_Input = 10;
        public static int limit_LB_plane=10;
        public static int limit_LB_balloon = 10;
        
        public static int walk_Count = 0;
        public static int car_Count = 0;        
        public static int plane_Count = 0;
        public static int balloon_Count = 0;
        

        public static string hint;
        public static string hint_character;
        public static string hint_name;
        public static bool hint_on;

        public static string grade;    //学年
        public static int gradenum;
        public static int car_count=0;
        public static bool car_finish = true;
        public static List<string> list_car;

        public static string lockedCarPattern = null;

        private Random rand = new Random(); // フィールドに保持
        public static int meteorTargetX, meteorTargetY;//隕石処理
        public static int meteorX, meteorY;
        public static int meteorX_speed, meteorY_speed;
        System.Windows.Forms.Timer meteorTimer = new System.Windows.Forms.Timer();

        //会話文
        PictureBox pictureBox_Conv;
        byte[] Capt;
        List<Conversation> Message;
        bool isMessageMode;
        public List<Conversation> currentConversation;
        public List<Conversation> infoConversation;
        private int _hintCount = 0;

        public Graphics g1;
        public Graphics g2;

        Panel returnMapPanel;

        //パンダ設定
        public List<bool> sasa;
        public static bool panda;
        public class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        List<Coordinate> sasa_place;   
        public static int x_panda; 
        public static int y_panda; 


        //ペンギン設定
        public static Dictionary<string, Image> Img_Penguin = new Dictionary<string, Image>();
        public static bool Penguin = false;
        public static int x_penguin; //現在位置ｘ
        public static int y_penguin; //現在位置 y



        //表示される絵文字
        public static Dictionary<string, string> Emoji = new Dictionary<string, string>()
        {
            //絵文字の追加
            { "walk", "🚶‍" },
            { "car", "🚗" },
            { "balloon" ,"🎈" },
            { "plane","✈️" },
            { "penguin","🐧" }

        };
        public static string picture;
        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            Penguin = false;
            if (MainCharacter.isBoy)
            {
                button_walk.BackgroundImage = Dictionaries.Img_Button["walk_on_boy"];
                SelectCharacter = "boy";
            }
            else if (MainCharacter.isGirl)
            {
                button_walk.BackgroundImage = Dictionaries.Img_Button["walk_on_girl"];
                SelectCharacter = "girl";
            }
            else
            {
                button_walk.BackgroundImage = Dictionaries.Img_Button["walk_on_silver"];
                SelectCharacter = "silver";
            }
            lockedCarPattern = null;
            this.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber+".2"];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
           //パンダの設定
            sasa = new List<bool>();
            sasa_place = new List<Coordinate>();
            panda = false;

            map = CreateStage(stageName); //ステージ作成

            button_carEnter.BackgroundImage= Dictionaries.Img_Button["入れるoff"];
            

            InputListBox = listBox_Order;
            ListBoxes.Add(listBox_Order);
            ListBoxes.Add(listBox_Car);            
            listBox_Order.Focus();
            ShowListBox();
            grade = Regex.Replace(stageName, @"[^0-9]", "");
            int chapter_num = int.Parse(grade) / 10;

            string convFileName = "Conv_stage" + _worldNumber + "-" + _level + ".csv";
            if (File.Exists($"Conversation\\{convFileName}"))
            {
                // Func.LoadStoriesは最初のセグメントを返すように変更したため、戻り値を直接currentConversationに格納
                currentConversation = Func.LoadStories(convFileName,this, "※");

                isMessageMode = false;
            }

            string[] files = Directory.GetFiles(@"Image\\DotPic\\Vehicle");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_", "");
                Dictionaries.Img_DotPic[key] = Image.FromFile(file);
            }          


            // それぞれの枠の高さ
            int height_LB_walk = 10;
            int height_LB_car = 10;
            int height_LB_car_Input = 10;
            int height_LB_plane = 10;
            int height_LB_balloon = 10;

            using (StreamReader sr = new StreamReader($"stage_frame.csv"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');


                    if (values[0] == stageName)
                    {
                        limit_LB_walk = int.Parse(values[1]);
                        limit_LB_car = int.Parse(values[2]);
                        limit_LB_car_Input = int.Parse(values[3]);
                        limit_LB_plane = int.Parse(values[4]);
                        limit_LB_balloon = int.Parse(values[5]);
                        break;
                    }
                }
            }
            label_Walk.Text = $"{limit_LB_walk}";
            label_Car.Text = $"{limit_LB_car}";
            label_car_Input.Text = $"{limit_LB_car_Input}";
            label_Plane.Text = $"{limit_LB_plane}";
            label_Balloon.Text = $"{limit_LB_balloon}";            
            walk_Count = 0;
            car_Count = 0;            
            plane_Count = 0;
            balloon_Count = 0;

            if (_worldNumber == 1&& !(ClearCheck.IsCleared[_worldNumber, _level])) button_return.Visible = false;
            
            picture = "walk";

            if (limit_LB_walk == 0)
            {
                picture = "car";
                if (MainCharacter.isBoy)
                {
                    button_walk.BackgroundImage = Dictionaries.Img_Button["walk_lock_boy"];
                }
                else if (MainCharacter.isGirl)
                {
                    button_walk.BackgroundImage = Dictionaries.Img_Button["walk_lock_girl"];
                }
                else
                {
                    button_walk.BackgroundImage = Dictionaries.Img_Button["walk_lock_silver"];
                }                
                button_walk.Enabled = false;
                
                          
            }
            if (limit_LB_car == 0)
            {
                if (picture == "car") picture = "plane";
                if (_worldNumber==1)
                {
                    button_car.Visible = false;
                    label_Car.Visible = false;
                    button_carEnter.Visible = false;
                    label_car_Input.Visible = false;
                    listBox_Car.Visible = false;
                    pictureBox_Car.Visible = false;
                    pictureBox_car2.Visible = false;
                    pictureBox_car_enter.Visible = false;
                }
                else
                {
                    button_car.BackgroundImage = Dictionaries.Img_Button["car_lock"];
                    button_car.Enabled = false;
                    button_carEnter.Visible = false;
                    label_car_Input.Visible = false;
                    listBox_Car.Visible = false;
                    pictureBox_Car.Visible = false;                    
                    pictureBox_car_enter.Visible = false;
                }
            }
            if (limit_LB_plane == 0)
            {
                if (picture == "plane")
                {
                    picture = "balloon";
                    
                }
                if (_worldNumber <3)
                {
                    button_plane.Visible = false;
                    label_Plane.Visible = false;
                    pictureBox_plane.Visible = false;
                }
                else
                {
                    button_plane.BackgroundImage = Dictionaries.Img_Button["plane_lock"];
                    button_plane.Enabled = false;
                }
            }
            if (limit_LB_balloon == 0)
            {
                if (_worldNumber < 4)
                {
                    button_balloon.Visible = false;
                    label_Balloon.Visible = false;
                    pictureBox_balloon.Visible = false;
                }
                else
                {
                    button_balloon.BackgroundImage = Dictionaries.Img_Button["balloon_lock"];
                    label_Balloon.Enabled = false;
                }
                
            }

            if (picture == "car")
            {
                InputListBox = listBox_Car;
                listBox_Car.Focus();
                ShowListBox();
            }

            if (_worldNumber > 1 && _worldNumber < 5 && _level == 1) { 
                
                pictureBox_buttonUp.Enabled = false;
                pictureBox_buttonRight.Enabled = false;
                pictureBox_buttonDown.Enabled = false;
                pictureBox_buttonLeft.Enabled = false;
                pictureBox_upperRight.Enabled = false;
                pictureBox_lowerRight.Enabled = false;
                pictureBox_lowerLeft.Enabled = false;
                pictureBox_upperLeft.Enabled = false;
            }
            else 
            {
                UpdateMovementButtonImages();
            }


            AllCount = limit_LB_walk + limit_LB_car + limit_LB_plane + limit_LB_balloon;
            listBox_Order.Height = AllCount * listBox_Order.ItemHeight + 20;
            listBox_Car.Height = limit_LB_car_Input * listBox_Car.ItemHeight+20;

            ClearCheck.IsButtonEnabled[1,1] = true;

            //ヒントのボタン
            if (_worldNumber == 1 || _level == 1) button_hint.Visible = false;

            // 開始時の会話があれば、再生を開始する
            if (currentConversation != null && currentConversation.Count > 0)
            {
                await Task.Delay((int)ConstNum.waitTime_Load + 500);
                Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
            }


            //ペンギンの設定
            Img_Penguin.Clear();
            files = Directory.GetFiles(@"Image\\DotPic\\Penguin");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Penguin_", "");
                Img_Penguin[key] = Image.FromFile(file);
            }

            
            

            hint_on = false;

            // 説明文切り替え
            if (_level == 1 && (_worldNumber == 3 || _worldNumber == 4))
            {
                infoConversation = Dictionaries.Conversations["Info_stage" + _worldNumber + "-1"];
            }
            else if (_worldNumber < 4)
            {
                infoConversation = Dictionaries.Conversations["Info_stage" + _worldNumber];
            }
            else
            {
                infoConversation = Dictionaries.Conversations["Info_stage" + _worldNumber];
            }

            CreateReturnMapUI();

        }


        #region 各コントロール機能設定

        
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

            g2.Clear(Color.Transparent);
            // キャラクターの描画をループの外に出す
            cell_length = pictureBox_Map2.Width / map_width;

            Random rand = new Random();
            for (int y = 0; y < map_width; y++)
            {
                for (int x = 0; x < map_width; x++)
                {
                    int placeX = x * cell_length;
                    int placeY = y * cell_length;
                    if ((_worldNumber < 5 && _level == 3 && map[x, y] == 1) || (_worldNumber == 1 && _level == 2 && map[x, y] == 1))
                    {

                        g1.DrawImage(Dictionaries.Img_Object[(map[x, y] + 100).ToString()], placeX, placeY, cell_length, cell_length);

                    }
                    else if (_worldNumber < 5 || _worldNumber == 7) 
                    {
                        if (map[x, y] == 2)
                        {
                            int num = rand.Next(0, 10);
                            int num2;
                            if (num <= 3) num2 = 0;
                            else num2 = 1;
                            g1.DrawImage(Dictionaries.Img_Object["grass" + num2], placeX, placeY, cell_length, cell_length);
                        }
                        else g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                        // g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                    }
                    else
                    {
                        switch (_worldNumber)
                        {
                            case 5:
                                if (map[x, y] == 4 || map[x, y] == 5) g1.DrawImage(Dictionaries.Img_Object[3.ToString()], placeX, placeY, cell_length, cell_length);
                                else if (map[x, y] == 2)
                                {
                                    int num = rand.Next(0, 10);
                                    int num2;
                                    if (num <= 3) num2 = 0;
                                    else num2 = 1;
                                    g1.DrawImage(Dictionaries.Img_Object["grass" + num2], placeX, placeY, cell_length, cell_length);
                                }
                                else g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                                if (map[x, y] == 4)
                                {
                                    g2.DrawImage(Dictionaries.Img_DotPic["sasa"], x * cell_length - extra_length, y * cell_length - extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                                    sasa_place.Add(new Coordinate(x, y));
                                    sasa.Add(false);
                                }
                                else if (map[x, y] == 5)
                                {
                                    g2.DrawImage(Dictionaries.Img_DotPic["Panda1"], x * cell_length - extra_length, y * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                                    
                                    x_panda = x;
                                    y_panda = y;
                                }
                                break;

                            case 6:
                                if (map[x, y] < 2) g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                                else if (map[x, y] == 2)
                                {
                                    int num = rand.Next(0, 10);
                                    int num2;
                                    if (num <= 3) num2 = 0;
                                    else num2 = 1;
                                    g1.DrawImage(Dictionaries.Img_Object["ocean" + num2], placeX, placeY, cell_length, cell_length);
                                }
                                else g1.DrawImage(Dictionaries.Img_Object[(map[x, y] + 100).ToString()], placeX, placeY, cell_length, cell_length);
                                break;

                            case 8:
                                if (map[x, y] < 2) g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                                else g1.DrawImage(Dictionaries.Img_Object[(map[x, y] + 200).ToString()], placeX, placeY, cell_length, cell_length);
                                break;
                        }
                    }   
                }
            }
            

            character_me = Dictionaries.Img_DotPic["正面"];
            if (Penguin == true) character_me = Img_Penguin["正面"];           
            
            g2.DrawImage(character_me, x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        
            this.Invoke((MethodInvoker)delegate
            {
                // pictureBox_Map2を同期的にRefreshする
                pictureBox_Map1.Refresh();
                pictureBox_Map2.Refresh();
            });
            return map;
        }
       
        private void listBox_Order_Click(object sender, EventArgs e)
        {
            if (InputListBox == listBox_Car)
            {

                pictureBox_buttonUp.Visible = true;
                pictureBox_buttonRight.Visible = true;
                pictureBox_buttonDown.Visible = true;
                pictureBox_buttonLeft.Visible = true;
                pictureBox_upperRight.Visible = false;
                pictureBox_lowerRight.Visible = false;
                pictureBox_lowerLeft.Visible = false;
                pictureBox_upperLeft.Visible = false;

                if (limit_LB_walk!=0) button_walk.Enabled = true;
                if (limit_LB_car != 0) button_car.Enabled = true;
                if (limit_LB_plane != 0) button_plane.Enabled = true;
                if (limit_LB_balloon != 0) button_balloon.Enabled = true;

                if (limit_LB_walk != 0) picture = "walk";
                if (limit_LB_plane != 0 && picture == "car") picture = "plane";
                if (limit_LB_balloon != 0 && picture == "car") picture = "balloon";
                InputListBox = listBox_Order;
                listBox_Order.Focus();
                ShowListBox();
                UpdateMovementButtonImages();
            }
            else return;
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
                     

            picture = "car";
            UpdateMovementButtonImages();
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

                if (InputListBox == listBox_Order)
                {
                    if (InputListBox.SelectedItem.ToString().Contains("🚶‍"))
                    {
                        walk_Count -= 1;
                    }
                    else if (InputListBox.SelectedItem.ToString().Contains("🚗"))
                    {
                        car_Count -= 1;
                    }
                    else if (InputListBox.SelectedItem.ToString().Contains("✈️"))
                    {
                        plane_Count -= 1;
                    }
                    else if (InputListBox.SelectedItem.ToString().Contains("🎈"))
                    {
                        balloon_Count -= 1;
                    }
                    else if (InputListBox.SelectedItem.ToString().Contains("🐧"))
                    {
                        walk_Count -= 1;
                    }

                }
                
                InputListBox.Items.RemoveAt(InputListBox.SelectedIndex);//1つ消す
                if (InputListBox == listBox_Order)
                {
                    label_Walk.Text = $"{limit_LB_walk - walk_Count}";
                    label_Car.Text = $"{limit_LB_car - car_Count}";
                    label_Plane.Text = $"{limit_LB_plane - plane_Count}";
                    label_Balloon.Text = $"{limit_LB_balloon - balloon_Count}";
                }
                else label_car_Input.Text = $"{limit_LB_car_Input- listBox_Car.Items.Count}";


                if (car_Count == 0) lockedCarPattern = null;

            }
            else
            {
                if (InputListBox.Items.Count < 1) return;
                if (InputListBox == listBox_Order)
                {
                    if (InputListBox.Items[InputListBox.Items.Count - 1].ToString().Contains("🚶‍"))
                    {
                        walk_Count -= 1;
                    }
                    else if (InputListBox.Items[InputListBox.Items.Count - 1].ToString().Contains("🚗"))
                    {
                        car_Count -= 1;
                    }
                    else if (InputListBox.Items[InputListBox.Items.Count - 1].ToString().Contains("✈️"))
                    {
                        plane_Count -= 1;
                    }
                    else if (InputListBox.Items[InputListBox.Items.Count - 1].ToString().Contains("🎈"))
                    {
                        balloon_Count -= 1;
                    }
                    else if (InputListBox.Items[InputListBox.Items.Count - 1].ToString().Contains("🐧"))
                    {
                        walk_Count -= 1;
                    }

                }
                InputListBox.Items.RemoveAt(InputListBox.Items.Count - 1);//1つ消す
                if (InputListBox == listBox_Order)
                {
                    label_Walk.Text = $"{limit_LB_walk - walk_Count}";
                    label_Car.Text = $"{limit_LB_car - car_Count}";
                    label_Plane.Text = $"{limit_LB_plane - plane_Count}";
                    label_Balloon.Text = $"{limit_LB_balloon - balloon_Count}";
                }
                else label_car_Input.Text = $"{limit_LB_car_Input - listBox_Car.Items.Count}";
                if (car_Count == 0) lockedCarPattern = null;

            }
            InputListBox.Focus();
        }

        private void button_reset_Click(object sender, EventArgs e)
        {


            InputListBox.Items.Clear();//全て消す
            if (InputListBox == listBox_Order)
            {
                walk_Count = 0;
                car_Count = 0;
                plane_Count = 0;
                balloon_Count = 0;
                lockedCarPattern = null;
            }
            Left_Availabel_Input();
            InputListBox.Focus();
        }

        private async void DisplayMessage(string type)
        {
            if (Func.IsInputLocked) return;

            // Conv_Message.csvから読み込んだ辞書に、指定されたキーが存在するか確認
            if (Dictionaries.Messages.ContainsKey(type))
            {
                isMessageMode = true;

                // メッセージのテキストを取得
                string messageText = Dictionaries.Messages[type][0].Dialogue;

                // 新しいメソッドでシステムメッセージを再生
                await Func.PlaySystemMessage(this, pictureBox_Conv, messageText);
            }
        }



        #endregion

        #region ボタン押下時処理
        private async void button_Start_Click(object sender, EventArgs e)
        {
            Func.IsInputLocked = true; // キーボード入力をロック
            pictureBox_buttonUp.Enabled = false;
            pictureBox_buttonRight.Enabled = false;
            pictureBox_buttonDown.Enabled = false;
            pictureBox_buttonLeft.Enabled = false;
            pictureBox_upperRight.Enabled = false;
            pictureBox_lowerRight.Enabled = false;
            pictureBox_lowerLeft.Enabled = false;
            pictureBox_upperLeft.Enabled = false;
            listBox_Order.Enabled = false;
            listBox_Car.Enabled = false;
            button_back.Enabled = false;
            button_balloon.Enabled = false;
            button_car.Enabled = false;
            button_carEnter.Enabled = false;
            button_hint.Enabled = false;
            button_info.Enabled = false;
            button_plane.Enabled = false;
            button_reset.Enabled = false;
            button_walk.Enabled = false;
            button_return.Enabled = false;
            button_Start.Visible = false;
            button_Start.Enabled = false;
            
            if (listBox_Order.Items.Count == 0) {
                //MessageBox.Show("やり直し");
                DisplayMessage("ゴール未達");
                Func.IsInputLocked = false;
                pictureBox_buttonUp.Enabled = true;
                pictureBox_buttonRight.Enabled = true;
                pictureBox_buttonDown.Enabled = true;
                pictureBox_buttonLeft.Enabled = true;
                pictureBox_upperRight.Enabled = true;
                pictureBox_lowerRight.Enabled = true;
                pictureBox_lowerLeft.Enabled = true;
                pictureBox_upperLeft.Enabled = true;
                listBox_Order.Enabled = true;
                listBox_Car.Enabled = true;
                button_back.Enabled = true;
                button_balloon.Enabled = true;
                button_car.Enabled = true;
                button_carEnter.Enabled = true;
                button_hint.Enabled = true;
                button_info.Enabled = true;
                button_plane.Enabled = true;
                button_reset.Enabled = true;
                button_walk.Enabled = true;
                button_return.Enabled = true;
                button_Start.Visible = true;
                button_Start.Enabled = true;
                
                return;
            }

            if (hint_on)
            {
                CreateStage(stageName);
                hint_on= false;
            }
            move = Movement(); //ユーザーの入力を読み取る
            //List<string> Input_Main = 

            await SquareMovement(x_now, y_now, map, move); //キャラ動かす

            if (x_goal == x_now && y_goal == y_now)
            {
                ClearCheck.IsCleared[_worldNumber, _level] = true;    //クリア状況管理
                button_return.Visible = true;
                button_Start.Enabled = false;

                if (_level == 3)
                {
                    ClearCheck.IsCleared[_worldNumber, 0] = true;
                    switch (_worldNumber)
                    {
                        case 2:
                        case 3:
                        case 4:
                            for (int j = 0; j <= 1; j++)
                            {
                                ClearCheck.IsButtonEnabled[_worldNumber + 1, j] = true;
                                if (!ClearCheck.IsCleared[_worldNumber+1,j]) ClearCheck.IsNew[_worldNumber + 1, j] = true;
                            }
                            break; ;
                    }
                }
                else if (_level == 2 && _worldNumber == 1)
                {
                    ClearCheck.IsCleared[_worldNumber, 0] = true;
                    for (int j = 0; j <= 1; j++)
                    {
                        ClearCheck.IsButtonEnabled[_worldNumber + 1, j] = true;
                        if (!ClearCheck.IsCleared[_worldNumber + 1, j]) ClearCheck.IsNew[_worldNumber + 1, j] = true;
                    }
                }
                else
                {
                    ClearCheck.IsButtonEnabled[_worldNumber, _level + 1] = true;
                    if (!ClearCheck.IsCleared[_worldNumber, _level + 1]) ClearCheck.IsNew[_worldNumber, _level + 1] = true;
                    Func.UpdateIsNew();
                }
                button_return.Enabled = true;

            }
            

            //会話再生用
            if (Func.WaitingForButton == "start")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                }
            }
            InputListBox.Focus();
        }

        void Left_Availabel_Input()
        {
            if (InputListBox.Items.Count != 0&& InputListBox==listBox_Order)
            {
                switch (picture)
                {
                    case "walk":
                        walk_Count += 1;
                        break;                    
                    case "plane":
                        plane_Count += 1;
                        break;
                    case "balloon":
                        balloon_Count += 1;
                        break;
                    case "penguin":
                        walk_Count += 1;
                        break;
                    default:
                        // どのcaseにも当てはまらないときの処理
                        break;
                }
            }
            if (InputListBox == listBox_Order) 
            { 
                label_Walk.Text = $"{limit_LB_walk - walk_Count}";
                label_Car.Text = $"{limit_LB_car - car_Count}";
                label_Plane.Text = $"{limit_LB_plane - plane_Count}";
                label_Balloon.Text = $"{limit_LB_balloon - balloon_Count}";
            }
            else label_car_Input.Text = $"{limit_LB_car_Input - listBox_Car.Items.Count}";


        }


        private async void PictureBox_buttonUp_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↑");
            //if (isChange) Item_Change();            
            Left_Availabel_Input();
            InputListBox.Focus();

            //会話再生用
            if (Func.WaitingForButton == "up")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                }
            }
        }
        private void PictureBox_upperRight_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↗");
            Left_Availabel_Input();
            InputListBox.Focus();
        }
        private async void PictureBox_buttonRight_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "→");
            Left_Availabel_Input();
            InputListBox.Focus();

            //会話再生用
            if (Func.WaitingForButton == "right")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);

                }
            }
        }
        private void PictureBox_lowerRight_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↘");
            Left_Availabel_Input();
        }
        private async void PictureBox_buttonDown_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↓");
            Left_Availabel_Input();
            InputListBox.Focus();

            //会話再生用
            if (Func.WaitingForButton == "down")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                    
                }
                else
                {
                    Func.Commond_Action(this);
                }
            }
        }
        private void PictureBox_lowerLeft_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↙");
            Left_Availabel_Input();
        }
        private async void PictureBox_buttonLeft_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "←");
            Left_Availabel_Input();
            InputListBox.Focus();

            //会話再生用
            if (Func.WaitingForButton == "left")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                }
            }
        }
        private void PictureBox_upperLeft_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↖");
            Left_Availabel_Input();
            InputListBox.Focus();
        }

        bool Input_check()
        {
            bool result = false;
            if (InputListBox == listBox_Order)
            {
                switch (picture)
                {
                    case "walk":
                        if (walk_Count < limit_LB_walk) break;
                        else goto default;                    
                    case "plane":
                        if (plane_Count < limit_LB_plane) break;
                        else goto default;
                    case "balloon":
                        if (balloon_Count < limit_LB_balloon) break;
                        else goto default;
                    case "penguin":
                        if (walk_Count < limit_LB_walk) break;
                        else goto default;
                    default:
                        //MessageBox.Show("これ以上入力できないよ！");
                        //label_Info.Text = "これ以上入力できないよ！";
                        //label_Info.Visible = true;
                        DisplayMessage("手数オーバー");
                        result = true;
                        break;
                }
            }
            else
            {
                if (!(listBox_Car.Items.Count < limit_LB_car_Input))                
                {
                    //MessageBox.Show("これ以上入力できないよ！");
                    //label_Info.Text = "これ以上入力できないよ！";
                    //label_Info.Visible = true;
                    DisplayMessage("手数オーバー");
                    result = true;                    
                }
            }          

            return result;
        }



        private void button_walk_Click(object sender, EventArgs e)
        {
            
            picture = "walk";
            InputListBox = listBox_Order;
            listBox_Order.Focus();
            ShowListBox();
            UpdateMovementButtonImages();

            Enable();
            //ボタンを有効にする            
            pictureBox_buttonUp.Visible = true;
            pictureBox_buttonRight.Visible = true;
            pictureBox_buttonDown.Visible = true;
            pictureBox_buttonLeft.Visible = true;
            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;
            if (Penguin) picture = "penguin";
            //this.ActiveControl = null;
        }

        private async void button_car_Click(object sender, EventArgs e)
        {
            picture = "car";
            InputListBox = listBox_Car;
            listBox_Car.Focus();
            ShowListBox();
            UpdateMovementButtonImages();
            Enable();
            //ボタンを有効にする
            pictureBox_buttonUp.Visible = true;
            pictureBox_buttonRight.Visible = true;
            pictureBox_buttonDown.Visible = true;
            pictureBox_buttonLeft.Visible = true;
            pictureBox_upperRight.Visible = false;
            pictureBox_lowerRight.Visible = false;
            pictureBox_lowerLeft.Visible = false;
            pictureBox_upperLeft.Visible = false;
            

            //会話再生用
            if (Func.WaitingForButton == "car")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                }
            }
        }

        private void Enable()
        {
            pictureBox_buttonUp.Enabled = true;
            pictureBox_buttonRight.Enabled = true;
            pictureBox_buttonDown.Enabled = true;
            pictureBox_buttonLeft.Enabled = true;
            pictureBox_upperRight.Enabled = true;
            pictureBox_lowerRight.Enabled = true;
            pictureBox_lowerLeft.Enabled = true;
            pictureBox_upperLeft.Enabled = true;
        }
        private async void button_carEnter_Click(object sender, EventArgs e)
        {
            
            if (!(car_Count < limit_LB_car))
            {
                //MessageBox.Show("これ以上入力できないよ!");
                DisplayMessage("手数オーバー");
                return;
            }
            if (listBox_Car.Items.Count < 1)
            {
                //MessageBox.Show("車の入力をしてね");
                DisplayMessage("車未入力");
                pictureBox_buttonUp.Visible = true;
                pictureBox_buttonRight.Visible = true;
                pictureBox_buttonDown.Visible = true;
                pictureBox_buttonLeft.Visible = true;
                pictureBox_upperRight.Visible = false;
                pictureBox_lowerRight.Visible = false;
                pictureBox_lowerLeft.Visible = false;
                pictureBox_upperLeft.Visible = false;

                picture = "car";
                InputListBox = listBox_Car;
                listBox_Car.Focus();
                ShowListBox();
                return;
            }

            string combined = "";
            foreach (var item in listBox_Car.Items)
            {
                string text = item.ToString();
                string emoji = text.Substring(text.Length - 2, 2);
                combined += emoji;
            }

            // まだ車のルートが登録されていない（ロックされていない）場合
            if (lockedCarPattern == null)
            {
                // 現在のパターンを最初のルートとして登録（ロック）
                lockedCarPattern = combined;
            }
            // 登録済みのルートと異なるパターンが入力された場合
            else if (lockedCarPattern != combined)
            {
                // エラーメッセージを表示して処理を中断
                //MessageBox.Show("くるまのルートがさいしょとちがうよ！\nくるまのルートは一つにしてね！");
                DisplayMessage("車ルート変更");
                return;
            }

            InputListBox = listBox_Order;
            listBox_Order.Focus();
            ShowListBox();

            listBox_Order.Items.Add("🚗 (" + combined + ")");
            car_Count += 1;
            label_Walk.Text = $"{limit_LB_walk - walk_Count}";
            label_Car.Text = $"{limit_LB_car - car_Count}";
            label_Plane.Text = $"{limit_LB_plane - plane_Count}";
            label_Balloon.Text = $"{limit_LB_balloon - balloon_Count}";

            picture = "walk";
            if (limit_LB_walk != 0)
            {
                
                UpdateMovementButtonImages();
                pictureBox_buttonUp.Visible = true;
                pictureBox_buttonRight.Visible = true;
                pictureBox_buttonDown.Visible = true;
                pictureBox_buttonLeft.Visible = true;
                pictureBox_upperRight.Visible = false;
                pictureBox_lowerRight.Visible = false;
                pictureBox_lowerLeft.Visible = false;
                pictureBox_upperLeft.Visible = false;
                if (Penguin) picture = "penguin";
            }
            //会話再生用
            if (Func.WaitingForButton == "carEnter")
            {
                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                }
            }

            //会話再生用
            if (Func.WaitingForButton == "carEnterWait")
            {
                // スクショ表示
                this.pictureBox_tutorialCapt.Visible = true;

                // 次の会話セグメントを取得して再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);

                    while (pictureBox_Conv.Visible)
                    {
                        await Task.Delay(50);
                    }
                }

                // スクショ非表示
                this.pictureBox_tutorialCapt.Visible = false;
            }
        }
        private void button_carEnter_MouseEnter(object sender, EventArgs e)
        {
            // マウスが上に乗ったら画像を切り替える
            button_carEnter.BackgroundImage = Dictionaries.Img_Button["入れるon"];
        }

        private void button_carEnter_MouseLeave(object sender, EventArgs e)
        {
            // マウスが離れたら元に戻す
            button_carEnter.BackgroundImage = Dictionaries.Img_Button["入れるoff"];
        }



        private void button_balloon_Click(object sender, EventArgs e)
        {
            picture = "balloon";
            InputListBox = listBox_Order;
            listBox_Order.Focus();
            ShowListBox();
            UpdateMovementButtonImages();
            //ボタンを有効にする
            Enable();
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
            InputListBox = listBox_Order;
            listBox_Order.Focus();
            ShowListBox();
            UpdateMovementButtonImages();

            Enable();
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

        //マップに戻る
        private void button_return_Click(object sender, EventArgs e)
        {
            if (!ClearCheck.IsCleared[_worldNumber, _level])
            {
                string message = "にゅうりょくしたないようがリセットされちゃうよ！\nほんとうにマップにもどりますか？";
                string caption = "確認";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result;

                // 確認ダイアログを表示します。
                result = MessageBox.Show(this, message, caption, buttons, MessageBoxIcon.Warning);

                // ユーザーが「はい」を押した場合のみ、マップ選択画面に戻ります。
                if (result == DialogResult.Yes)
                {
                    Func.CreateStageSelect(this, _worldName, _worldNumber);
                }
            }
            else Func.CreateStageSelect(this, _worldName, _worldNumber);
        }

        //ヒント
        private async void button_hint_Click(object sender, EventArgs e)
        {
            hint_on = true;
            if (stageName=="stage3-2"|| stageName == "stage3-3" || stageName == "stage4-2" || stageName == "stage4-3" || stageName == "stage6-2" || stageName == "stage6-3")
            CreateStage(stageName + "hint");

            currentConversation = Dictionaries.Conversations[stageName + "hint"];
            if (currentConversation != null && currentConversation.Count > 0)
            {
                Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
            }
            InputListBox.Focus();
            ShowListBox();
        }

        /// <summary>
        /// Updates the images of movement-type buttons based on the selected 'picture'.
        /// </summary>
        private void UpdateMovementButtonImages()
        {
            // Reset all buttons to their "off" state
            if(limit_LB_walk == 0) button_walk.BackgroundImage = Dictionaries.Img_Button["walk_lock_" + SelectCharacter];
            else button_walk.BackgroundImage = Dictionaries.Img_Button["walk_off_"+ SelectCharacter];
            if (limit_LB_car == 0) button_car.BackgroundImage = Dictionaries.Img_Button["car_lock"];
            else button_car.BackgroundImage = Dictionaries.Img_Button["car_off"];
            if (limit_LB_plane == 0) button_plane.BackgroundImage = Dictionaries.Img_Button["plane_lock"];
            else button_plane.BackgroundImage = Dictionaries.Img_Button["plane_off"];
            if (limit_LB_balloon == 0) button_balloon.BackgroundImage = Dictionaries.Img_Button["balloon_lock"];
            else button_balloon.BackgroundImage = Dictionaries.Img_Button["balloon_off"];
            
            // Set the selected button to its "on" state
            switch (picture)
            {
                case "walk":
                    button_walk.BackgroundImage = Dictionaries.Img_Button["walk_on_"+ SelectCharacter];
                    break;
                case "car":
                    button_car.BackgroundImage = Dictionaries.Img_Button["car_on"];
                    break;
                case "plane":
                    button_plane.BackgroundImage = Dictionaries.Img_Button["plane_on"];
                    break;
                case "balloon":
                    button_balloon.BackgroundImage = Dictionaries.Img_Button["balloon_on"];
                    break;
            }
        }

        private async void Arrow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Func.IsInputLocked) return;

            if (picture != "balloon")            {
                
                if (e.KeyCode == Keys.Up)
                {
                    if (Input_check()) return;
                    InputListBox.Items.Add(Emoji[picture] + "  " + "↑");
                    Left_Availabel_Input();

                    //会話再生用
                    if (Func.WaitingForButton == "up")
                    {
                        // 次の会話セグメントを取得して再生
                        currentConversation = Func.GetNextSegment(this);
                        if (currentConversation != null && currentConversation.Count > 0)
                        {
                            Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                        }
                    }
                }

                if (e.KeyCode == Keys.Right)
                {
                    if (Input_check()) return;
                    InputListBox.Items.Add(Emoji[picture] + "  " + "→");
                    Left_Availabel_Input();

                    //会話再生用
                    if (Func.WaitingForButton == "right")
                    {
                        // 次の会話セグメントを取得して再生
                        currentConversation = Func.GetNextSegment(this);
                        if (currentConversation != null && currentConversation.Count > 0)
                        {
                            Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                        }
                    }
                }

                if (e.KeyCode == Keys.Down)
                {
                    if (Input_check()) return;
                    InputListBox.Items.Add(Emoji[picture] + "  " + "↓");
                    Left_Availabel_Input();

                    //会話再生用
                    if (Func.WaitingForButton == "down")
                    {
                        // 次の会話セグメントを取得して再生
                        currentConversation = Func.GetNextSegment(this);
                        if (currentConversation != null && currentConversation.Count > 0)
                        {
                            Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                        }
                    }
                }

                if (e.KeyCode == Keys.Left)
                {
                    if (Input_check()) return;
                    InputListBox.Items.Add(Emoji[picture] + "  " + "←");
                    Left_Availabel_Input();

                    //会話再生用
                    if (Func.WaitingForButton == "left")
                    {
                        // 次の会話セグメントを取得して再生
                        currentConversation = Func.GetNextSegment(this);
                        if (currentConversation != null && currentConversation.Count > 0)
                        {
                            Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                        }
                    }
                }
            }
        }
        private async void button_info_Click(object sender, EventArgs e)
        {
            if (this.currentConversation != null && Func.convIndex < this.currentConversation.Count) return;

            if (_worldNumber == 2 && _level == 1)
            {
                infoConversation = Dictionaries.Conversations["Info_stage2-1-" + _hintCount];
            }

            currentConversation = infoConversation;
            Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
            InputListBox.Focus();
            ShowListBox();
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
            Image original = Dictionaries.Img_Button["point_on"];
            pictureBox_buttonUp.Image = RotateImage(original, 0f);
            pictureBox_buttonRight.Image = RotateImage(original, 90f);
            pictureBox_buttonDown.Image = RotateImage(original, 180f);
            pictureBox_buttonLeft.Image = RotateImage(original, 270f);
            original = Dictionaries.Img_Button["point_naname"];
            pictureBox_lowerLeft.Image = RotateImage(original, 0f);
            pictureBox_lowerRight.Image = RotateImage(original, 270f);
            pictureBox_upperLeft.Image = RotateImage(original, 90f);
            pictureBox_upperRight.Image = RotateImage(original, 180f);

        }


        //指差しボタンの動き
        private void pictureBox_finger1_VisibleChanged(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;
            if (pic != null)
            {
                if (pic.Visible)
                {
                    SetupPointerAnimation(pic);
                }
                else
                {
                    StopPointerAnimation(pic);
                }
            }
        }

        private void pictureBox_finger2_VisibleChanged(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;
            if (pic != null)
            {
                if (pic.Visible)
                {
                    SetupPointerAnimation(pic);
                }
                else
                {
                    StopPointerAnimation(pic);
                }
            }
        }

        private void pictureBox_finger3_VisibleChanged(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;
            if (pic != null)
            {
                if (pic.Visible)
                {
                    SetupPointerAnimation(pic);
                }
                else
                {
                    StopPointerAnimation(pic);
                }
            }
        }

        private void pictureBox_finger4_VisibleChanged(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;
            if (pic != null)
            {
                if (pic.Visible)
                {
                    SetupPointerAnimation(pic);
                }
                else
                {
                    StopPointerAnimation(pic);
                }
            }
        }
        private void pictureBox_finger5_VisibleChanged(object sender, EventArgs e)
        {
            var pic = sender as PictureBox;
            if (pic != null)
            {
                if (pic.Visible)
                {
                    SetupPointerAnimation(pic);
                }
                else
                {
                    StopPointerAnimation(pic);
                }
            }
        }


        private Dictionary<PictureBox, System.Windows.Forms.Timer> pointerTimers = new Dictionary<PictureBox, System.Windows.Forms.Timer>();
        private Dictionary<PictureBox, int> originalYPositions = new Dictionary<PictureBox, int>();
        private Dictionary<PictureBox, bool> movingUpMap = new Dictionary<PictureBox, bool>();

        private void SetupPointerAnimation(PictureBox name)
        {
            if (!pointerTimers.ContainsKey(name))
            {
                // 初期Y座標記録
                originalYPositions[name] = name.Top;
                movingUpMap[name] = true;

                System.Windows.Forms.Timer moveTimer = new System.Windows.Forms.Timer();
                moveTimer.Interval = 50;
                moveTimer.Tick += (s, e) => MovePointer(name);
                pointerTimers[name] = moveTimer;
            }

            pointerTimers[name].Start();
        }

        private void MovePointer(PictureBox name)
        {
            int delta = 2;

            if (!originalYPositions.ContainsKey(name)) return;

            int originalY = originalYPositions[name];
            bool movingUp = movingUpMap[name];

            if (movingUp)
            {
                name.Top -= delta;
                if (name.Top <= originalY - 5)
                    movingUpMap[name] = false;
            }
            else
            {
                name.Top += delta;
                if (name.Top >= originalY + 5)
                    movingUpMap[name] = true;
            }
        }

        private void StopPointerAnimation(PictureBox name)
        {
            if (pointerTimers.ContainsKey(name))
            {
                pointerTimers[name].Stop();
            }
        }



        #endregion

        #region 動作関連
        public void MoveTo(List<int[]> movelist, string item)    //指定方向に移動する関数
        {

            string direction = "";

            if (item.Contains("↑")) direction = "↑";
            else if (item.Contains("→")) direction = "→";
            else if (item.Contains("↓")) direction = "↓";
            else if (item.Contains("←")) direction = "←";
            else if (item.Contains("↗")) direction = "↗";
            else if (item.Contains("↘")) direction = "↘";
            else if (item.Contains("↙")) direction = "↙";
            else if (item.Contains("↖")) direction = "↖";


            int Direction_Index = 10;
            switch (direction)
            {
                case "↑":
                    Direction_Index = 0;
                    break;
                case "→":
                    Direction_Index = 1;
                    break;
                case "↓":
                    Direction_Index = 2;
                    break;
                case "←":
                    Direction_Index = 3;
                    break;
                case "↗":
                    Direction_Index = 4;
                    break;
                case "↘":
                    Direction_Index = 5;
                    break;
                case "↙":
                    Direction_Index = 6;
                    break;
                case "↖":
                    Direction_Index = 7;
                    break;


                default:
                    break;
            }
            int[][] move = new int[8][];       // up,right.down,leftの順
            move[0] = new int[] { 0, -1 };     //up
            move[1] = new int[] { 1, 0 };      //right
            move[2] = new int[] { 0, 1 };      //down 
            move[3] = new int[] { -1, 0 };     //left
            move[4] = new int[] { 1, -1 };     
            move[5] = new int[] { 1, 1 };      
            move[6] = new int[] { -1, 1 };      
            move[7] = new int[] { -1, -1 };
            if (Direction_Index < 8) movelist.Add(move[Direction_Index]);
        }

        /// <summary>
        /// 実際にユーザーの入力を読み取り、動きのListを作成
        /// </summary>
        /// <returns></returns>
        
        public List<int[]> Movement()      //動作の関数
        {
            var Move_Car = new List<int[]>();                                                           //車での動きを保存                                                                    
            string[] Get_Input_Car = this.listBox_Car.Items.Cast<string>().ToArray();                     //車のListへの入力を保存


            var Move_Car_List = new List<string>(Get_Input_Car);

            if (Get_Input_Car.Length != 0)
            {
                for (int i = 0; i < Move_Car_List.Count; i++)
                {
                    MoveTo(Move_Car, Move_Car_List[i]);
                }
            }

            string[] Get_Input_Main = this.listBox_Order.Items.Cast<string>().ToArray();
            //Get_Input_Main = exchange_move(Get_Input_Main);
            List<string> Move_Main_List = new List<string>(Get_Input_Main);
            foreach (string item in Get_Input_Main)
            {
                Input_arrow.Add(item);  // ← ここで値を追加
            }
            List<int[]> move = new List<int[]>();

            if (Get_Input_Main.Length != 0)
            {
                for (int i = 0; i < Move_Main_List.Count; i++)
                {
                    if (Move_Main_List[i].Contains("🚗")) move.AddRange(Move_Car);
                    else MoveTo(move, Move_Main_List[i]);
                }
            }
            return move;
        }

        /// <summary>
        /// 動いた先の衝突検知(入れない部分に侵入した場合)
        /// </summary>
        /// <param name="x">現在位置x座標</param>
        /// <param name="y">現在位置y座標</param>
        /// <param name="Map">ステージのマップ情報</param>
        /// <param name="move">動いた先の座標</param>
        /// <returns></returns>
        public bool Colision_detection(int x, int y, int[,] Map, List<int[]> move)
        {
            int max_x = Map.GetLength(0);
            int max_y = Map.GetLength(1);

            int new_x = x + move[0][0];
            int new_y = y + move[0][1];

            if (new_x <= 0 || (max_x - new_x) <= 1 || new_y <= 0 || (max_y - new_y) <= 1) return true;
            else if (Map[new_x, new_y] == 2) return true;
            else if(Map[new_x, new_y] == 5&& !(sasa.Contains(true))) return true;
            else
            {
                //move.RemoveAt(0);
                return false;
            }
        }

        

        /// <summary>
        /// 動く際の描画や処理を行う関数
        /// </summary>
        /// <param name="x">現在地のx座標</param>
        /// <param name="y">現在地のy座標</param>
        /// <param name="Map">ステージのマップ情報</param>
        /// <param name="move">動きのリスト</param>
        public async Task  SquareMovement(int x, int y, int[,] Map, List<int[]> move)
        {
            Graphics g2 = Graphics.FromImage(bmp2);
            //cell_length = pictureBox1.Width / 12;
            if (move.Count == 0) //ゴールについていない場合(入力がない)場合のエラー処理
            {
                //DisplayMessage("miss_end");
                return;
            }

            List<int[]> move_copy = new List<int[]>(move);
            

            int jump = 0;
            bool DoubleJump = false;
            int waittime = 250; //ミリ秒
            count_walk = 1;//何マス歩いたか、歩き差分用
            bool isWarp = false;

            (int, int) place_update(int a, int b, List<int[]> move_next)
            {
                x += move_next[0][0];
                y += move_next[0][1];
                x_now = x;
                y_now = y;
                g2.Clear(Color.Transparent);
                return (x_now, y_now);
            }

            void DrawCharacter(int a, int b, ref Image character_me)
            {
                //if (_worldNumber > 4 && (x_now != x_goal || y_now != y_goal))
                //{
                //    int placeX = x_goal * cell_length;
                //    int placeY = y_goal * cell_length;
                //    int goal = 10 + _worldNumber;
                //    g2.DrawImage(Dictionaries.Img_Object[goal.ToString()], placeX, placeY, cell_length, cell_length);
                //}
                if(car_count == 0&&Input_arrow.Count > 0 && Input_arrow[0].Contains("🚶‍")) g2.DrawImage(character_me, a * cell_length - extra_length, b * cell_length - 2 *extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                else g2.DrawImage(character_me, a * cell_length - extra_length, b * cell_length - extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
            }


            (int, int)draw_move(int a, int b, ref List<int[]> move_next)
            {
                
                (x_now, y_now) = place_update(a, b, move_next);

                //character_me = Character_Image(move_copy[0][0], move_copy[0][1], count_walk, jump, DoubleJump, character_me);
                if (Input_arrow.Count > 0)
                {
                    if (car_count==0&&Input_arrow[0].Contains("🚗"))
                    {
                        car_count = listBox_Car.Items.Count;
                        Input_arrow.RemoveAt(0);

                        list_car = listBox_Car.Items.Cast<string>().ToList();
                        car_finish = false;

                    }

                    if (car_count > 0)
                    {
                        if (list_car[0].Contains("↑")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 0f);
                        else if (list_car[0].Contains("→")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 90f);
                        else if (list_car[0].Contains("↓")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 180f);
                        else if (list_car[0].Contains("←")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 270f);
                        car_count -= 1;
                        list_car.RemoveAt(0);
                    }
                    else if (car_count == 0&& Input_arrow[0].Contains("🚶‍"))
                    {
                        if (Input_arrow[0].Contains("↑")) character_me = Dictionaries.Img_DotPic["後ろ"];
                        else if (Input_arrow[0].Contains("→")) character_me = Dictionaries.Img_DotPic["右"];
                        else if (Input_arrow[0].Contains("↓")) character_me = Dictionaries.Img_DotPic["正面"];
                        else if (Input_arrow[0].Contains("←")) character_me = Dictionaries.Img_DotPic["左"];
                    }
                    else if(car_count == 0&& Input_arrow[0].Contains("🎈"))
                    {
                        if (Input_arrow[0].Contains("↗")) character_me = RotateImage(Dictionaries.Img_DotPic["ball"], 0f);
                        else if (Input_arrow[0].Contains("↘")) character_me = RotateImage(Dictionaries.Img_DotPic["ball"], 0f);
                        else if (Input_arrow[0].Contains("↙")) character_me = RotateImage(Dictionaries.Img_DotPic["ball"], 0f);
                        else if (Input_arrow[0].Contains("↖")) character_me = RotateImage(Dictionaries.Img_DotPic["ball"], 0f);
                    }
                    else if (car_count == 0 && Input_arrow[0].Contains("✈️"))
                    {
                        if (Input_arrow[0].Contains("↑")) character_me = RotateImage(Dictionaries.Img_DotPic["plane"], 0f);
                        else if (Input_arrow[0].Contains("→")) character_me = RotateImage(Dictionaries.Img_DotPic["plane"], 90f);
                        else if (Input_arrow[0].Contains("↓")) character_me = RotateImage(Dictionaries.Img_DotPic["plane"], 180f);
                        else if (Input_arrow[0].Contains("←")) character_me = RotateImage(Dictionaries.Img_DotPic["plane"], 270f);
                           
                        
                        
                    }
                }                
                else if (Input_arrow.Count == 0&& car_count > 0)
                {
                    
                        if (list_car[0].Contains("↑")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 0f);
                        else if (list_car[0].Contains("→")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 90f);
                        else if (list_car[0].Contains("↓")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 180f);
                        else if (list_car[0].Contains("←")) character_me = RotateImage(Dictionaries.Img_DotPic["car"], 270f);
                        car_count -= 1;
                        list_car.RemoveAt(0);
                                     
                }
                if(car_count == 0 && Input_arrow.Count > 0 && Penguin == true)
                {
                    if (Input_arrow[0].Contains("↑")) character_me = Img_Penguin["後ろ"];
                    else if (Input_arrow[0].Contains("→")) character_me = Img_Penguin["右"];
                    else if (Input_arrow[0].Contains("↓")) character_me = Img_Penguin["正面"];
                    else if (Input_arrow[0].Contains("←")) character_me = Img_Penguin["左"];
                    if (Map[x_now, y_now] == 7)
                    {
                        if (character_me == Img_Penguin["右"]) character_me = Img_Penguin["滑る右"];
                        else if (character_me == Img_Penguin["左"]) character_me = Img_Penguin["滑る左"];
                    }
                }

                if (_worldNumber == 5) Panda();
                DrawCharacter(x_now, y_now, ref character_me);
                
                pictureBox_Map2.Refresh();
                
                
                //this.Invoke((MethodInvoker)delegate
                //{
                //    // pictureBox2を同期的にRefreshする
                //    //pictureBox2.Refresh();
                //});
                return (x_now, y_now);
            }
            while (true)
            {
                if (move_copy.Count == 0)//動作がすべて終了した場合
                {

                    g2.Clear(Color.Transparent);
                    Input_arrow.Clear();
                    car_finish = true;
                    car_count = 0;                    
                    if (_worldNumber == 5) Panda();
                    Image character_me = Dictionaries.Img_DotPic["正面"];
                    if(Penguin==true) character_me= Img_Penguin["正面"];
                    g2.DrawImage(character_me, x_now * cell_length - extra_length, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    pictureBox_Map2.Refresh();
                    button_Start.Visible = true;
                    button_Start.Enabled = true;
                    if (x_now != x_goal || y_now != y_goal)
                    {
                        Func.IsInputLocked = false;
                        //MessageBox.Show("やり直し");
                        DisplayMessage("ゴール未達");
                        // クリック待ち
                        while (pictureBox_Conv.Visible)
                        {
                            await Task.Delay(50); // わずかに待機
                        }

                        pictureBox_buttonUp.Enabled = true;
                        pictureBox_buttonRight.Enabled = true;
                        pictureBox_buttonDown.Enabled = true;
                        pictureBox_buttonLeft.Enabled = true;
                        pictureBox_upperRight.Enabled = true;
                        pictureBox_lowerRight.Enabled = true;
                        pictureBox_lowerLeft.Enabled = true;
                        pictureBox_upperLeft.Enabled = true;
                        listBox_Order.Enabled = true;
                        listBox_Car.Enabled = true;
                        button_back.Enabled = true;
                        button_balloon.Enabled = true;
                        button_car.Enabled = true;
                        button_carEnter.Enabled = true;
                        button_hint.Enabled = true;
                        button_info.Enabled = true;
                        button_plane.Enabled = true;
                        button_reset.Enabled = true;
                        button_walk.Enabled = true;
                        button_return.Enabled = true;
                        button_Start.Visible = true;
                        button_Start.Enabled = true;

                        g2.Clear(Color.Transparent);
                        for (int i = 0; i < sasa.Count; i++)
                        {
                            sasa[i] = false;
                        }
                       
                        panda = false;
                        if (_worldNumber == 5) Panda();
                        g2.DrawImage(character_me, x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);                        
                        pictureBox_Map2.Refresh();                        
                        x_now = x_start;
                        y_now = y_start;
                        
                    }
                    else
                    {
                        //ゴール時の処理
                        if (((_worldNumber < 5 && _level == 3 && map[x, y] == 1) || (_worldNumber == 1 && _level == 2 && map[x, y] == 1)))
                        {
                            g2.Clear(Color.Transparent);
                            Image character = Dictionaries.Img_DotPic["ゴール"];
                            if(MainCharacter.isGirl) g2.DrawImage(character, x_now * cell_length - extra_length+5, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                            else g2.DrawImage(character, x_now * cell_length - extra_length-6, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                            
                            pictureBox_Map2.Refresh();
                        }
                        Func.IsInputLocked = false;
                        //MessageBox.Show("成功");
                        DisplayMessage("ゴール");

                        // クリックまで待機
                        while (pictureBox_Conv.Visible)
                        {
                            await Task.Delay(50); // わずかに待機
                        }

                        //会話再生用
                        if (Func.WaitingForButton == "Clear")
                        {
                            // 次の会話セグメントを取得して再生
                            currentConversation = Func.GetNextSegment(this);
                            if (currentConversation != null && currentConversation.Count > 0)
                            {
                                Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                                while (pictureBox_Conv.Visible)
                                {
                                    await Task.Delay(50);
                                }
                            }

                        }

                        // クリア後マップ遷移後会話再生用
                        if (Func.WaitingForButton == "returnMap")
                        {

                            if ((_worldNumber == 1 && _level == 2) || (_worldNumber <= 4 && _level == 3))
                            {
                                ClearCheck.IsCleared[_worldNumber, _level] = true;
                                ClearCheck.IsCleared[_worldNumber, 0] = true;
                                for (int j = 0; j <= 1; j++) // 0番目はワールド全体、1番目は最初のステージ
                                {
                                    ClearCheck.IsButtonEnabled[_worldNumber + 1, j] = true;
                                    ClearCheck.IsNew[_worldNumber + 1, j] = true;
                                }
                            }
                            else if (_level == 3)
                            {
                                ClearCheck.IsCleared[_worldNumber, _level] = true;
                                ClearCheck.IsCleared[_worldNumber, 0] = true;
                                Func.UpdateIsNew();
                            }
                            else
                            {
                                ClearCheck.IsCleared[_worldNumber, _level] = true;
                                ClearCheck.IsButtonEnabled[_worldNumber, _level + 1] = true;
                                if(!ClearCheck.IsCleared[_worldNumber, _level+1]) ClearCheck.IsNew[_worldNumber, _level + 1] = true;
                            }

                            if (ClearCheck.IsCleared[5, 3] && ClearCheck.IsCleared[6, 3] && ClearCheck.IsCleared[7, 3] && ClearCheck.IsCleared[8, 3])
                            {
                                ClearCheck.Completed = true;
                            }

                            // 遷移後再生フラグ
                            CurrentFormState.NextConversationTrigger = "PLAY";


                            // カスタムパネルを表示する
                            returnMapPanel.Visible = true;
                            returnMapPanel.BringToFront();
                        }
                    }
                        //else
                        //{
                        //    g2.Clear(Color.Transparent);
                        //    //Graphics g2 = Graphics.FromImage(bmp2);
                        //    int placeX = x_goal * cell_length;
                        //    int placeY = y_goal * cell_length;
                        //    g2.DrawImage(Dictionaries.Img_DotPic["GOAL"], placeX - extra_length, placeY - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        //    this.Invoke((MethodInvoker)delegate
                        //    {
                        //        // pictureBox2を同期的にRefreshする
                        //        //pictureBox2.Refresh();
                        //    });
                        //}
                      break;
                }
                else
                {
                    //List<int[]>Direction  = new List<int[]>();

                    var Direction = move_copy;
                    
                    if (!Colision_detection(x, y, Map, move_copy))
                    {
                        (x_now, y_now) = draw_move(x, y, ref move_copy);

                        if (Map[x, y] == 8)
                        {
                            for (int i = 0; i < map_width; i++)
                            {
                                for (int j = 0; j < map_width; j++)
                                {
                                    if (Map[i, j] == 8 && (i != x || j != y))
                                    {
                                        await Task.Delay(400);
                                        x = i;
                                        x_now = i;
                                        y = j;
                                        y_now = j;
                                        g2.Clear(Color.Transparent);
                                        DrawCharacter(x_now, y_now, ref character_me);
                                        pictureBox_Map2.Refresh();
                                        isWarp = true;
                                    }
                                }
                                if (isWarp) break;
                            }

                        }
                        isWarp = false;
                        if (Map[x, y] == 5 && sasa.Contains(true))
                        {
                            panda = true;
                            //g2.DrawImage(Dictionaries.Img_DotPic["Panda2"], x * cell_length - extra_length, y * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        }

                        if (car_finish == true && Input_arrow.Count > 0)
                        {
                            if (Input_arrow[0].Contains("✈️"))//飛行機の処理
                            {                                
                                while (true)
                                {
                                    if (!Colision_detection(x, y, Map, Direction))
                                    {
                                        await Task.Delay(200);
                                        (x_now, y_now) = place_update(x, y, Direction);
                                        if (Map[x, y] == 4) 
                                        {
                                            if (sasa_place[0].X == x && sasa_place[0].Y == y) sasa[0] = true;
                                            else sasa[1] = true;
                                        }
                                        
                                        if (Map[x, y] == 5 && sasa.Contains(true)) panda = true;
                                        if (_worldNumber == 5) Panda();
                                        DrawCharacter(x_now, y_now, ref character_me);
                                        pictureBox_Map2.Refresh();
                                        if (Map[x, y] == 8)
                                        {
                                            for (int i = 0; i < map_width; i++)
                                            {
                                                for (int j = 0; j < map_width; j++)
                                                {
                                                    if (Map[i, j] == 8 && (i != x || j != y))
                                                    {
                                                        await Task.Delay(400);
                                                        x = i;
                                                        x_now = i;
                                                        y = j;
                                                        y_now = j;
                                                        g2.Clear(Color.Transparent);
                                                        DrawCharacter(x_now, y_now, ref character_me);
                                                        pictureBox_Map2.Refresh();
                                                        isWarp = true;
                                                    }
                                                }
                                                if (isWarp) break;
                                            }

                                        }
                                    }
                                    else break;

                                }
                                
                            }
                            Input_arrow.RemoveAt(0);
                        }
                        if (car_count == 0) car_finish = true;//車の処理が終わったかどうか

                        //笹を取ったかどうか 
                        if (Map[x, y] == 4) 
                        {
                            if (sasa_place[0].X == x && sasa_place[0].Y == y)
                            {
                                sasa[0] = true;
                            }
                            else sasa[1] = true;
                        }
                                       
                        
                        if (Map[x, y] == 7)//氷の処理
                        {                            
                            while (true)
                            {
                                if (!Colision_detection(x, y, Map, Direction))
                                {
                                    await Task.Delay(200);
                                    (x_now, y_now) = place_update(x, y, Direction);                                    
                                    DrawCharacter(x_now, y_now, ref character_me);
                                    pictureBox_Map2.Refresh();
                                    if (!(Map[x, y] == 7)) break;

                                }
                                else break;

                            }                            

                        }

                        if (Map[x, y] == 6)//ジャンプ処理
                        {
                            while (true)
                            {
                                
                                int x_jump=x_now;
                                int y_jump=y_now;
                                (x_now, y_now) = place_update(x, y, Direction);
                                if (!Colision_detection(x, y, Map, Direction))
                                {
                                    await Task.Delay(400);
                                    await Jump(x_jump, y_jump,Direction);
                                    (x_now, y_now) = place_update(x, y, Direction);
                                    DrawCharacter(x_now, y_now, ref character_me);
                                    pictureBox_Map2.Refresh();
                                    if (Map[x, y] != 6) break;
                                }
                                else
                                {
                                    Func.IsInputLocked = false;
                                    pictureBox_buttonUp.Enabled = true;
                                    pictureBox_buttonRight.Enabled = true;
                                    pictureBox_buttonDown.Enabled = true;
                                    pictureBox_buttonLeft.Enabled = true;
                                    pictureBox_upperRight.Enabled = true;
                                    pictureBox_lowerRight.Enabled = true;
                                    pictureBox_lowerLeft.Enabled = true;
                                    pictureBox_upperLeft.Enabled = true;
                                    listBox_Order.Enabled = true;
                                    listBox_Car.Enabled = true;
                                    button_back.Enabled = true;
                                    button_balloon.Enabled = true;
                                    button_car.Enabled = true;
                                    button_carEnter.Enabled = true;
                                    button_hint.Enabled = true;
                                    button_info.Enabled = true;
                                    button_plane.Enabled = true;
                                    button_reset.Enabled = true;
                                    button_walk.Enabled = true;
                                    button_return.Enabled = true;
                                    button_Start.Visible = true;
                                    button_Start.Enabled = true;
                                    //MessageBox.Show("前に進めません");
                                    DisplayMessage("行き止まり");
                                    g2.Clear(Color.Transparent);//人の移動などのリセット
                                    Input_arrow.Clear();//入力のリセット
                                    Image character_me = Dictionaries.Img_DotPic["正面"];
                                    if (Penguin == true) character_me = Img_Penguin["正面"];
                                    g2.DrawImage(character_me, x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);                                    
                                    pictureBox_Map2.Refresh();
                                    x_now = x_start;//スタート位置に戻す
                                    y_now = y_start;
                                    button_Start.Visible = true;
                                    button_Start.Enabled = true;
                                    break;
                                }
                            }
                        }




                        move_copy.RemoveAt(0); // 使い終わった移動ステップを削除
                    }
                    else
                    {
                        Func.IsInputLocked = false;
                        pictureBox_buttonUp.Enabled = true;
                        pictureBox_buttonRight.Enabled = true;
                        pictureBox_buttonDown.Enabled = true;
                        pictureBox_buttonLeft.Enabled = true;
                        pictureBox_upperRight.Enabled = true;
                        pictureBox_lowerRight.Enabled = true;
                        pictureBox_lowerLeft.Enabled = true;
                        pictureBox_upperLeft.Enabled = true;
                        listBox_Order.Enabled = true;
                        listBox_Car.Enabled = true;
                        button_back.Enabled = true;
                        button_balloon.Enabled = true;
                        button_car.Enabled = true;
                        button_carEnter.Enabled = true;
                        button_hint.Enabled = true;
                        button_info.Enabled = true;
                        button_plane.Enabled = true;
                        button_reset.Enabled = true;
                        button_walk.Enabled = true;
                        button_return.Enabled = true;
                        button_Start.Visible = true;
                        button_Start.Enabled = true;
                        //MessageBox.Show("前に進めません");
                        DisplayMessage("行き止まり");
                        while (pictureBox_Conv.Visible)

                        {

                            await Task.Delay(50); // わずかに待機

                        }

                        g2.Clear(Color.Transparent);//人の移動などのリセット
                        Input_arrow.Clear();//入力のリセット
                        car_finish = true;
                        car_count = 0;
                        for (int i = 0; i < sasa.Count; i++)
                        {
                            sasa[i] = false;
                        }
                        panda = false;
                        Image character_me = Dictionaries.Img_DotPic["正面"];
                        if (Penguin == true) character_me = Img_Penguin["正面"];
                        if (_worldNumber == 5) Panda();
                        g2.DrawImage(character_me, x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        pictureBox_Map2.Refresh();
                        x_now = x_start;//スタート位置に戻す
                        y_now = y_start;
                        button_Start.Visible = true;
                        button_Start.Enabled = true;
                        break;
                    }                 
                                        

                    await Task.Delay(400);


                }

                                
            }
        }

        
        private void Panda()
        {
            for (int i = 0; i < sasa.Count; i++)
            {
                if(!sasa[i]) g2.DrawImage(Dictionaries.Img_DotPic["sasa"], sasa_place[i].X * cell_length - extra_length, sasa_place[i].Y * cell_length - extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
            }
            
            if(!panda)g2.DrawImage(Dictionaries.Img_DotPic["Panda1"], x_panda * cell_length - extra_length, y_panda * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
            else g2.DrawImage(Dictionaries.Img_DotPic["Panda2"], x_panda * cell_length - extra_length, y_panda * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
        }
        private async Task Jump(int x_jump,int y_jump, List<int[]> Direction)
        {
            int a = 0;
            
            while(true) 
            {
                if (a > (2 * cell_length)) break;
                g2.Clear(Color.Transparent);//人の移動などのリセット
                if (Direction[0][0]==1&& Direction[0][1]==0) g2.DrawImage(Dictionaries.Img_DotPic["tazan_右"], x_jump * cell_length - extra_length+a, y_jump * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                if (Direction[0][0] == -1 && Direction[0][1] == 0) g2.DrawImage(Dictionaries.Img_DotPic["tazan_左"], x_jump * cell_length - extra_length - a, y_jump * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                if (Direction[0][0] == 0 && Direction[0][1] == -1) g2.DrawImage(Dictionaries.Img_DotPic["tazan_後ろ"], x_jump * cell_length - extra_length, y_jump * cell_length - 2 * extra_length-a, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                if (Direction[0][0] == 0 && Direction[0][1] == 1) g2.DrawImage(Dictionaries.Img_DotPic["tazan_正面"], x_jump * cell_length - extra_length, y_jump * cell_length - 2 * extra_length+a, cell_length + 2 * extra_length, cell_length + 2 * extra_length);

                pictureBox_Map2.Refresh();
                a += 30;
                await Task.Delay(100);
                
            }
           
        }




        #endregion


        #region 会話進行処理

        /// 会話を1フレーム進める
        private void AdvanceConversation()
        {
            if (currentConversation != null && Capt != null)
            {                
                Func.DrawConv(this, pictureBox_Conv, Capt, currentConversation);
            }
        }

        // 会話用のPictureBoxがクリックされたときの処理
        private void pictureBox_Conv_Click(object sender, EventArgs e)
        {
            if (isMessageMode)
            {
                // システムメッセージモードの場合、ウィンドウを閉じてフラグを戻すだけ
                Func.ChangeControl(pictureBox_Conv, false);
                isMessageMode = false;
            }
            else
            {
                // 通常の会話モードの場合、今まで通り会話を進める
                AdvanceConversation();
            }
        }

        #endregion

        #region UI
        private void CreateReturnMapUI()
        {
            // Panelの基本設定
            returnMapPanel = new System.Windows.Forms.Panel
            {
                Size = new System.Drawing.Size(650, 450),
                BackColor = System.Drawing.Color.FromArgb(245, 255, 255, 255),
                BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle,
                Visible = false
            };
            // Panelを中央に配置
            returnMapPanel.Location = new System.Drawing.Point(this.ClientSize.Width / 2 - returnMapPanel.Width / 2, this.ClientSize.Height / 2 - returnMapPanel.Height / 2);
            this.Controls.Add(returnMapPanel);

            // タイトルラベル
            System.Windows.Forms.Label lblTitle = new System.Windows.Forms.Label
            {
                Text = "🎉 クリアおめでとう！ 🎉",
                Font = new System.Drawing.Font("Ink Free", 28F, System.Drawing.FontStyle.Bold),
                ForeColor = System.Drawing.Color.DodgerBlue,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Size = new System.Drawing.Size(returnMapPanel.Width, 70),
                Location = new System.Drawing.Point(0, 40)
            };
            returnMapPanel.Controls.Add(lblTitle);

            // 説明ラベル
            System.Windows.Forms.Label lblExplanation = new System.Windows.Forms.Label
            {
                Text = "つぎのステージにすすむひとは「マップにもどる」を、\nクリアしたがめんでしゃしんをとりたいひとは「このまま」をおしてね！",
                Font = new System.Drawing.Font("Yu Gothic UI", 15F),
                ForeColor = System.Drawing.Color.DarkSlateGray,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Size = new System.Drawing.Size(returnMapPanel.Width - 40, 100),
                Location = new System.Drawing.Point(20, 140)
            };
            returnMapPanel.Controls.Add(lblExplanation);

            // 「マップにもどる」ボタン
            System.Windows.Forms.Button btnReturn = new System.Windows.Forms.Button
            {
                Text = "マップにもどる",
                Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(250, 80),
                Location = new System.Drawing.Point(returnMapPanel.Width / 2 - 250 - 20, 280),
                BackColor = System.Drawing.Color.White,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ForeColor = System.Drawing.Color.DodgerBlue
            };
            btnReturn.FlatAppearance.BorderSize = 1;
            btnReturn.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            btnReturn.Click += (s, e) =>
            {
                returnMapPanel.Visible = false;
                if ((_worldNumber == 1 && _level == 2) || _level == 3)
                {
                    if (_worldNumber <= 4) Func.CreateWorldMap(this);
                    else Func.CreateAnotherWorld(this);
                }
                else
                {
                    Func.CreateStageSelect(this, _worldName, _worldNumber);
                }
            };
            returnMapPanel.Controls.Add(btnReturn);

            // 「このまま」ボタン
            System.Windows.Forms.Button btnStay = new System.Windows.Forms.Button
            {
                Text = "このまま",
                Font = new System.Drawing.Font("Yu Gothic UI", 16F, System.Drawing.FontStyle.Bold),
                Size = new System.Drawing.Size(250, 80),
                Location = new System.Drawing.Point(returnMapPanel.Width / 2 + 20, 280),
                BackColor = System.Drawing.Color.WhiteSmoke,
                FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                ForeColor = System.Drawing.Color.DimGray
            };
            btnStay.FlatAppearance.BorderSize = 1;
            btnStay.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            btnStay.Click += (s, e) =>
            {
                returnMapPanel.Visible = false;
            };
            returnMapPanel.Controls.Add(btnStay);
        }
        #endregion

        #region 隕石
        public static TaskCompletionSource<bool> MeteoResult = new TaskCompletionSource<bool>();//隕石の処理が終わったかどうか
        private async void button_meteo_Click(object sender, EventArgs e)
        {
            MeteoResult = new TaskCompletionSource<bool>();
            int n = rand.Next(4, 8);
            int m = rand.Next(4, 8);
            meteorTargetX = n;
            meteorTargetY = m;
            meteorX = (n - 4) * cell_length;
            meteorY = (m - 4) * cell_length; // 空からスタート
            meteorX_speed = (4 * cell_length) / 10;
            meteorY_speed = (4 * cell_length) / 10;

            await StartMeteorFallAsync();//隕石を落とす
            await Task.Delay(100);
            Image explosion = RotateImage(Dictionaries.Img_DotPic["explosion"], 0f);
            g2.DrawImage(explosion, (n - 2) * cell_length - extra_length, (m - 2) * cell_length - 2 * extra_length, 4 * (cell_length + 2 * extra_length), 4 * (cell_length + 2 * extra_length));
            pictureBox_Map2.Refresh();
            await Task.Delay(500);
            g2.Clear(Color.Transparent);//人の移動などのリセット
            Image character_me = Dictionaries.Img_DotPic["正面"];
            g2.DrawImage(character_me, x_now * cell_length - extra_length, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
            pictureBox_Map2.Refresh();

            Graphics g1 = Graphics.FromImage(bmp1);

            for (int y = -1; y < 2; y++)
            {
                for (int x = -1; x < 2; x++)
                {
                    if (map[n + x, m + y] == 2)
                    {
                        int placeX = (n + x) * cell_length;
                        int placeY = (m + y) * cell_length;
                        map[n + x, m + y] = 3;
                        g1.DrawImage(Dictionaries.Img_Object["3"], placeX, placeY, cell_length, cell_length);
                    }

                }
            }

            pictureBox_Map1.Refresh();            

        }

        private async Task StartMeteorFallAsync()
        {
            MeteoResult = new TaskCompletionSource<bool>();
            meteorTimer.Interval = 16;
            meteorTimer.Tick += meteorTimer_Tick;
            meteorTimer.Start();

            await MeteoResult.Task;
        }

        

        private void meteorTimer_Tick(object sender, EventArgs e)
        {
            g2.DrawImage(Dictionaries.Img_DotPic["meteo"], meteorX, meteorY, 3 * (cell_length + 2 * extra_length), 3 * (cell_length + 2 * extra_length));
            pictureBox_Map2.Refresh();
            meteorX += meteorX_speed;
            meteorY += meteorY_speed; // 落下スピード
            if (meteorY >= (meteorTargetY - 2) * cell_length)
            {
                meteorTimer.Stop();
                meteorTimer.Tick -= meteorTimer_Tick;
                MeteoResult?.SetResult(true);
                pictureBox_Map2.Refresh();
                MeteoResult = new TaskCompletionSource<bool>();
                return;
            }
            return;
        }

        private void pictureBox_Car_Click(object sender, EventArgs e)
        {

        }

        private void Meteo_KeyDown(object sender, KeyEventArgs e)
        {
            if (Func.IsInputLocked) return;

            if (e.KeyCode == Keys.M)
            {
                button_meteo.Visible = true;
            }
        }

        #endregion


        #region ペンギン
        private void Penguin_KeyDown(object sender, KeyEventArgs e)
        {
            if (Func.IsInputLocked) return;

            if (e.KeyCode == Keys.P)
            {
                if (_worldNumber == 6) 
                {
                    listBox_Order.Items.Clear();//全て消す
                    if (InputListBox == listBox_Order)
                    {
                        walk_Count = 0;
                        car_Count = 0;
                        plane_Count = 0;
                        balloon_Count = 0;
                        lockedCarPattern = null;
                    }
                    Left_Availabel_Input();
                    InputListBox.Focus();
                    if (Penguin == false)
                    {
                        
                        g2.Clear(Color.Transparent);
                        g2.DrawImage(Img_Penguin["正面"], x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        pictureBox_Map2.Refresh();
                        Penguin = true;
                        picture = "penguin";
                    }
                    else
                    {
                        g2.Clear(Color.Transparent);
                        g2.DrawImage(Dictionaries.Img_DotPic["正面"], x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        pictureBox_Map2.Refresh();
                        Penguin = false;
                        picture = "walk";
                    }
                
                }

            }
        }

        #endregion


        #region UFO

        private async void UFO_KeyDown(object sender, KeyEventArgs e)
        {
            if (Func.IsInputLocked) return;

            if (e.KeyCode == Keys.U)
            {
                g2.DrawImage(Dictionaries.Img_DotPic["UFO"], x_start * cell_length - extra_length, (y_start-1) * cell_length - 3 * extra_length-10, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                
                pictureBox_Map2.Refresh();
                int n;
                int m;
                await Task.Delay(200);
                Image character_me;

                for (int i = 1; i < 6; i++)
                {                    
                    g2.Clear(Color.Transparent);
                    character_me = Dictionaries.Img_DotPic["正面"];
                    if (Penguin == true) character_me = Img_Penguin["正面"];
                    g2.DrawImage(character_me, x_now * cell_length - extra_length, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    g2.DrawImage(Dictionaries.Img_DotPic["UFO"], x_start * cell_length - extra_length, (y_start - 1) * cell_length - 3 * extra_length - 10, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    g2.DrawImage(Dictionaries.Img_DotPic["Light"+i], x_start * cell_length - extra_length - 2, y_start * cell_length - 2 * extra_length - 13, cell_length + extra_length + 10, cell_length + 2 * extra_length + 10);
                    if(_worldNumber==5) Panda();
                    pictureBox_Map2.Refresh();
                    await Task.Delay(100);
                }
                for (int i = 5; i > 0; i--)
                {
                    g2.Clear(Color.Transparent);             
                    
                    g2.DrawImage(Dictionaries.Img_DotPic["UFO"], x_start * cell_length - extra_length, (y_start - 1) * cell_length - 3 * extra_length - 10, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    g2.DrawImage(Dictionaries.Img_DotPic["Light" + i], x_start * cell_length - extra_length - 2, y_start * cell_length - 2 * extra_length - 13, cell_length + extra_length + 10, cell_length + 2 * extra_length + 10);
                    if (_worldNumber == 5) Panda();
                    pictureBox_Map2.Refresh();
                    await Task.Delay(100);
                }
                g2.Clear(Color.Transparent);
                if (_worldNumber == 5) Panda();
                pictureBox_Map2.Refresh();
                await Task.Delay(200);

                while (true)
                {
                    n = rand.Next(1, map_width);
                    m = rand.Next(1, map_width);
                    if (map[n, m] == 3&&(x_start!=n||y_start!=m))
                    {
                        x_start =n;
                        x_now = n;
                        y_start=m;
                        y_now = m;
                        break;
                    }
                }

                g2.Clear(Color.Transparent);
                if (_worldNumber == 5) Panda();
                g2.DrawImage(Dictionaries.Img_DotPic["UFO"], x_start * cell_length - extra_length, (y_start - 1) * cell_length - 3 * extra_length - 10, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                pictureBox_Map2.Refresh();
                await Task.Delay(200);

                for (int i = 1; i < 6; i++)
                {
                    g2.Clear(Color.Transparent);                    
                    g2.DrawImage(Dictionaries.Img_DotPic["UFO"], x_start * cell_length - extra_length, (y_start - 1) * cell_length - 3 * extra_length - 10, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    g2.DrawImage(Dictionaries.Img_DotPic["Light" + i], x_start * cell_length - extra_length - 2, y_start * cell_length - 2 * extra_length - 13, cell_length + extra_length + 10, cell_length + 2 * extra_length + 10);
                    if (_worldNumber == 5) Panda();
                    pictureBox_Map2.Refresh();
                    await Task.Delay(100);
                }
                for (int i = 5; i > 0; i--)
                {
                    g2.Clear(Color.Transparent);
                    character_me = Dictionaries.Img_DotPic["正面"];
                    if (Penguin == true) character_me = Img_Penguin["正面"];
                    g2.DrawImage(character_me, x_now * cell_length - extra_length, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    g2.DrawImage(Dictionaries.Img_DotPic["UFO"], x_start * cell_length - extra_length, (y_start - 1) * cell_length - 3 * extra_length - 10, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                    g2.DrawImage(Dictionaries.Img_DotPic["Light" + i], x_start * cell_length - extra_length - 2, y_start * cell_length - 2 * extra_length - 13, cell_length + extra_length + 10, cell_length + 2 * extra_length + 10);
                    if (_worldNumber == 5) Panda();
                    pictureBox_Map2.Refresh();
                    await Task.Delay(100);
                }
                g2.Clear(Color.Transparent);
                character_me = Dictionaries.Img_DotPic["正面"];
                if (Penguin == true) character_me = Img_Penguin["正面"];
                g2.DrawImage(character_me, x_start * cell_length - extra_length, y_start * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                if (_worldNumber == 5) Panda();
                pictureBox_Map2.Refresh();
                await Task.Delay(200);


            }
        }



        #endregion

    }
}
