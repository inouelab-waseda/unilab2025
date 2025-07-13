using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static unilab2025.Program;

namespace unilab2025
{
    public partial class Prologue : Form
    {
        private PictureBox pictureBox_Conversation;
        private byte[] capturedScreen;

        // 現在の状態を管理するEnum
        private enum GameState
        {
            /// <summary>初期状態（オープニング会話再生前）</summary>
            Initial,
            /// <summary>オープニング会話中</summary>
            PlayingOpeningConversation,
            /// <summary>キャラクター選択中</summary>
            CharacterSelection,
            /// <summary>イントロダクション会話中</summary>
            PlayingIntroductionConversation
        }

        private GameState currentState = GameState.Initial;

        // キャラクター選択用PictureBox
        private PictureBox boyChoiceBox;
        private PictureBox girlChoiceBox;
        private PictureBox silverChoiceBox;

        public Prologue()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += new KeyEventHandler(Prologue_KeyDown);
            this.KeyPreview = true;
            this.BackgroundImage = Dictionaries.Img_Background["Stage1"];//背景

            //会話表示
            pictureBox_Conversation = Func.CreatePictureBox_Conv(this);
            pictureBox_Conversation.Click += PictureBox_Conversation_Click;
            //最初は非表示
            Func.ChangeControl(pictureBox_Conversation, false);

            // キャラ選択
            boyChoiceBox = new PictureBox();
            boyChoiceBox.Image = Dictionaries.Img_Character["Boy2"]; // 例: 選択画面用のBoy画像
            boyChoiceBox.Size = new Size(300, 500); // 適当なサイズに設定
            boyChoiceBox.Location = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2 );
            boyChoiceBox.SizeMode = PictureBoxSizeMode.Zoom; // 画像サイズに合わせて調整
            boyChoiceBox.Click += BoyChoiceBox_Click;
            this.Controls.Add(boyChoiceBox);
            boyChoiceBox.Visible = false; // 最初は非表示
            boyChoiceBox.Enabled = false; // 最初は無効
            boyChoiceBox.BackColor = Color.Transparent;

            girlChoiceBox = new PictureBox();
            girlChoiceBox.Image = Dictionaries.Img_Character["Girl2"]; // 例: 選択画面用のGirl画像
            girlChoiceBox.Size = new Size(300, 500);
            girlChoiceBox.Location = new Point(this.ClientSize.Width / 2+300 , this.ClientSize.Height / 2 );
            girlChoiceBox.SizeMode = PictureBoxSizeMode.Zoom;
            girlChoiceBox.Click += GirlChoiceBox_Click;
            this.Controls.Add(girlChoiceBox);
            girlChoiceBox.Visible = false;
            girlChoiceBox.Enabled = false;
            girlChoiceBox.BackColor = Color.Transparent;

            silverChoiceBox = new PictureBox();
            silverChoiceBox.Image = Dictionaries.Img_Character["Silver2"]; // 例: 選択画面用のSilver画像
            silverChoiceBox.Size = new Size(300, 500);
            silverChoiceBox.Location = new Point(this.ClientSize.Width / 2 + 600, this.ClientSize.Height / 2);
            silverChoiceBox.SizeMode = PictureBoxSizeMode.Zoom;
            silverChoiceBox.Click += SilverChoiceBox_Click;
            this.Controls.Add(silverChoiceBox);
            silverChoiceBox.Visible = false;
            silverChoiceBox.Enabled = false;
            silverChoiceBox.BackColor = Color.Transparent;
        }

        private async void Prologue_Load(object sender, EventArgs e)
        {
            
            // フォームロード時にOP会話を再生
            currentState = GameState.PlayingOpeningConversation;
            List<Conversation> opConversations = Dictionaries.Conversations["Op"];
            capturedScreen = await Func.PlayConv(this, pictureBox_Conversation, opConversations);
            
        }

        #region 諸々クリックの処理
        // 会話進行処理
        private void PictureBox_Conversation_Click(object sender, EventArgs e)
        {
            List<Conversation> currentConversations = null;

            if (currentState == GameState.PlayingOpeningConversation)
            {
                currentConversations = Dictionaries.Conversations["Op"];
            }
            else if (currentState == GameState.PlayingIntroductionConversation)
            {
                currentConversations = Dictionaries.Conversations["Intro"];
            }

            if (currentConversations != null)
            {
                // 次の会話を表示
                Func.DrawConv(this, pictureBox_Conversation, capturedScreen, currentConversations);

                // 現在の会話がすべて表示されたかチェック
                if (Func.convIndex >= currentConversations.Count)
                {
                    Func.ChangeControl(pictureBox_Conversation, false); // 会話表示を無効化

                    // 次のステップに進む
                    if (currentState == GameState.PlayingOpeningConversation)
                    {
                        // オープニング会話終了後、キャラクター選択へ
                        currentState = GameState.CharacterSelection;
                        ShowCharacterSelectionButtons(true); // キャラクター選択ボタンを表示
                    }
                    else if (currentState == GameState.PlayingIntroductionConversation)
                    {
                        // イントロ会話終了後どこ行きゃいいの
                        Func.CreateStage(this, "1年生", 1, 1);
                        this.Dispose(); // Prologueフォームを閉じる
                    }
                }
            }
        }
        #endregion

        #region　キャラ選択
        // キャラクター選択用PictureBoxの表示/非表示を切り替える
        private void ShowCharacterSelectionButtons(bool show)
        {
            boyChoiceBox.Visible = show;
            boyChoiceBox.Enabled = show;
            girlChoiceBox.Visible = show;
            girlChoiceBox.Enabled = show;
            silverChoiceBox.Visible = show;
            silverChoiceBox.Enabled = show;
        }

        // 男の子選択PictureBoxクリック時の処理
        private void BoyChoiceBox_Click(object sender, EventArgs e)
        {
            MainCharacter.isBoy = true;
            MainCharacter.isGirl = false; // GirlとSilverはfalseに
            StartIntroductionConversation();
        }

        // 女の子選択PictureBoxクリック時の処理
        private void GirlChoiceBox_Click(object sender, EventArgs e)
        {
            MainCharacter.isBoy = false;
            MainCharacter.isGirl = true;
            StartIntroductionConversation();
        }

        // Silver選択PictureBoxクリック時の処理
        private void SilverChoiceBox_Click(object sender, EventArgs e)
        {
            MainCharacter.isBoy = false; // BoyとGirlはfalseに
            MainCharacter.isGirl = false; // Silverの場合、両方falseで表現
            StartIntroductionConversation();
        }

        // イントロダクション会話を開始する
        private async void StartIntroductionConversation()
        {
            this.BackgroundImage = Dictionaries.Img_Background["Stage1"];//背景
            ShowCharacterSelectionButtons(false); // キャラクター選択PictureBoxを非表示にする

            // ドット絵の画像データを再ロードする（選択されたキャラクターに合わせて）
            Func.LoadImg_DotPic();

            currentState = GameState.PlayingIntroductionConversation;
            List<Conversation> introConversations = Dictionaries.Conversations["Intro"];
            // 現在のフォームの画面を再度キャプチャして会話を開始
            capturedScreen = await Func.PlayConv(this, pictureBox_Conversation, introConversations);
        }

        #endregion


        #region ストーリースキップ用
        //public Prologue関数内に以下を追記
        //this.KeyDown += new KeyEventHandler(Prologue_KeyDown);
        //this.KeyPreview = true;
        private void Prologue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                Func.LoadImg_DotPic();
                Func.CreateStage(this, "1年生", 1, 1);
            }
        }
        #endregion
    }
}
