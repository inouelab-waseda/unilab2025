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
            Func.LoadImg_Button();
            Func.LoadImg_Background();
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
    #endregion

    #region キャラ選択結果
    public partial class MainCharacter
    {
        public static bool isBoy = true;
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
        public static Dictionary<string, List<Conversation>> Messages = new Dictionary<string, List<Conversation>>();
    }

    public partial class Func
    {
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

        public static void LoadImg_DotPic()
        {
            Dictionaries.Img_DotPic.Clear();
            string charaDirectory = "";
            if (MainCharacter.isBoy)
            {
                charaDirectory = @"Image\\DotPic\\Boy";
            }
            else
            {
                charaDirectory = @"Image\\DotPic\\Girl";
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

    //    public static void LoadImg_Conversation()
    //    {
    //        Dictionaries.Img_Conversation.Clear();
    //        string[] files = Directory.GetFiles(@"Image\\Conversation");
    //        foreach (string file in files)
    //        {
    //            string key = Path.GetFileNameWithoutExtension(file).Replace("Img_Conversation_", "");
    //            Dictionaries.Img_Conversation[key] = Image.FromFile(file);
    //        }
    //    }

    //    public static void LoadMessages()
    //    {
    //        Dictionaries.Messages.Clear();
    //        string[] files = Directory.GetFiles(@"Message");
    //        foreach (string file in files)
    //        {
    //            string key = Path.GetFileNameWithoutExtension(file).Replace("Message_", "");
    //            Dictionaries.Messages[key] = LoadMessageCSV(file);
    //        }
    //    }
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
