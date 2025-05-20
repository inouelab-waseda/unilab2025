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
            Application.Run(new Title());
        }
        #endregion

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
        }
        #endregion

        #region 会話
        public static partial class ConversationsFunc
        {
            //セリフCSV読み込み
            public static List<Conversation> LoadConvertationCSV(string ConvertationCSVFileName)
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
            public static List<Message> LoadMessageCSV(string MessageCSVFileName)
            {
                List<Message> Messages = new List<Message>();

                using (StreamReader sr = new StreamReader($"{MessageCSVFileName}"))
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

                        Messages.Add(new Message(values[0], values[1]));
                    }
                }

                return Messages;
            }


            //昨年版、立ち絵あり
            //public static List<Conversation> LoadMessageCSV(string MessageCSVFileName)
            //{
            //    List<Conversation> Message = new List<Conversation>();

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

            //            Message.Add(new Conversation(values[0], values[1], values[2]));
            //        }
            //    }

            //    return Message;
            //}

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
            public static Dictionary<string, List<Conversation>> Convertations = new Dictionary<string, List<Conversation>>();
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
        }

        #endregion


        #region 進行状況管理
        public partial class CurrentFormState
        {
            public static string FormName = "Prologue";
            public static Dictionary<string, object> StateData = new Dictionary<string, object>();
        }

        #endregion


    }
}
