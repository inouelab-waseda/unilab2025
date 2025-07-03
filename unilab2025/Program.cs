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
            Func.LoadImg_Button_MapSelect();
            Func.LoadImg_Conversation();
            Func.LoadConversations();
            Func.InitializeClearCheck();

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

        public static void CreateStageSelect(Form currentForm, string worldName, int worldNumber) //呼び出し方: Func.CreateStageSelect(this,"1年生",1);
        {
            CurrentFormState.FormName = "StageSelect";
            CurrentFormState.StateData.Clear();
            CurrentFormState.StateData["WorldName"] = worldName;
            CurrentFormState.StateData["WorldNumber"] = worldNumber;

            StageSelect form = new StageSelect();
            form.WorldName = worldName;
            form.WorldNumber = worldNumber;
            form.Show();
            if (!(currentForm is Title))
            {
                currentForm.Dispose();
            }
        }

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
    //会話データ読込
    public static partial class Func
    {
        //セリフCSV読み込み
        public static List<Conversation> LoadConversationCSV(string ConvertationCSVFileName)
        {
            List<Conversation> Conversations = new List<Conversation>();

            using (StreamReader sr = new StreamReader($"{ConvertationCSVFileName}"))
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
            pictureBox_Conv.BackColor = Color.Transparent;

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

    //表示関連
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

        //ルビ表示用
        public static void DrawStringWithRuby(Graphics g, string text, Font baseFont, Font rubyFont, Brush brush, PointF startPoint)
        {
            //横幅測定用（これないと漢字の横に謎空白できる）
            StringFormat format = new StringFormat(StringFormat.GenericTypographic);
            format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;

            float currentX = startPoint.X;
            float currentY = startPoint.Y;

            // 書式 |漢字(かんじ)| を見つけるための正規表現
            Regex rubyRegex = new Regex(@"\｜(.*?)\（(.*?)\）\｜");

            int lastIndex = 0;

            foreach (Match match in rubyRegex.Matches(text))
            {
                // ルビ指定の前に通常のテキストがあれば描画
                if (match.Index > lastIndex)
                {
                    string normalText = text.Substring(lastIndex, match.Index - lastIndex);
                    g.DrawString(normalText, baseFont, brush, new PointF(currentX, currentY));
                    currentX += g.MeasureString(normalText, baseFont, PointF.Empty, format).Width;
                }

                string baseText = match.Groups[1].Value; // 親文字 (例: 漢字)
                string rubyText = match.Groups[2].Value; // ルビ (例: かんじ)

                // 親文字とルビのサイズを計測
                SizeF baseSize = g.MeasureString(baseText, baseFont, PointF.Empty, format);
                SizeF rubySize = g.MeasureString(rubyText, rubyFont, PointF.Empty, format);

                // ルビを親文字の中央上に描画
                float rubyX = currentX + (baseSize.Width - rubySize.Width) / 2 + 3;
                float rubyY = currentY - rubySize.Height + 10; // 親文字の上に来るように調整
                g.DrawString(rubyText, rubyFont, brush, new PointF(rubyX, rubyY));

                // 親文字を描画
                g.DrawString(baseText, baseFont, brush, new PointF(currentX, currentY));
                currentX += baseSize.Width;

                lastIndex = match.Index + match.Length;
            }

            // 最後に残った通常のテキストを描画
            if (lastIndex < text.Length)
            {
                string remainingText = text.Substring(lastIndex);
                g.DrawString(remainingText, baseFont, brush, new PointF(currentX, currentY));
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

            Bitmap bmp_Capt = ByteArrayToBitmap(Capt);
            Graphics g = Graphics.FromImage(bmp_Capt);

            Font fnt_name = new Font("游ゴシック", 30);
            Font fnt_dia = new Font("游ゴシック", 30);
            Font fnt_ruby = new Font("游ゴシック", 12);

            bool showCharacterArea = !string.IsNullOrEmpty(Conversations[convIndex].Img);

            if (showCharacterArea)
            {
                // キャラクター有りモードのレイアウト
                // ウィンドウ座標
                int dia_x = 1300;
                int dia_y = 270;
                int margin_x = (1400 - dia_x) / 2; // 画面中央に配置
                int margin_y = 400;
                int lineHeight = fnt_dia.Height;

                // ウィンドウ内部の余白設定
                int face_icon_size = 200;
                int padding_left = 40;
                int text_start_offset_x = face_icon_size + padding_left;
                int text_start_offset_y = 70;

                // ウィンドウ描画
                g.DrawImage(Dictionaries.Img_Conversation["MessageWindow"], margin_x, margin_y, dia_x, dia_y);

                // キャラ名表示
                string charaName = Conversations[convIndex].Character;
                // 名前の描画位置 (ウィンドウ左上からのオフセット。お好みで調整してください)
                Point namePosition = new Point(margin_x + 55, margin_y + 12);
                g.DrawString(charaName, fnt_name, Brushes.Black, namePosition);

                // キャラクターアイコン画像描画
                Image charaImage = null;
                string imgKey = Conversations[convIndex].Img;
                if (imgKey == "Main")
                {
                    if (MainCharacter.isBoy) charaImage = Dictionaries.Img_Character["Boy"];
                    else if (MainCharacter.isGirl) charaImage = Dictionaries.Img_Character["Girl"];
                    else charaImage = Dictionaries.Img_Character["Silver"];
                }
                else if (Dictionaries.Img_Character.ContainsKey(imgKey))
                {
                    charaImage = Dictionaries.Img_Character[imgKey];
                }

                if (charaImage != null)
                {
                    // 元画像の縦横比を維持した描画サイズを計算
                    float aspectRatio = (float)charaImage.Width / charaImage.Height;

                    float newWidth, newHeight;
                    if (charaImage.Width > charaImage.Height)
                    {
                        // 画像が横長の場合、幅を基準に高さを計算
                        newWidth = face_icon_size;
                        newHeight = newWidth / aspectRatio;
                    }
                    else
                    {
                        // 画像が縦長または正方形の場合、高さを基準に幅を計算
                        newHeight = face_icon_size;
                        newWidth = newHeight * aspectRatio;
                    }

                    // アイコン表示領域を計算します（ウィンドウ内で垂直中央揃え）。
                    RectangleF iconArea = new RectangleF(
                        margin_x + padding_left,
                        margin_y + (dia_y - face_icon_size) / 2,
                        face_icon_size,
                        face_icon_size
                    );

                    // アイコン表示領域の中央に画像を描画するための座標計算。
                    float drawX = iconArea.X + (iconArea.Width - newWidth) / 2 - 20;
                    float drawY = iconArea.Y + (iconArea.Height - newHeight) / 2 + 30;

                    // 計算した位置とサイズで画像を描画
                    g.DrawImage(charaImage, drawX, drawY, newWidth, newHeight);
                }

                // セリフの描画
                char[] lineBreak = new char[] { '\\' };
                string[] DialogueLines = Conversations[convIndex].Dialogue.Replace("\\n", "\\").Split(lineBreak);
                for (int i = 0; i < DialogueLines.Length; i++)
                {
                    PointF startPoint = new PointF(
                        margin_x + text_start_offset_x,
                        margin_y + text_start_offset_y + i * lineHeight + fnt_ruby.Height
                    );
                    DrawStringWithRuby(g, DialogueLines[i], fnt_dia, fnt_ruby, Brushes.Black, startPoint);
                }
            }
            else
            {
                // ナレーターモード（キャラクター無し）のレイアウト
                int dia_x = 1300;
                int dia_y = 270;
                int margin_x = (1400 - dia_x) / 2;
                int margin_y = 400;
                int lineHeight = fnt_dia.Height;
                int textPaddingX = 60;
                int sp_y = 70;

                g.DrawImage(Dictionaries.Img_Conversation["MessageWindow"], margin_x, margin_y, dia_x, dia_y);

                char[] lineBreak = new char[] { '\\' };
                string[] DialogueLines = Conversations[convIndex].Dialogue.Replace("\\n", "\\").Split(lineBreak);
                for (int i = 0; i < DialogueLines.Length; i++)
                {
                    PointF startPoint = new PointF(margin_x + textPaddingX, margin_y + sp_y + i * lineHeight + fnt_ruby.Height);
                    DrawStringWithRuby(g, DialogueLines[i], fnt_dia, fnt_ruby, Brushes.Black, startPoint);
                }
            }

            // 最後の描画処理
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
            public static Dictionary<string, Image> Img_Button_MapSelect = new Dictionary<string, Image>();
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
        public static void LoadImg_Button_MapSelect()
        {
            Dictionaries.Img_Button_MapSelect.Clear();
            string[] files = Directory.GetFiles(@"Image\\Button_MapSelect");
            foreach (string file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Button_MapSelect_", "");
                Dictionaries.Img_Button_MapSelect[key] = Image.FromFile(file);
            }
        }

    }
    #endregion

    #region 進行状況管理

    public enum ConstNum
    {
        numWorlds = 8 + 1,
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


    #region カスタムボタン（文字の上に画像を描画する）
    public class CustomButton : Button
    {
        private Image foreImage;

        public Image ForeImage
        {
            get { return foreImage; }
            set
            {
                foreImage = value;
                Invalidate();
            }
        }

        private Image conditionImage;

        public Image ConditionImage
        {
            get { return conditionImage; }
            set
            {
                conditionImage = value;
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            //ボタンのベース描画
            base.OnPaint(pevent);

            //文字の描画
            TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, this.ClientRectangle, this.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);

            //ボタンサイズ
            int buttonWidth = this.Width;
            int buttonHeight = this.Height;

            //背景画像を文字の上に描画
            if (this.ForeImage != null)
            {
                //Zoomレイアウトで背景画像を描画
                //画像サイズ
                int imageWidth = this.ForeImage.Width;
                int imageHeight = this.ForeImage.Height;

                //縦横比を保ちながらスケーリング
                float scale = Math.Min((float)buttonWidth / imageWidth, (float)buttonHeight / imageHeight);
                int scaleWidth = (int)(imageWidth * scale);
                int scaleHeight = (int)(imageHeight * scale);

                //位置調整
                int x = (buttonWidth - scaleWidth) / 2;
                int y = (buttonHeight - scaleHeight) / 2;
                Rectangle destRect = new Rectangle(x, y, scaleWidth, scaleHeight);

                pevent.Graphics.DrawImage(this.ForeImage, destRect);
            }

            if (this.ConditionImage != null)
            {
                //画像サイズ
                int imageWidth = this.ConditionImage.Width;
                int imageHeight = this.ConditionImage.Height;

                // 表示領域の大きさ指定
                int scaleHeight = buttonHeight / 4;
                double scale = (double)scaleHeight / imageHeight;
                int scaleWidth = (int)(scale * imageWidth);
                Rectangle destRect = new Rectangle(0, 0, scaleWidth, scaleHeight);

                pevent.Graphics.DrawImage(this.ConditionImage, destRect);
            }
        }
    }
    #endregion

}
