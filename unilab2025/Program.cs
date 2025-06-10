using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using static unilab2025.Program;

namespace unilab2025
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        #region Main関数
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ゲームで使う画像を先に読み込んでおく
            Func.LoadImg_Character();
            Func.LoadImg_Object();
            Func.LoadImg_Button();//ボタン
            Func.LoadImg_Background();//背景
            Func.LoadImg_DotPic();
            //Func.LoadImg_Conversation();
            //Func.LoadMessages();
            //Func.InitializeClearCheck();

            Application.Run(new Title());
        }
        #endregion
    }

    #region フォーム呼び出し
    public static partial class Func
    {

        public static void CreatePrologue(Form currentForm)
        {
            CurrentFormState.FormName = "Prologue";
            CurrentFormState.StateData.Clear();

            Prologue form = new Prologue();
            form.Show();
            if (!(currentForm is Title))
            {
                currentForm.Dispose();
            }
        }

        public static void CreateWorldMap(Form currentForm) //呼び出し方: Func.CreateWorldMap(this);
        {
            CurrentFormState.FormName = "WorldMap";
            CurrentFormState.StateData.Clear();

            WorldMap form = new WorldMap();
            form.Show();
            if (!(currentForm is Title))
            {
                currentForm.Dispose();
            }
        }

        public static void CreateAnotherWorld(Form currentForm)
        {
            CurrentFormState.FormName = "AnotherWorld";
            CurrentFormState.StateData.Clear();

            AnotherWorld form = new AnotherWorld();
            form.Show();
            if (!(currentForm is Title))
            {
                currentForm.Dispose();
            }
        }

        //public static void CreateStageSelect(Form currentForm, string worldName, int worldNumber) //呼び出し方: Func.CreateStageSelect(this,"1年生",1);
        //{
        //    CurrentFormState.FormName = "StageSelect";
        //    CurrentFormState.StateData.Clear();
        //    CurrentFormState.StateData["WorldName"] = worldName;
        //    CurrentFormState.StateData["WorldNumber"] = worldNumber;

        //    StageSelect form = new StageSelect();
        //    form.WorldName = worldName;
        //    form.WorldNumber = worldNumber;
        //    form.Show();
        //    if (!(currentForm is Title))
        //    {
        //        currentForm.Dispose();
        //    }
        //}

        public static void CreateStage(Form currentForm, string worldName, int worldNumber, int level) //呼び出し方: Func.CreateStageSelect(this,"1");  各ステージどう名付けるか決めたい
        {
            CurrentFormState.FormName = "Stage";
            CurrentFormState.StateData.Clear();
            CurrentFormState.StateData["WorldName"] = worldName;
            CurrentFormState.StateData["WorldNumber"] = worldNumber;
            CurrentFormState.StateData["Level"] = level;

            Stage form = new Stage();
            form.WorldName = worldName;
            form.WorldNumber = worldNumber;
            form.Level = level;
            form.Show();
            if (!(currentForm is Title))
            {
                currentForm.Dispose();
            }
        }

    }
    #endregion


    #region 会話
    public static partial class Func
    {
        //セリフCSV読み込み
        public static List<Conversation> LoadConversationCSV(string ConvertationCSVFileName)
        {
            List<Conversation> Conversations = new List<Conversation>();

            using (StreamReader sr = new StreamReader($"Conversation\\{ConvertationCSVFileName}"))
            {
                bool isFirstRow = true;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (isFirstRow) //１行目は要素説明のためスキップ
                    {
                        isFirstRow = false;
                        continue;
                    }

                    Conversations.Add(new Conversation(values[0], values[1], values[2]));
                }
            }

            return Conversations;
        }

        //システムメッセージCSV読み込み（立ち絵要らないならこっち、メッセージCSV１個）
        //public static List<Message> LoadMessageCSV(string MessageCSVFileName)
        //{
        //    List<Message> Messages = new List<Message>();

        //    using (StreamReader sr = new StreamReader($"{MessageCSVFileName}"))
        //    {
        //        bool isFirstRow = true;

        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            string[] values = line.Split(',');

        //            if (isFirstRow) //１行目は要素説明のためスキップ
        //            {
        //                isFirstRow = false;
        //                continue;
        //            }

        //            Messages.Add(new Message(values[0], values[1]));
        //        }
        //    }

        //    return Messages;
        //}


        //昨年版、立ち絵あり
        //public static List<Conversation> LoadMessageCSV(string MessageCSVFileName)
        //{
        //    List<Conversation> Message = new List<Conversation>();

        //    using (StreamReader sr = new StreamReader($"Conversation\\{MessageCSVFileName}"))
        //    {
        //        bool isFirstRow = true;

        //        while (!sr.EndOfStream)
        //        {
        //            string line = sr.ReadLine();
        //            string[] values = line.Split(',');

        //            if (isFirstRow) //１行目は要素説明のためスキップ
        //            {
        //                isFirstRow = false;
        //                continue;
        //            }

        //            Message.Add(new Conversation(values[0], values[1], values[2]));
        //        }
        //    }

        //    return Message;
        //}

        public static (List<Conversation>, List<Conversation>) LoadStories(string ConvFileName, string cutWord)
        {
            List<Conversation> StartConv = new List<Conversation>();
            List<Conversation> EndConv = new List<Conversation>();

            using (StreamReader sr = new StreamReader($"Story\\{ConvFileName}"))
            {
                bool isFirstRow = true;
                bool isBeforePlay = true;

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(',');

                    if (isFirstRow) //escape 1st row
                    {
                        isFirstRow = false;
                        continue;
                    }
                    if (values[1] == cutWord)
                    {
                        isBeforePlay = false;
                        continue;
                    }

                    if (isBeforePlay)
                    {
                        StartConv.Add(new Conversation(values[0], values[1], values[2]));
                    }
                    else
                    {
                        EndConv.Add(new Conversation(values[0], values[1], values[2]));
                    }
                }
            }

            return (StartConv, EndConv);
        }


        //メッセージボックス
        public static PictureBox CreatePictureBox_Conv(Form currentForm)
        {
            PictureBox pictureBox_Conv = new PictureBox();
            pictureBox_Conv.Location = new Point(0, 0);
            pictureBox_Conv.Size = new Size(1536, 900);
            currentForm.Controls.Add(pictureBox_Conv);

            return pictureBox_Conv;
        }
    }

    public struct Conversation
    {
        public string Character;
        public string Dialogue;
        public string Img;

        public Conversation(string character, string dialogue, string img)
        {
            Character = character;
            Dialogue = dialogue;
            Img = img;
        }
    }

    public struct Message
    {
        public string Situation;
        public string Dialogue;

        public Message(string situation, string dialogue)
        {
            Situation = situation;
            Dialogue = dialogue;
        }
    }

    public partial class Func
    {
        // WinAPI 関数のインポート（とりあえず丸コピ）
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);
        private const int SRCCOPY = 0x00CC0020;

        public static byte[] CaptureClientArea(Form currentForm)
        {
            Rectangle clientRect = currentForm.ClientRectangle;

            using (Graphics g = currentForm.CreateGraphics())
            {
                float scaleX = g.DpiX / 96.0f;
                float scaleY = g.DpiY / 96.0f;

                int width = (int)(clientRect.Width * scaleX);
                int height = (int)(clientRect.Height * scaleY);

                Bitmap bmp_Capt = new Bitmap(width, height);
                using (Graphics g_bmp = Graphics.FromImage(bmp_Capt))
                {
                    IntPtr hdcBitmap = g_bmp.GetHdc();
                    IntPtr hdcSrc = GetDC(currentForm.Handle);

                    BitBlt(hdcBitmap, 0, 0, width, height, hdcSrc, 0, 0, SRCCOPY);

                    ReleaseDC(currentForm.Handle, hdcSrc);
                    g_bmp.ReleaseHdc(hdcBitmap);
                }
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp_Capt.Save(ms, ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        public static Bitmap ByteArrayToBitmap(byte[] byteArray)
        {
            using (MemoryStream ms = new MemoryStream(byteArray))
            {
                return new Bitmap(ms);
            }
        }

        public static int convIndex;

        public static void DrawConv(Form currentForm, PictureBox pictureBox_Conv, byte[] Capt, List<Conversation> Conversations)
        {
            if (convIndex >= Conversations.Count)
            {
                ChangeControl(pictureBox_Conv, false);
                return;
            }

            bool isStage = currentForm is Stage;

            Bitmap bmp_Capt = ByteArrayToBitmap(Capt);
            Graphics g = Graphics.FromImage(bmp_Capt);

            Font fnt_name = new Font("游ゴシック", 33, FontStyle.Bold);
            Font fnt_dia = new Font("游ゴシック", 33);
            Brush Color_BackConv = new SolidBrush(ColorTranslator.FromHtml("#f8e58c"));
            Brush Color_BackName = new SolidBrush(ColorTranslator.FromHtml("#856859"));

            int margin_x = 15;
            int margin_y = 200;

            int sp = 5;

            int sp_x = 150;
            int sp_y = 30;

            int face = 300;
            int name_x = 300;
            int name_y = 60;

            int dia_x = 1500;
            int dia_y = 270;

            int lineHeight = fnt_dia.Height;

            if (isStage)
            {
                sp = 30;
                sp_x = 250;
                face = 200;
            }

            if (isStage)
            {
                g.DrawImage(Dictionaries.Img_Conversation["Dialogue_Stage"], margin_x, margin_y + 300 + name_y, dia_x, dia_y);
            }
            else
            {
                g.DrawImage(Dictionaries.Img_Conversation["Dialogue"], margin_x, margin_y + 300 + name_y, dia_x, dia_y);
                string charaName = Conversations[convIndex].Character;
                //キャラ名が必要かどうか
                //if (charaName == "主人公")
                //{
                //    if (MainCharacter.isBoy)
                //    {
                //        charaName = "アレックス（仮名）";
                //    }
                //    else if (MainCharacter.isGirl)
                //    {
                //        charaName = "エイミー（仮名）";
                //    }
                //    else
                //    {
                //        charaName = "シルバー（仮名）";
                //    }
                //}

                g.DrawImage(Dictionaries.Img_Conversation["Name"], margin_x, margin_y + face, name_x, name_y);
                g.DrawString(charaName, fnt_name, Brushes.White, margin_x + sp, margin_y + face + sp);
            }


            //改行の処理はこう書かないとうまくいかない
            char[] lineBreak = new char[] { '\\' };
            string[] DialogueLines = Conversations[convIndex].Dialogue.Replace("\\n", "\\").Split(lineBreak);
            for (int i = 0; i < DialogueLines.Length; i++)
            {
                g.DrawString(DialogueLines[i], fnt_dia, Brushes.Black, margin_x + sp_x, margin_y + 300 + name_y + sp_y + i * lineHeight);
            }

            Image charaImage = null;
            if (Conversations[convIndex].Img == "Main")
            {
                if (MainCharacter.isBoy)
                {
                    charaImage = Dictionaries.Img_Character["Boy"];
                }
                else if (MainCharacter.isGirl)
                {
                    charaImage = Dictionaries.Img_Character["Girl"];
                }
                else
                {
                    charaImage = Dictionaries.Img_Character["Silver"];
                }
            }
            else
            {
                charaImage = Dictionaries.Img_Character[Conversations[convIndex].Img];
            }

            if (isStage)
            {
                g.DrawImage(charaImage, margin_x + sp, margin_y + 300 + name_y + sp, face, face);
            }
            else
            {
                g.DrawImage(charaImage, margin_x, margin_y, face, face);
            }

            pictureBox_Conv.Image = bmp_Capt;
            g.Dispose();

            if (convIndex < Conversations.Count)
            {
                convIndex++;
            }
        }

        public static void ChangeControl(PictureBox pictureBox_Conv, bool isStart)
        {
            pictureBox_Conv.Enabled = isStart;
            pictureBox_Conv.Visible = isStart;
            if (isStart)
            {
                pictureBox_Conv.BringToFront();
                pictureBox_Conv.Cursor = Cursors.Hand;
            }
            else
            {
                pictureBox_Conv.SendToBack();
                pictureBox_Conv.Cursor = Cursors.Default;
            }
        }

        public static byte[] PlayConv(Form currentForm, PictureBox pictureBox_Conv, List<Conversation> Conversations)
        {
            byte[] Capt = CaptureClientArea(currentForm);
            ChangeControl(pictureBox_Conv, true);
            convIndex = 0;
            DrawConv(currentForm, pictureBox_Conv, Capt, Conversations);

            return Capt;
        }
    }

#endregion

    #region キャラ選択結果
    public partial class MainCharacter
    {
        public static bool isBoy = false;
        public static bool isGirl = false;
    }
    #endregion

    #region 各データのDictionaryと読み込み関数
    public partial class Dictionaries
        {
            public static Dictionary<string, Image> Img_Character = new Dictionary<string, Image>();
            public static Dictionary<string, Image> Img_DotPic = new Dictionary<string, Image>();
            public static Dictionary<string, Image> Img_Object = new Dictionary<string, Image>();
            public static Dictionary<string, Image> Img_Button = new Dictionary<string, Image>();
            public static Dictionary<string, Image> Img_Background = new Dictionary<string, Image>();
            public static Dictionary<string, Image> Img_Conversation = new Dictionary<string, Image>();
            public static Dictionary<string, List<Conversation>> Conversations = new Dictionary<string, List<Conversation>>();
            public static Dictionary<string, List<Message>> Messages = new Dictionary<string, List<Message>>();
        }

    public partial class Func
    {
        public static void LoadImg_Background()
        {
            Dictionaries.Img_Background.Clear();
            string[] files = Directory.GetFiles(@"Image\\Background");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Background_", "");
                Dictionaries.Img_Background[key] = Image.FromFile(file);
            }
        }

    　　public static void LoadImg_Character()
        {
            Dictionaries.Img_Character.Clear();
            string[] files = Directory.GetFiles(@"Image\\Character");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Character_", "");
                Dictionaries.Img_Character[key] = Image.FromFile(file);
            }
        }

        //メッセージウィンドウ画像読み込み
        public static void LoadImg_Conversation()
        {
            Dictionaries.Img_Conversation.Clear();
            string[] files = Directory.GetFiles(@"Image\\Conversation");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Conversation_", "");
                Dictionaries.Img_Conversation[key] = Image.FromFile(file);
            }
        }

        //DictionaryにMessageCSVのデータを追加
        public static Dictionary<string, List<Message>> LoadMessagesFromCsv()
        {
            string filePath = (@"Convertation\\Conv_Message.csv");
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] values = line.Split(',');

                    if (values.Length < 2) continue; // 不正な行をスキップ

                    string key = values[0].Trim();
                    Message message = new Message
                    {
                        Situation = key,
                        Dialogue = values[1].Trim()
                    };

                    if (!Dictionaries.Messages.ContainsKey(key))
                    {
                        Dictionaries.Messages[key] = new List<Message>();
                    }
                    Dictionaries.Messages[key].Add(message);
                }
            }

            return Dictionaries.Messages;
        }



        public static void LoadImg_DotPic()
        {
            Dictionaries.Img_DotPic.Clear();
            string charaDirectory = "";
            if (MainCharacter.isBoy)
            {
                charaDirectory = @"Image\\DotPic\\Boy";
            }
            else if (MainCharacter.isGirl)
            {
                charaDirectory = @"Image\\DotPic\\Girl";
            }
            else
            {
                charaDirectory = @"Image\\DotPic\\Silver";
            }
            string[] files = Directory.GetFiles(charaDirectory);
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_DotPic_", "");
                Dictionaries.Img_DotPic[key] = Image.FromFile(file);
            }
        }
        public static void LoadImg_Object()
        {
            Dictionaries.Img_Object.Clear();
            string[] files = Directory.GetFiles(@"Object");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Object_", "");
                Dictionaries.Img_Object[key] = Image.FromFile(file);
            }
        }
        public static void LoadImg_Button()
        {
            Dictionaries.Img_Button.Clear();
            string[] files = Directory.GetFiles(@"Image\\Button");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Button_", "");
                Dictionaries.Img_Button[key] = Image.FromFile(file);
            }
        }
        public static void LoadConversations()
        {
            Dictionaries.Conversations.Clear();
            string[] files = Directory.GetFiles(@"Conversation");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Conv_", "");
                Dictionaries.Conversations[key] = LoadConversationCSV(file);
            }
        }

    }
    #endregion

    #region 進行状況管理

    public enum ConstNum
    {
        numWorlds = 7 + 1,
        numStages = 3 + 1,
        waitTime_End = 100,
        waitTime_Load = 400
    }
    public partial class CurrentFormState
    {
        public static string FormName = "Prologue";
        public static Dictionary<string, object> StateData = new Dictionary<string, object>();
    }

    public partial class ClearCheck
    {
        //クリアチェック配列
        //0番目はそのWorldのレベル3つをすべてクリアしたらtrueにする。
        public static bool[,] IsCleared = new bool[(int)ConstNum.numWorlds, (int)ConstNum.numStages];

        //ボタン管理配列
        //0番目はWorldMapでそのボタンを押せるかどうか（押せる場合true）
        public static bool[,] IsButtonEnabled = new bool[(int)ConstNum.numWorlds, (int)ConstNum.numStages];

        //新ステージ出現チェック配列
        //0番目はWorldMapでそのワールドの中に新ステージがあるかどうか
        public static bool[,] IsNew = new bool[(int)ConstNum.numWorlds, (int)ConstNum.numStages];

        //卒業試験クリア時
        public static bool PlayAfterChapter4Story;

        //外の世界クリア時
        public static bool Completed;
        public static bool PlayAfterAnotherWorldStory;
    }

    public partial class Func
    {
        public static void InitializeClearCheck()    //Main関数で呼び出す
        {
            for (int i = 0; i < (int)ConstNum.numWorlds; i++)
            {
                for (int j = 0; j < (int)ConstNum.numStages; j++)
                {
                    ClearCheck.IsCleared[i, j] = false;
                    ClearCheck.IsButtonEnabled[i, j] = false;
                    ClearCheck.IsNew[i, j] = false;
                }
            }

            ClearCheck.PlayAfterChapter4Story = false;
            ClearCheck.PlayAfterAnotherWorldStory = false;
            ClearCheck.Completed = false;

            ClearCheck.IsButtonEnabled[1, 0] = true;
            ClearCheck.IsButtonEnabled[1, 1] = true;
        }

        public static void UpdateIsNew()    //IsNew配列の更新
        {
            for (int i = 1; i < (int)ConstNum.numWorlds; i++)
            {
                bool isNew0 = false;
                for (int j = 1; j < (int)ConstNum.numStages; j++)
                {
                    if (ClearCheck.IsNew[i, j])
                    {
                        isNew0 = true;
                        break;
                    }
                }
                ClearCheck.IsNew[i, 0] = isNew0;
            }
        }

        public static bool HasNewStageInWorld(bool isWorldMap)
        {
            // WorldMapまたはAnotherWorldに新ステージがあるかどうか
            bool hasNewStage = false;

            if (isWorldMap)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (ClearCheck.IsNew[i, 0])
                    {
                        hasNewStage = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 5; i <= 7; i++)
                {
                    if (ClearCheck.IsNew[i, 0])
                    {
                        hasNewStage = true;
                        break;
                    }
                }
            }

            return hasNewStage;
        }

        public static bool HasNewStageFromStageSelect(bool isWorldMap, int worldNumber)
        {
            // StageSelectからWorld選択に戻った時に新ステージがあるかどうか
            if (Func.HasNewStageInWorld(!isWorldMap)) return true;

            bool hasNewStage = false;

            if (isWorldMap)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (i == worldNumber) continue;
                    if (ClearCheck.IsNew[i, 0])
                    {
                        hasNewStage = true;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 5; i <= 7; i++)
                {
                    if (i == worldNumber) continue;
                    if (ClearCheck.IsNew[i, 0])
                    {
                        hasNewStage = true;
                        break;
                    }
                }
            }

            return hasNewStage;
        }

        public static bool HasNewStageInAllWorld()
        {
            // 新ステージがあるかどうか
            bool hasNewStage = false;

            for (int i = 1; i < (int)ConstNum.numWorlds; i++)
            {
                for (int j = 0; j < (int)ConstNum.numStages; j++)
                {
                    if (ClearCheck.IsNew[i, j])
                    {
                        hasNewStage = true;
                        break;
                    }
                }
            }

            return hasNewStage;
        }

        public static bool IsAllStageClearedInWorld(bool isWorldMap)
        {
            // WorldMapまたはAnotherWorldがすべてクリアされているかどうか
            bool isAllStageCleared = true;

            if (isWorldMap)
            {
                for (int i = 1; i <= 4; i++)
                {
                    if (!ClearCheck.IsCleared[i, 0])
                    {
                        isAllStageCleared = false;
                        break;
                    }
                }
            }
            else
            {
                for (int i = 5; i <= 7; i++)
                {
                    if (!ClearCheck.IsCleared[i, 0])
                    {
                        isAllStageCleared = false;
                        break;
                    }
                }
            }

            return isAllStageCleared;
        }
    }


    #endregion


}
