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
            PlayingIntroductionConversation,
            PlayingEasy,
            EasySelection
        }

        private GameState currentState = GameState.Initial;

        // キャラクター選択用PictureBox
        private PictureBox boyChoiceBox;
        private PictureBox girlChoiceBox;
        private PictureBox silverChoiceBox;
        private Label centerLabel;
        private Button Button_easy;
        private Button Button_hard;

        public Prologue()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.KeyDown += new KeyEventHandler(Prologue_KeyDown);
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            this.BackgroundImage = Dictionaries.Img_Background["Character_select"];//背景

            //会話表示
            pictureBox_Conversation = Func.CreatePictureBox_Conv(this);
            pictureBox_Conversation.Click += PictureBox_Conversation_Click;
            //最初は非表示
            Func.ChangeControl(pictureBox_Conversation, false);

            // キャラ選択
            boyChoiceBox = new PictureBox();
            boyChoiceBox.Image = Dictionaries.Img_Character["boy_select"]; // 例: 選択画面用のBoy画像
            boyChoiceBox.Size = new Size(300, 500); // 適当なサイズに設定
            boyChoiceBox.Location = new Point(this.ClientSize.Width / 2-20, this.ClientSize.Height / 2 );
            boyChoiceBox.SizeMode = PictureBoxSizeMode.Zoom; // 画像サイズに合わせて調整
            boyChoiceBox.Click += BoyChoiceBox_Click;
            this.Controls.Add(boyChoiceBox);
            boyChoiceBox.Visible = false; // 最初は非表示
            boyChoiceBox.Enabled = false; // 最初は無効
            boyChoiceBox.BackColor = Color.Transparent;
            this.boyChoiceBox.MouseEnter += new System.EventHandler(this.boyChoiceBox_MouseEnter);
            this.boyChoiceBox.MouseLeave += new System.EventHandler(this.boyChoiceBox_MouseLeave);

            girlChoiceBox = new PictureBox();
            girlChoiceBox.Image = Dictionaries.Img_Character["girl_select"]; // 例: 選択画面用のGirl画像
            girlChoiceBox.Size = new Size(300, 500);
            girlChoiceBox.Location = new Point(this.ClientSize.Width / 2+300 , this.ClientSize.Height / 2 );
            girlChoiceBox.SizeMode = PictureBoxSizeMode.Zoom;
            girlChoiceBox.Click += GirlChoiceBox_Click;
            this.Controls.Add(girlChoiceBox);
            girlChoiceBox.Visible = false;
            girlChoiceBox.Enabled = false;
            girlChoiceBox.BackColor = Color.Transparent;
            this.girlChoiceBox.MouseEnter += new System.EventHandler(this.girlChoiceBox_MouseEnter);
            this.girlChoiceBox.MouseLeave += new System.EventHandler(this.girlChoiceBox_MouseLeave);

            silverChoiceBox = new PictureBox();
            silverChoiceBox.Image = Dictionaries.Img_Character["Silver_select"]; // 例: 選択画面用のSilver画像
            silverChoiceBox.Size = new Size(300, 500);
            silverChoiceBox.Location = new Point(this.ClientSize.Width / 2 + 620, this.ClientSize.Height / 2);
            silverChoiceBox.SizeMode = PictureBoxSizeMode.Zoom;
            silverChoiceBox.Click += SilverChoiceBox_Click;
            this.Controls.Add(silverChoiceBox);
            silverChoiceBox.Visible = false;
            silverChoiceBox.Enabled = false;
            silverChoiceBox.BackColor = Color.Transparent;
            this.silverChoiceBox.MouseEnter += new System.EventHandler(this.silverChoiceBox_MouseEnter);
            this.silverChoiceBox.MouseLeave += new System.EventHandler(this.silverChoiceBox_MouseLeave);

            //言葉の表示
            centerLabel = new Label();
            centerLabel.Text = "すきなキャラクターをえらんでね";
            centerLabel.Font = new Font("Meiryo UI", 30F, FontStyle.Bold); // フォントとサイズ
            centerLabel.ForeColor = Color.White; // 文字色（必要に応じて）
            centerLabel.BackColor = Color.Transparent; // 背景を透過
            centerLabel.AutoSize = true; // テキストのサイズに合わせる
            centerLabel.Location = new Point((this.ClientSize.Width - centerLabel.Width) / 2+40,(this.ClientSize.Height - centerLabel.Height) / 3 );
            centerLabel.Visible = false;
            centerLabel.BringToFront();
            this.Controls.Add(centerLabel);

            Button_easy = new Button();
            Button_easy.Text = "簡単";
            Button_easy.Size = new Size(200, 200);
            Button_easy.Location = new Point(this.ClientSize.Width / 2 + 150, this.ClientSize.Height / 2 + 100); // フォーム内の位置
            Button_easy.Font = new Font("Meiryo UI", 14); // フォント設定
            Button_easy.BackColor = Color.LightBlue;     // 背景色
            Button_easy.Visible = false;
            Button_easy.Click += Button_easy_Click;
            this.Controls.Add(Button_easy); // フォームに追加

            Button_hard = new Button();
            Button_hard.Text = "難しい";
            Button_hard.Size = new Size(200, 200);
            Button_hard.Location = new Point(this.ClientSize.Width / 2 + 550, this.ClientSize.Height / 2 + 100); // フォーム内の位置
            Button_hard.Font = new Font("Meiryo UI", 14); // フォント設定
            Button_hard.BackColor = Color.LightBlue;     // 背景色
            Button_hard.Visible = false;
            Button_hard.Click += Button_hard_Click;
            this.Controls.Add(Button_hard); // フォームに追加


        }

        private async void Prologue_Load(object sender, EventArgs e)
        {

            // フォームロード時にOP会話を再生
            currentState = GameState.PlayingEasy;
            List<Conversation> EazyConversations = Dictionaries.Conversations["Op_0"];
            capturedScreen = await Func.PlayConv(this, pictureBox_Conversation, EazyConversations);

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
            else if (currentState == GameState.PlayingEasy)
            {
                currentConversations = Dictionaries.Conversations["Op_0"];
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
                        centerLabel.Visible = true;
                        ShowCharacterSelectionButtons(true); // キャラクター選択ボタンを表示
                    }
                    else if (currentState == GameState.PlayingIntroductionConversation)
                    {
                        // イントロ会話終了後どこ行きゃいいの
                        Func.CreateStage(this, "1年生", 1, 1);
                        this.Dispose(); // Prologueフォームを閉じる
                    }
                    else if (currentState == GameState.PlayingEasy)
                    {

                        currentState = GameState.EasySelection;
                        Button_easy.Visible = true;
                        Button_hard.Visible = true;

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
            centerLabel.Visible=false;
            StartIntroductionConversation();
        }

        // 女の子選択PictureBoxクリック時の処理
        private void GirlChoiceBox_Click(object sender, EventArgs e)
        {
            MainCharacter.isBoy = false;
            MainCharacter.isGirl = true;
            centerLabel.Visible = false;
            StartIntroductionConversation();
        }

        // Silver選択PictureBoxクリック時の処理
        private void SilverChoiceBox_Click(object sender, EventArgs e)
        {
            MainCharacter.isBoy = false; // BoyとGirlはfalseに
            MainCharacter.isGirl = false; // Silverの場合、両方falseで表現
            centerLabel.Visible = false;
            StartIntroductionConversation();
        }
        private void Button_easy_Click(object sender, EventArgs e)
        {
            MainDifficult.isEasy = true;
            StartOPConversation();
        }

        private void Button_hard_Click(object sender, EventArgs e)
        {
            MainDifficult.isHard = true;
            StartOPConversation();
        }
        private async void StartOPConversation()
        {
            Button_easy.Visible = false;
            Button_hard.Visible = false;
            currentState = GameState.PlayingOpeningConversation;
            List<Conversation> OpConversations = Dictionaries.Conversations["Op"];
            capturedScreen = await Func.PlayConv(this, pictureBox_Conversation, OpConversations);
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

        #region　ボタン表示

        private void boyChoiceBox_MouseEnter(object sender, EventArgs e)
        {
            // マウスが上に乗ったら画像を切り替える
            boyChoiceBox.Image = Dictionaries.Img_Character["boy_select2"];
        }

        private void boyChoiceBox_MouseLeave(object sender, EventArgs e)
        {
            // マウスが離れたら元に戻す
            boyChoiceBox.Image = Dictionaries.Img_Character["boy_select"];
        }
        private void girlChoiceBox_MouseEnter(object sender, EventArgs e)
        {
            // マウスが上に乗ったら画像を切り替える
            girlChoiceBox.Image = Dictionaries.Img_Character["girl_select2"];
        }

        private void girlChoiceBox_MouseLeave(object sender, EventArgs e)
        {
            // マウスが離れたら元に戻す
            girlChoiceBox.Image = Dictionaries.Img_Character["girl_select"];
        }
        private void silverChoiceBox_MouseEnter(object sender, EventArgs e)
        {
            // マウスが上に乗ったら画像を切り替える
            silverChoiceBox.Image = Dictionaries.Img_Character["Silver_select2"];
        }

        private void silverChoiceBox_MouseLeave(object sender, EventArgs e)
        {
            // マウスが離れたら元に戻す
            silverChoiceBox.Image = Dictionaries.Img_Character["Silver_select"];
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
