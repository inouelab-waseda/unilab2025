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
    public partial class MiniGame_Nono : Form
    {

        public MiniGame_Nono()
        {
            InitializeComponent();
        }

        private void MiniGame_Nono_Load(object sender, EventArgs e)
        {
            UpdateLevelButtons();
        }

#region　ボタンクリック
        private void button_StartNono_Click(object sender, EventArgs e)
        {
            // タイトルと「はじめる」ボタンを非表示にする
            button_StartNono.Visible = false;

            // 非表示だったレベル選択ボタンをすべて表示する
            for (int i = 1; i <= 6; i++)
            {
                var buttons = this.Controls.Find("button_NonoLevel" + i, true);
                if (buttons.Length > 0)
                {
                    buttons[0].Visible = true;
                }
            }
        }


        // レベル選択ボタン（9個共通）のクリック処理
        private void LevelButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int stageId = int.Parse(clickedButton.Name.Replace("button_Level", ""));

            // usingブロックで、ゲームフォームをダイアログとして開く
            using (var gameForm = new MiniGame_Nono_Stage())
            {
                gameForm.StageId = stageId;
                var result = gameForm.ShowDialog(); // ゲーム画面が閉じられるまでここで待機

                // もしゲーム画面が「クリアした」という結果を返したら
                if (result == DialogResult.OK)
                {
                    UpdateLevelButtons();
                }
            }
        }

#endregion

        // レベル選択ボタンの見た目を更新する
        private void UpdateLevelButtons()
        {
            for (int i = 1; i <= 6; i++)
            {
                var buttons = this.Controls.Find("button_Level" + i, true);
                if (buttons.Length > 0 && buttons[0] is Button levelButton)
                {
                    if (NonogramProgress.IsCleared[i])
                    {
                        // levelButton.BackgroundImage = ...; // クリア済み画像など
                    }
                    else
                    {
                        // levelButton.BackgroundImage = ...; // クリア前画像など
                    }
                }
            }
        }

    }
    public static class NonogramProgress
    {
        // クリアチェック
        public static bool[] IsCleared = new bool[7];
    }

}
