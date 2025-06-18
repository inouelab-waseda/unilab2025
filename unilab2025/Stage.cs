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


        Image character_me = Dictionaries.Img_DotPic["銀髪ドット正面"];

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

        public static string grade;    //学年
        public static int gradenum;
        public static int car_count=0;
        public static bool car_finish = true;
        public static List<string> list_car;


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
            this.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成


            InputListBox = listBox_Order;
            ListBoxes.Add(listBox_Order);
            ListBoxes.Add(listBox_Car);
            picture = "walk";
            UpdateMovementButtonImages();
            listBox_Order.Focus();
            ShowListBox();
            grade = Regex.Replace(stageName, @"[^0-9]", "");
            int chapter_num = int.Parse(grade) / 10;

            
            Dictionaries.Img_DotPic["car"] = Image.FromFile(@"Image\\DotPic\\car.png");
            Dictionaries.Img_DotPic["ball"] = Image.FromFile(@"Image\\DotPic\\ball.png");
            Dictionaries.Img_DotPic["plane"] = Image.FromFile(@"Image\\DotPic\\plane.png");

            if (_worldNumber == 1)
            {
                button_car.Visible = false;
                button_balloon.Visible = false;
                button_plane.Visible = false;
                listBox_Car.Visible = false;
                pictureBox_Car.Visible=false;
                label_Car.Visible = false;
                label_Car_Input.Visible = false;
                label_Plane.Visible = false;
                label_Balloon.Visible = false;


            }
            if (_worldNumber == 2)
            {
                
                button_balloon.Visible = false;
                button_plane.Visible = false;
                label_Plane.Visible = false;
                label_Balloon.Visible = false;

            }
            if (_worldNumber == 3)
            {

                button_balloon.Visible = false;
                label_Balloon.Visible = false;

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
            label_Walk.Text = $"あと {limit_LB_walk}";
            label_Car.Text = $"あと {limit_LB_car}";
            label_Car_Input.Text = $"あと {limit_LB_car_Input}";
            label_Plane.Text = $"あと {limit_LB_plane}";
            label_Balloon.Text = $"あと {limit_LB_balloon}";
            walk_Count = 0;
            car_Count = 0;
            plane_Count = 0;
            balloon_Count = 0;

                                     
            listBox_Car.Height = limit_LB_car_Input * listBox_Car.ItemHeight+20;

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
            if(_worldNumber >= 2) button_car.Enabled = true;
            if(_worldNumber >= 3) button_plane.Enabled = true;
            if (_worldNumber >= 4) button_balloon.Enabled = true;

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
                    if(InputListBox.SelectedItem.ToString().Contains("🚶‍"))
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

                }
                InputListBox.Items.RemoveAt(InputListBox.SelectedIndex);//1つ消す
                if (InputListBox == listBox_Order)
                {
                    label_Walk.Text = $"あと {limit_LB_walk - walk_Count}";
                    label_Car.Text = $"あと {limit_LB_car - car_Count}";
                    label_Plane.Text = $"あと {limit_LB_plane - plane_Count}";
                    label_Balloon.Text = $"あと {limit_LB_balloon - balloon_Count}";
                }

                else if (InputListBox == listBox_Car) label_Car_Input.Text = $"あと {limit_LB_car_Input - listBox_Car.Items.Count}";
            }
            else
            {
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

                }
                InputListBox.Items.RemoveAt(InputListBox.Items.Count - 1);//1つ消す
                if (InputListBox == listBox_Order)
                {
                    label_Walk.Text = $"あと {limit_LB_walk - walk_Count}";
                    label_Car.Text = $"あと {limit_LB_car - car_Count}";
                    label_Plane.Text = $"あと {limit_LB_plane - plane_Count}";
                    label_Balloon.Text = $"あと {limit_LB_balloon - balloon_Count}";
                }

                else if (InputListBox == listBox_Car) label_Car_Input.Text = $"あと {limit_LB_car_Input - listBox_Car.Items.Count}";

            }
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
            }
            Left_Availabel_Input();
        }

        private void DisplayMessage(string type)
        {
            isMessageMode = true;
            //Message = Dictionaries.Messages[type];
            //Capt = Func.PlayConv(this, pictureBox_Conv, Message);
        }



        #endregion

        #region ボタン押下時処理
        private async void button_Start_Click(object sender, EventArgs e)
        {
            button_Start.Visible = false;
            button_Start.Enabled = false;
            if (listBox_Order.Items.Count == 0) { 
                MessageBox.Show("やり直し");
                button_Start.Visible = true;
                button_Start.Enabled = true;
            }
            move = Movement(); //ユーザーの入力を読み取る
            //List<string> Input_Main = 

            SquareMovement(x_now, y_now, map, move); //キャラ動かす
            
            //count += 1;
            //if (x_goal == x_now && y_goal == y_now)
            //{
            //    //label_Result.Text = "クリア！！";
            //    //label_Result.Visible = true;
            //    //button_ToMap.Enabled = true;
            //    //button_Retry.Enabled = false;
            //    //button_ToMap.Visible = true;
            //    isStartConv = false;
            //    //button_ToMap.Location = new Point(800, 600);
            //    //button_ToMap.Size = new Size(200, 50);

            //    ClearCheck.IsCleared[_worldNumber, _level] = true;    //クリア状況管理
            //    if (_worldNumber == 4)
            //    {
            //        if (!ClearCheck.IsCleared[_worldNumber, 0])
            //        {
            //            ClearCheck.PlayAfterChapter4Story = true;
            //        }
            //        for (int j = 0; j < (int)ConstNum.numStages; j++)
            //        {
            //            ClearCheck.IsCleared[_worldNumber, j] = true;
            //        }
            //        for (int i = _worldNumber + 1; i < (int)ConstNum.numWorlds; i++)
            //        {
            //            for (int j = 0; j <= 1; j++)
            //            {
            //                ClearCheck.IsButtonEnabled[i, j] = true;
            //                ClearCheck.IsNew[i, j] = true;
            //            }
            //        }
            //    }
            //    else if (_level == 3)
            //    {
            //        ClearCheck.IsCleared[_worldNumber, 0] = true;
            //        switch (_worldNumber)
            //        {
            //            case 1:
            //            case 2:
            //            case 3:
            //                for (int j = 0; j <= 1; j++)
            //                {
            //                    ClearCheck.IsButtonEnabled[_worldNumber + 1, j] = true;
            //                    ClearCheck.IsNew[_worldNumber + 1, j] = true;
            //                }
            //                break; ;
            //        }
            //    }
            //    else
            //    {
            //        ClearCheck.IsButtonEnabled[_worldNumber, _level + 1] = true;
            //        ClearCheck.IsNew[_worldNumber, _level + 1] = true;
            //        Func.UpdateIsNew();
            //    }

            //    if (Func.HasNewStageInAllWorld())
            //    {
            //        //button_ToMap.ConditionImage = Dictionaries.Img_Button["New"];
            //    }

            //    if (!ClearCheck.Completed)
            //    {
            //        if (Func.IsAllStageClearedInWorld(false))
            //        {
            //            ClearCheck.Completed = true;
            //            ClearCheck.PlayAfterAnotherWorldStory = true;
            //        }
            //    }

            //    await Task.Delay((int)ConstNum.waitTime_End);
            //    Capt = Func.PlayConv(this, pictureBox_Conv, EndConv);
            //}
            //else
            //{
            //    resetStage("miss_end");
            //}
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

                    default:
                        // どのcaseにも当てはまらないときの処理
                        break;
                }
            }
            if (InputListBox == listBox_Order) 
            { 
                label_Walk.Text = $"あと {limit_LB_walk - walk_Count}";
                label_Car.Text = $"あと {limit_LB_car - car_Count}";
                label_Plane.Text = $"あと {limit_LB_plane - plane_Count}";
                label_Balloon.Text = $"あと {limit_LB_balloon - balloon_Count}";
            }
            else if (InputListBox == listBox_Car) label_Car_Input.Text = $"あと {limit_LB_car_Input - listBox_Car.Items.Count}";            
        }


        private void PictureBox_buttonUp_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↑");
            //if (isChange) Item_Change();            
            Left_Availabel_Input();
        }
        private void PictureBox_upperRight_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↗");
            Left_Availabel_Input();
        }
        private void PictureBox_buttonRight_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "→");
            Left_Availabel_Input();
        }
        private void PictureBox_lowerRight_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↘");
            Left_Availabel_Input();
        }
        private void PictureBox_buttonDown_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↓");
            Left_Availabel_Input();
        }
        private void PictureBox_lowerLeft_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↙");
            Left_Availabel_Input();
        }
        private void PictureBox_buttonLeft_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "←");
            Left_Availabel_Input();
        }
        private void PictureBox_upperLeft_Click(object sender, EventArgs e)
        {
            if (Input_check()) return;
            InputListBox.Items.Add(Emoji[picture] + "  " + "↖");
            Left_Availabel_Input();
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
                    default:
                        MessageBox.Show("これ以上入力できないよ");
                        //label_Info.Text = "これ以上入力できないよ";
                        //label_Info.Visible = true;
                        //DisplayMessage("Overflow");
                        result = true;
                        break;
                }
            }
            else
            {
                if (!(listBox_Car.Items.Count < limit_LB_car_Input))                
                {
                    MessageBox.Show("これ以上入力できないよ");
                    //label_Info.Text = "これ以上入力できないよ";
                    //label_Info.Visible = true;
                    //DisplayMessage("Overflow");
                    result = true;                    
                }
            }          

            return result;
        }



        private void button_walk_Click(object sender, EventArgs e)
        {
            
            picture = "walk";
            UpdateMovementButtonImages();

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
            if (!(car_Count < limit_LB_car))
            {
                MessageBox.Show("これ以上入力できないよ");
                return;
            }
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
            
            string combined = "";
            foreach (var item in listBox_Car.Items)
            {
                string text = item.ToString();

                // 先頭の1文字（または2文字）を抽出（絵文字によっては2文字以上）
                string emoji = text.Substring(text.Length - 2, 2); // 1〜2文字目を仮に絵文字として取り出す
                combined += emoji;
            }

            InputListBox.Items.Add("🚗 (" + combined + ")");
            car_Count += 1;
            label_Walk.Text = $"あと {limit_LB_walk - walk_Count}";
            label_Car.Text = $"あと {limit_LB_car - car_Count}";
            label_Plane.Text = $"あと {limit_LB_plane - plane_Count}";
            label_Balloon.Text = $"あと {limit_LB_balloon - balloon_Count}";            

        }

        private void button_balloon_Click(object sender, EventArgs e)
        {
            picture = "balloon";
            UpdateMovementButtonImages();
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
            UpdateMovementButtonImages();
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
            Func.CreateStageSelect(this, _worldName, _worldNumber);
            return;

        }

        /// <summary>
        /// Updates the images of movement-type buttons based on the selected 'picture'.
        /// </summary>
        private void UpdateMovementButtonImages()
        {
            // Reset all buttons to their "off" state
            button_walk.BackgroundImage = Dictionaries.Img_Button["walk_off"];
            button_plane.BackgroundImage = Dictionaries.Img_Button["plane_off"];
            button_balloon.BackgroundImage = Dictionaries.Img_Button["balloon_off"];

            // Set the selected button to its "on" state
            switch (picture)
            {
                case "walk":
                    button_walk.BackgroundImage = Dictionaries.Img_Button["walk_on"];
                    break;
                case "plane":
                    button_plane.BackgroundImage = Dictionaries.Img_Button["plane_on"];
                    break;
                case "balloon":
                    button_balloon.BackgroundImage = Dictionaries.Img_Button["balloon_on"];
                    break;
            }
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
            pictureBox_upperRight.Image = RotateImage(original, 45f);
            pictureBox_buttonRight.Image = RotateImage(original, 90f);
            pictureBox_lowerRight.Image = RotateImage(original, 135f);
            pictureBox_buttonDown.Image = RotateImage(original, 180f);
            pictureBox_lowerLeft.Image = RotateImage(original, 225f);
            pictureBox_buttonLeft.Image = RotateImage(original, 270f);
            pictureBox_upperLeft.Image = RotateImage(original, 315f);
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
        public async void  SquareMovement(int x, int y, int[,] Map, List<int[]> move)
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
                g2.DrawImage(character_me, a * cell_length - extra_length, b * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
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
                        if (Input_arrow[0].Contains("↑")) character_me = Dictionaries.Img_DotPic["銀髪(後ろ)"];
                        else if (Input_arrow[0].Contains("→")) character_me = Dictionaries.Img_DotPic["銀髪(横右)"];
                        else if (Input_arrow[0].Contains("↓")) character_me = Dictionaries.Img_DotPic["銀髪ドット正面"];
                        else if (Input_arrow[0].Contains("←")) character_me = Dictionaries.Img_DotPic["銀髪ドット(横左)"];
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
                    Image character_me = Dictionaries.Img_DotPic["銀髪ドット正面"];
                    DrawCharacter(x_now, y_now, ref character_me);
                    pictureBox_Map2.Refresh();
                    button_Start.Visible = true;
                    button_Start.Enabled = true;
                    if (x_now != x_goal || y_now != y_goal)
                    {
                        MessageBox.Show("やり直し");
                        g2.Clear(Color.Transparent);
                        DrawCharacter(x_start, y_start, ref character_me);
                        pictureBox_Map2.Refresh();                        
                        x_now = x_start;
                        y_now = y_start;
                    }
                    else
                    {
                        MessageBox.Show("成功");
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
                    if (!Colision_detection(x, y, Map, move_copy))
                    {

                        (x_now, y_now) = draw_move(x, y, ref move_copy);
                        if (car_finish == true && Input_arrow.Count > 0)
                        {
                            if (Input_arrow[0].Contains("✈️"))//飛行機の処理
                            {
                                var Plane = new List<int[]>();
                                MoveTo(Plane, Input_arrow[0]);
                                while (true)
                                {
                                    if (!Colision_detection(x, y, Map, Plane))
                                    {
                                        await Task.Delay(500);
                                        (x_now, y_now) = place_update(x, y, Plane);
                                        DrawCharacter(x_now, y_now, ref character_me);
                                        pictureBox_Map2.Refresh();
                                    }
                                    else break;

                                }
                                Plane.Clear();
                            }
                            Input_arrow.RemoveAt(0);
                        }
                        if (car_count == 0) car_finish = true;
                        move_copy.RemoveAt(0); // 使い終わった移動ステップを削除
                    }
                    else
                    {
                        MessageBox.Show("前に進めません");
                        g2.Clear(Color.Transparent);//人の移動などのリセット
                        Input_arrow.Clear();//入力のリセット
                        Image character_me = Dictionaries.Img_DotPic["銀髪ドット正面"];
                        DrawCharacter(x_start, y_start, ref character_me);
                        pictureBox_Map2.Refresh();
                        x_now = x_start;//スタート位置に戻す
                        y_now = y_start;
                        button_Start.Visible = true;
                        button_Start.Enabled = true;
                        break;
                    }
                        await Task.Delay(500);

                    //if (Colision_detection(x, y, Map, move_copy) && jump == 0)
                    //{
                    //    //忍者を動かしてからミスの表示を出す
                    //    (x_now, y_now) = draw_move(x, y, ref move_copy);
                    //    DisplayMessage("miss_out");
                    //    //DrawCharacter(x, y, ref character_me);
                    //    break;
                    //}
                    ////if (jump == 0 && Map[x + move_copy[0][0], y + move_copy[0][1]] == 2) //jumpの時着地先が木の場合、ゲームオーバー
                    ////{
                    ////    (x_now, y_now) = draw_move(x, y, ref move_copy);
                    ////    Thread.Sleep(waittime);
                    ////    (x_now, y_now) = draw_move(x, y, ref move_copy);

                    ////    DisplayMessage("miss_out");
                    ////    //DrawCharacter(x, y, ref character_me);
                    ////    break;
                    ////}
                    //if (count_walk > 50) //無限ループ対策
                    //{
                    //    DisplayMessage("miss_countover");
                    //    //DrawCharacter(x, y, ref character_me);
                    //    break;
                    //}
                }

                #endregion



            }
        }

        
    }
}
