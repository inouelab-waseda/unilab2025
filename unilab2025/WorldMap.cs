using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class WorldMap : Form
    {
        private PictureBox pictureBox_Conv;
        private List<Conversation> currentConversation;
        private byte[] Capt;

        public WorldMap()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.KeyDown += new KeyEventHandler(WorldMap_KeyDown);
            this.KeyPreview = true;

            pictureBox_Conv = Func.CreatePictureBox_Conv(this);
            pictureBox_Conv.Click += new EventHandler(pictureBox_Conv_Click);
            pictureBox_Conv.Visible = false;
        }

        #region 読み込み時
        private async void WorldMap_Load(object sender, EventArgs e)
        {
            int Map = 5;
            if (!(ClearCheck.IsCleared[1, 0])) 
            { 
                button2.Visible = false;
                Map -= 1;
            }
            if (!(ClearCheck.IsCleared[2, 0]))
            {
                button3.Visible = false;
                Map -= 1;
            }
            if (!(ClearCheck.IsCleared[3, 0]))
            {
                button4.Visible = false;
                Map -= 1;
            }
            if (!(ClearCheck.IsCleared[4, 0]))
            {
                button5.Visible = false;
                Map -= 1;
            }
            
            this.BackgroundImage= Dictionaries.Img_Background["Map_" + Map];
            //pictureBox_Map.BackgroundImage= Dictionaries.Img_Background["Map"+Map];
            pictureBox_Map.Visible = false;


            // buttonに対する処理
            foreach (Control control in this.Controls)
            {
                if (control is CustomButton button)
                {
                    string NameWithoutButton = button.Name.Replace("button", "");
                    if (int.TryParse(NameWithoutButton, out int i))
                    {
                        button.ForeImage = null;
                        if (ClearCheck.IsNew[i, 0])
                        {                            
                            button.ConditionImage = Dictionaries.Img_Button["New"];
                        }                        
                        else
                        {
                            button.ConditionImage = null;
                        }

                    }

                }
            }

            // 画面遷移後に再生する会話があったら再生
            if (CurrentFormState.NextConversationTrigger == "PLAY")
            {
                // フラグ消去
                CurrentFormState.NextConversationTrigger = null;

                // 会話再生
                currentConversation = Func.GetNextSegment(this);
                if (currentConversation != null && currentConversation.Count > 0)
                {
                    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
                }
            }
        }
        #endregion

        #region button押下後の処理

        private void buttonI_Click(object sender, EventArgs e)
        {
            CustomButton button = sender as CustomButton;
            if (button != null)
            {
                string NameWithoutButton = button.Name.Replace("button", "");
                if (int.TryParse(NameWithoutButton, out int i))
                {
                    if (!ClearCheck.IsButtonEnabled[i, 0]) return;
                    Func.CreateStageSelect(this, button.Text, i);
                }
            }
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Func.CreateAnotherWorld(this);
        }

        #endregion

        #region クリアチェックスキップ用
        private void WorldMap_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.M)
            {
                for (int i = 1; i <= 4; i++)
                {
                    for (int j = 0; j < (int)ConstNum.numStages; j++)
                    {
                        ClearCheck.IsNew[i, j] = false;
                        ClearCheck.IsCleared[i, j] = true;
                        ClearCheck.IsButtonEnabled[i, j] = true;
                    }
                }

                ClearCheck.PlayAfterChapter4Story = true;

                for (int i = 5; i < (int)ConstNum.numWorlds; i++)
                {
                    for (int j = 0; j <= 1; j++)
                    {
                        ClearCheck.IsNew[i, j] = true;
                        ClearCheck.IsButtonEnabled[i, j] = true;
                    }
                }

                Func.CreateWorldMap(this);
            }
        }
        #endregion

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
            AdvanceConversation();
        }




    }
}
