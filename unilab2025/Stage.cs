using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            this.KeyPreview = true;

            #region ボタン表示

            #endregion


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
        Bitmap bmp2;

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
        public static int limit_LB_Input;
        public static int limit_LB_A;
        public static int limit_LB_B;

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


        #endregion


        private async void Stage_Load(object sender, EventArgs e)  //StageのFormの起動時処理
        {
            pictureBox_Background.BackgroundImage = Dictionaries.Img_Background["Stage" + _worldNumber];//背景
            stageName = "stage" + _worldNumber + "-" + _level;
            map = CreateStage(stageName); //ステージ作成   



        }

        private int[,] CreateStage(string stageName)     //ステージ作成
        {
            //string stagenum = _worldNumber + "-" + _level;
            using (StreamReader sr = new StreamReader($"Map\\{stageName}.csv"))
            {
                int x = 0;
                int y = 0;
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

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
                map_width = x; //マップの横幅
            }

            Graphics g1 = Graphics.FromImage(bmp1);
            Graphics g2 = Graphics.FromImage(bmp2);
            //label_Info.BackgroundImage = Image.FromFile("focus.png");


            cell_length = pictureBox_Map.Width / map_width;


            for (int y = 0; y < map_width; y++)
            {
                for (int x = 0; x < map_width; x++)
                {
                    int placeX = x * cell_length;
                    int placeY = y * cell_length;
                    g1.DrawImage(Dictionaries.Img_Object[map[x, y].ToString()], placeX, placeY, cell_length, cell_length);
                    switch (map[x, y])
                    {
                        case 0:
                            x_start = x;
                            y_start = y;
                            x_now = x;
                            y_now = y;
                            Image character_me = Dictionaries.Img_DotPic["魔法使いサンプル"];
                            g2.DrawImage(character_me, placeX - extra_length, placeY - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                            break;
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
            this.Invoke((MethodInvoker)delegate
            {
                // pictureBox_Mapを同期的にRefreshする
                pictureBox_Map.Refresh();
            });
            return map;
        }

        #region ボタン押下時処理

        #endregion

        #region リセット関連
        public bool ResetListBox(ListBox listbox)   //ListBoxの中身消去
        {
            bool isAllReset = false;
            if (listbox.SelectedIndex > -1)
            {
                listbox.Items.RemoveAt(listbox.SelectedIndex);
                return isAllReset;
            }
            else
            {
                listbox.Items.Clear();
                return !isAllReset;
            }
        }

        private void DisplayMessage(string type)
        {
            isMessageMode = true;
            Message = Dictionaries.Messages[type];
            Capt = Func.PlayConv(this, pictureBox_Conv, Message);
        }
        public void resetStage() // ステージリセット
        {
            //初期位置に戻す
            x_now = x_start;
            y_now = y_start;

            //初期位置に書き換え
            Graphics g2 = Graphics.FromImage(bmp2);
            g2.Clear(Color.Transparent);
            //int cell_length = pictureBox1.Width / 12;
            //character_me = Image.FromFile("忍者_正面.png");
            g2.DrawImage(Dictionaries.Img_DotPic["魔法使いサンプル"], x_now * cell_length - extra_length, y_now * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);

            //g2.DrawImage(goal_obj(_stageName), Global.x_goal * cell_length - Global.extra_length, Global.y_goal * cell_length - 2 * Global.extra_length, cell_length + 2 * Global.extra_length, cell_length + 2 * Global.extra_length);
            this.Invoke((MethodInvoker)delegate
            {
                // pictureBox2を同期的にRefreshする
                pictureBox2.Refresh();
            });
            CreateStage(stageName);

            //初期設定に戻す
            button_Start.Visible = true;
            button_Start.Enabled = true;
            label_Error.Visible = false;
            count = 0;
            miss_count = 0;
            isEndfor = true;
            label_Error.Text = "ミス！";
            label_Error.Visible = false;
            label_Result.Visible = false;
        }
        #endregion

        #region 動作関連

        /// <summary>
        /// 入力(direction)に応じてマップ上で動きを実装
        /// </summary>
        /// <param name="movelist">キャラクターがどう動くかのリスト</param>
        /// <param name="direction">動く方向</param>
        public void MoveTo(List<int[]> movelist, string direction)    //指定方向に移動する関数
        {
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
                default:
                    break;
            }
            int[][] move = new int[4][];       // up,right.down,leftの順
            move[0] = new int[] { 0, -1 };     //up
            move[1] = new int[] { 1, 0 };      //right
            move[2] = new int[] { 0, 1 };      //down 
            move[3] = new int[] { -1, 0 };     //left
            if (Direction_Index < 4) movelist.Add(move[Direction_Index]);
        }

        /// <summary>
        /// ユーザーの入力Listを作成する関数(ほかの関数を呼び出す場合などの処理)
        /// </summary>
        /// <param name="A">自作関数Aの動き</param>
        /// <param name="B">自作関数Bの動き</param>
        /// <param name="Move_Input">出力先の動き</param>
        /// <returns></returns>
        public List<string> MakeMoveList(string[] A, string[] B, List<string> Move_Input)     //入力A・B(Move_Input)が再起の場合の処理
        {
            List<string> Return_List = new List<string>();          //戻り値用のList
            for (int i = 0; i < Move_Input.Count; i++)　　　　　　　//A・Bの中身だけ探索
            {

                if (Move_Input[i] == "B")
                {
                    Return_List.AddRange(B);                        //Bが含まれていたらBの操作を追加

                }
                else if (Move_Input[i] == "A")
                {
                    Return_List.AddRange(A);                        //Aが含まれていたらAの操作を追加

                }
                else
                {
                    Return_List.Add(Move_Input[i]);                 //そのまま追加
                }
            }
            return Return_List;
        }

        /// <summary>
        /// for文での処理を行う関数、多重のfor文の場合にも対応
        /// </summary>
        /// <param name="move">キャラクターのマップ上での動き</param>
        /// <param name="Move_Input">ユーザーの入力した方向</param>
        /// <param name="Move_A">自作関数Aを呼び出す場合の処理</param>
        /// <param name="Move_B">自作関数Bを呼び出す場合の処理</param>
        /// <param name="i"></param>
        /// <returns></returns>
        public (List<int[]>, int a) ForLoop(List<int[]> move, List<string> Move_Input, List<int[]> Move_A, List<int[]> Move_B, int i)     //for文処理
        {
            int trial;                                                                                                //反復回数
            int Now;                                                                                                  //入力したListのうち何番目の処理か
            //List<int[]> Return_List = new List<int[]>();                                                              //出力の配列(動きを[x,y]として保存)
            int Return_Num = i;                                                                                       //何番目までfor文処理が続いているか
            if (Move_Input[i].StartsWith("リフレイン("))
            {
                trial = int.Parse(Regex.Replace(Move_Input[i], @"[^0-9]", ""));                                       //処理回数をtrialに設定
                for (int j = 0; j < trial; j++)
                {
                    Now = i + 1;                                                                                          //for文の処理内容はi+1行目から
                    while (true)
                    {
                        if (Now >= Move_Input.Count)                                                                 //for文の終わりが存在しない場合、エラー表示
                        {
                            //MessageBox.Show("「リフレイン」と「リフレインおわり」はセットで使ってください");
                            DisplayMessage("LackEndfor");
                            isEndfor = false;
                            return (move, i);
                        }
                        (move, Now) = ForLoop(move, Move_Input, Move_A, Move_B, Now);                                  //二重ループの探索
                        if (Move_Input[Now] == "リフレイン終わり") break;                                                       //for文終わりが存在したら処理終了
                        else
                        {
                            if (Move_Input[Now] == "A") move.AddRange(Move_A);                            //Aの魔法が入力されている場合、Aの処理内容をListに追加
                            else if (Move_Input[Now] == "B") move.AddRange(Move_B);                        //Bの魔法の際も同様                     
                            else MoveTo(move, Move_Input[Now]);                                        //動く方向が指定されている場合、その方向への動きをListに追加
                            Now++;
                        }
                    }
                    Return_Num = Now;
                }
            }
            return (move, Return_Num);                                                                          //動きの内容(List)とどこまで処理したかを返却
        }

        /// <summary>
        /// 実際にユーザーの入力を読み取り、動きのListを作成
        /// </summary>
        /// <returns></returns>
        public List<int[]> Movement()      //動作の関数
        {
            var Move_A = new List<int[]>();                                                           //Aでの動きを保存
            var Move_B = new List<int[]>();                                                           //Bでの動きを保存
            string[] Get_Input_A = this.listBox_A.Items.Cast<string>().ToArray();                     //AのListへの入力を保存
            string[] Get_Input_B = this.listBox_B.Items.Cast<string>().ToArray();                     //BのListへの入力を保存

            var Move_A_List = new List<string>(Get_Input_A);
            var Move_B_List = new List<string>(Get_Input_B);

            int loop_count = 0;
            while (Move_A_List.Count <= 30 || Move_B_List.Count <= 30)
            {
                Move_A_List = MakeMoveList(Get_Input_A, Get_Input_B, Move_A_List);
                Move_B_List = MakeMoveList(Get_Input_A, Get_Input_B, Move_B_List);
                loop_count++;

                if (loop_count > 5)
                {
                    break;
                }
            }

            if (Get_Input_A.Length != 0)
            {
                for (int i = 0; i < Move_A_List.Count; i++)
                {
                    (Move_A, i) = ForLoop(Move_A, Move_A_List, Move_A, Move_B, i);
                    MoveTo(Move_A, Move_A_List[i]);
                }
            }

            if (Get_Input_B.Length != 0)
            {
                for (int i = 0; i < Move_B_List.Count; i++)
                {
                    (Move_B, i) = ForLoop(Move_B, Move_B_List, Move_A, Move_B, i);
                    MoveTo(Move_B, Move_B_List[i]);
                }
            }

            string[] Get_Input_Main = this.listBox_Input.Items.Cast<string>().ToArray();
            //Get_Input_Main = exchange_move(Get_Input_Main);
            List<string> Move_Main_List = new List<string>(Get_Input_Main);
            List<int[]> move = new List<int[]>();

            if (Get_Input_Main.Length != 0)
            {
                for (int i = 0; i < Move_Main_List.Count; i++)
                {
                    (move, i) = ForLoop(move, Move_Main_List, Move_A, Move_B, i);
                    if (Move_Main_List[i] == "A") move.AddRange(Move_A);
                    else if (Move_Main_List[i] == "B") move.AddRange(Move_B);
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
        /// 動きに応じたキャラクターの画像を表示
        /// </summary>
        /// <param name="x">動きのx方向</param>
        /// <param name="y">動きのy方向</param>
        /// <param name="steps">歩きの差分(右足→左足)</param>
        /// <param name="jump">飛んでいる場合</param>
        /// <param name="Chara">出力画像へのポインタ</param>
        /// <returns></returns>

        Image Character_Image(int x, int y, int steps, int jump, bool DoubleJump, Image Chara)
        {
            int a = steps % 2;//歩き差分を識別
            int direction = x * 10 + y;
            int type = a + 1;
            //if (a % 2 == 0) return Dictionaries.Img_DotPic["魔法使いサンプル"];   //左右図でき次第差し替え
            if (jump == 0)
            {
                switch (direction)
                {
                    case 10:
                        Chara = Dictionaries.Img_DotPic[$"歩き右{type}"];
                        break;
                    case -10:
                        Chara = Dictionaries.Img_DotPic[$"歩き左{type}"];
                        break;
                    case 1:
                        Chara = Dictionaries.Img_DotPic[$"歩き{type}"];
                        break;
                    case -1:
                        Chara = Dictionaries.Img_DotPic[$"歩き後ろ{type}"];
                        break;
                }
            }
            else if (jump != 0 && !DoubleJump)
            {
                switch (direction)
                {
                    case 10:
                        Chara = Dictionaries.Img_DotPic[$"箒右"];
                        break;
                    case -10:
                        Chara = Dictionaries.Img_DotPic[$"箒左"];
                        break;
                    case 1:
                        Chara = Dictionaries.Img_DotPic[$"箒"];
                        break;
                    case -1:
                        Chara = Dictionaries.Img_DotPic[$"箒後ろ"];
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case 10:
                        Chara = Dictionaries.Img_DotPic[$"虹箒右"];
                        break;
                    case -10:
                        Chara = Dictionaries.Img_DotPic[$"虹箒左"];
                        break;
                    case 1:
                        Chara = Dictionaries.Img_DotPic[$"虹箒"];
                        break;
                    case -1:
                        Chara = Dictionaries.Img_DotPic[$"虹箒後ろ"];
                        break;
                }
            }
            return Chara;
        }


        /// <summary>
        /// 動く際の描画や処理を行う関数
        /// </summary>
        /// <param name="x">現在地のx座標</param>
        /// <param name="y">現在地のy座標</param>
        /// <param name="Map">ステージのマップ情報</param>
        /// <param name="move">動きのリスト</param>
        public void SquareMovement(int x, int y, int[,] Map, List<int[]> move)
        {
            Graphics g2 = Graphics.FromImage(bmp2);
            //cell_length = pictureBox1.Width / 12;
            if (move.Count == 0) //ゴールについていない場合(入力がない)場合のエラー処理
            {
                DisplayMessage("miss_end");
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
                if (_worldNumber > 4 && (x_now != x_goal || y_now != y_goal))
                {
                    int placeX = x_goal * cell_length;
                    int placeY = y_goal * cell_length;
                    int goal = 10 + _worldNumber;
                    g2.DrawImage(Dictionaries.Img_Object[goal.ToString()], placeX, placeY, cell_length, cell_length);
                }
                g2.DrawImage(character_me, a * cell_length - extra_length, b * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
            }

            (int, int) draw_move(int a, int b, ref List<int[]> move_next)
            {
                (x_now, y_now) = place_update(a, b, move_next);
                character_me = Character_Image(move_copy[0][0], move_copy[0][1], count_walk, jump, DoubleJump, character_me);
                DrawCharacter(x_now, y_now, ref character_me);
                this.Invoke((MethodInvoker)delegate
                {
                    // pictureBox2を同期的にRefreshする
                    pictureBox2.Refresh();
                });
                return (x_now, y_now);
            }
            while (true)
            {
                if (move_copy.Count == 0)//動作がすべて終了した場合
                {
                    if (x_now != x_goal || y_now != y_goal)
                    {
                        DisplayMessage("miss_end");
                    }
                    else
                    {
                        g2.Clear(Color.Transparent);
                        //Graphics g2 = Graphics.FromImage(bmp2);
                        int placeX = x_goal * cell_length;
                        int placeY = y_goal * cell_length;
                        g2.DrawImage(Dictionaries.Img_DotPic["GOAL"], placeX - extra_length, placeY - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                        this.Invoke((MethodInvoker)delegate
                        {
                            // pictureBox2を同期的にRefreshする
                            pictureBox2.Refresh();
                        });
                    }
                    break;
                }
                else
                {
                    if (Colision_detection(x, y, Map, move_copy) && jump == 0)
                    {
                        //忍者を動かしてからミスの表示を出す
                        (x_now, y_now) = draw_move(x, y, ref move_copy);
                        DisplayMessage("miss_out");
                        //DrawCharacter(x, y, ref character_me);
                        break;
                    }
                    if (jump == 0 && Map[x + move_copy[0][0], y + move_copy[0][1]] == 2) //jumpの時着地先が木の場合、ゲームオーバー
                    {
                        (x_now, y_now) = draw_move(x, y, ref move_copy);
                        Thread.Sleep(waittime);
                        (x_now, y_now) = draw_move(x, y, ref move_copy);

                        DisplayMessage("miss_out");
                        //DrawCharacter(x, y, ref character_me);
                        break;
                    }
                    if (count_walk > 50) //無限ループ対策
                    {
                        DisplayMessage("miss_countover");
                        //DrawCharacter(x, y, ref character_me);
                        break;
                    }
                }


                //jumpでない時移動先が木の場合、木の方向には進めない
                //if (jump == 0 && Map[x + move_copy[0][0], y + move_copy[0][1]] == 2)
                //{
                //    DrawCharacter(x, y, ref character_me);
                //    move_copy.RemoveAt(0);
                //    //500ミリ秒=0.5秒待機する
                //    Thread.Sleep(waittime);
                //    continue;
                //}
                if (Map[x + move_copy[0][0], y + move_copy[0][1]] == 10)
                {
                    DrawCharacter(x, y, ref character_me);
                    move_copy.RemoveAt(0);
                    //500ミリ秒=0.5秒待機する
                    Thread.Sleep(waittime);
                    continue;
                }
                if (Map[x, y] == 5 || Map[x, y] == 6)
                {
                    Graphics g1 = Graphics.FromImage(bmp1);
                    int placeX = x * cell_length;
                    int placeY = y * cell_length;
                    g1.DrawImage(Dictionaries.Img_Object["3"], placeX, placeY, cell_length, cell_length);
                }

                (x_now, y_now) = draw_move(x, y, ref move_copy);

                //if (Map[x, y] == 101 && Map[x - move_copy[0][0], y - move_copy[0][1]] != 2)     //何の判定??? -> 正面向かせる処理
                //{
                //    DrawCharacter(x, y, ref character_me);
                //    //character_me = Image.FromFile("忍者_正面.png");
                //    //g2.DrawImage(character_me, x * cell_length - extra_length, y * cell_length - 2 * extra_length, cell_length + 2 * extra_length, cell_length + 2 * extra_length);
                //    break;
                //}

                if ((Map[x, y] == 5) || (Map[x, y] == 6))
                {
                    jump = Map[x, y] - 3;
                    if (Map[x, y] == 6) DoubleJump = true;
                }
                //移動先がジャンプ台なら同じ方向に二回進む（１個先の障害物は無視）
                if (jump != 0)                           //ジャンプ台の番号も決める(いまは一旦9)
                {
                    jump--;
                    if (jump == 0) DoubleJump = false;
                    Thread.Sleep(waittime);
                    continue;
                }

                //移動先が氷の上なら同じ方向にもう一回進む
                if (jump == 0 && Map[x, y] == 8)
                {
                    //500ミリ秒=0.5秒待機する
                    Thread.Sleep(waittime);
                    continue;
                }

                //ワープの処理
                if (Map[x, y] == 7)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        for (int j = 0; j < 12; j++)
                        {
                            if (Map[i, j] == 7 && (i != x || j != y))
                            {
                                x = i;
                                y = j;
                                isWarp = true;
                            }
                        }
                        if (isWarp) break;
                    }
                    //Image character_me = Dictionaries.Img_DotPic["魔法使いサンプル"];
                    //DrawCharacter(x, y, ref character_me);
                    //this.Invoke((MethodInvoker)delegate
                    //{
                    //    // pictureBox2を同期的にRefreshする
                    //    pictureBox2.Refresh();
                    //});
                    Thread.Sleep(waittime);

                }

                move_copy.RemoveAt(0);

                Thread.Sleep(waittime);
                count_walk++;
            }
        }
#endregion


    #region カスタム設定
    public class UIButtonObject : UserControl
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            GraphicsPath graphic = new GraphicsPath();
            graphic.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new Region(graphic);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.FillEllipse(new SolidBrush(this.BackColor), 0, 0, this.ClientSize.Width, this.ClientSize.Height);
        }

    }

    #endregion
}
}
