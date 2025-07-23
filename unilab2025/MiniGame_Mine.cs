using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class MiniGame_Mine : Form
    {
        // セルの状態を管理する構造体
        private struct Cell
        {
            public bool isMine;
            public bool isRevealed;
            public bool isFlagged;
            public int adjacentMines;
        }

        // --- ゲーム設定 ---
        public int gridSize_x = 20; // グリッドのサイズ (10x10)
        public int gridSize_y = 20;
        public int mineCount = 40; // 地雷の数
        public int wide_x=10;
        public int wide_y = 10;

        public int Location_x;
        public int Location_y;

        private Cell[,] grid;       // セルのロジックを管理する2次元配列
        private Button[,] buttons;  // 画面に表示するボタンの2次元配列
        private bool isFirstClick;  // 最初のクリックかどうかを判定するフラグ
        private bool isGameOver;    // ゲームオーバー状態を管理するフラグ

        private Stopwatch gameStopwatch;//時間

        private Label lblGameOverTitle;
        private Label lblClearTitle;

        PictureBox pictureBox_Conv;
        byte[] Capt;
        List<Conversation> Message;
        bool isMessageMode;
        public List<Conversation> currentConversation;

        Panel instructionPanel = new Panel();
        Panel gameOverPanel = new Panel();


        public MiniGame_Mine()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            gameStopwatch = new Stopwatch();
            instructionPanel.Visible = false;

            

        }

        private async void minesweeper_Load(object sender, EventArgs e)
        {
            Location_x = this.ClientSize.Width;
            Location_y= this.ClientSize.Height;           

            settingsPanel.Location = new Point(
                (Location_x - settingsPanel.Width) / 2,
                (Location_y - settingsPanel.Height) / 2);

            numericUpDown1.Value = 20;
            numericUpDown2.Value = 20;
            numericUpDown3.Value = 40;
            comboBox1.SelectedItem = "たくさん";

            //currentConversation = Dictionaries.Conversations["mine"];
            //if (currentConversation != null && currentConversation.Count > 0)
            //{
            //    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
            //}
            LayoutControls();
            InstructionPanel();
            CreateGameOver();
            //InitializeGame();
            button_explain.Enabled = false;
            button_Reset.Enabled = false;
        }

        private void LayoutControls()
        {
            // PictureBoxをフォームの中央に配置
            pictureBox1.Size = new Size(gridSize_x * 30 + 100, gridSize_y * 30 + 200);
            pictureBox1.Location = new Point(
                (Location_x - pictureBox1.Width) / 2,
                (Location_y - pictureBox1.Height) / 2);
            pictureBox1.Update(); // UI再描画を明示的に促す
        }


        // ゲームの初期化
        private void InitializeGame()
        {
            
            this.Text = "Minesweeper";
                                  

            // グリッドとボタンの配列を作成
            grid = new Cell[gridSize_x, gridSize_y];
            buttons = new Button[gridSize_x, gridSize_y];
            isFirstClick = true;
            isGameOver = false;


            int offsetX = 50; // フォームの左端から50ピクセル右にずらす
            int offsetY = 100; // メニューバーの下から100ピクセル下にずらす


            // ボタンを動的に生成してフォームに配置
            for (int x = 0; x < gridSize_x; x++)
            {
                for (int y = 0; y < gridSize_y; y++)
                {
                    buttons[x, y] = new Button
                    {
                        Size = new Size(30, 30),
                        Location = new Point(x * 30 + offsetX, y * 30 + 30 + offsetY),
                        Font = new Font("Arial", 12, FontStyle.Bold),
                        Tag = new Point(x, y) // ボタンに座標を記憶させる
                    };
                    buttons[x, y].MouseUp += Cell_MouseUp; // マウスイベントハンドラを追加
                    pictureBox1.Controls.Add(buttons[x, y]);
                }
            }
            this.ClientSize = new Size(gridSize_x * 30 + offsetX, gridSize_y * 30 + 30 + offsetY);

        }

        // ゲームのリセット

        private void button_Reset_Click(object sender, EventArgs e)
        {
            ResetGame();
        }
        private void ResetGame()
        {
            timer1.Stop(); // 念のためタイマーを停止
            gameStopwatch.Reset();// 経過時間をリセット
            label_Time.Text = "Time: 00:00.00"; // ラベル表示をリセット

            this.SuspendLayout();
            pictureBox1.SuspendLayout();
            // 古いボタンを削除
            pictureBox1.Controls.Clear();
            LayoutControls();
            InitializeGame();

            pictureBox1.ResumeLayout();
            this.ResumeLayout();
        }

        // 地雷の配置（最初のクリック後に行う）
        private void PlaceMines(int firstClickX, int firstClickY)
        {
            var random = new Random();
            int minesPlaced = 0;
            while (minesPlaced < mineCount)
            {
                int x = random.Next(gridSize_x);
                int y = random.Next(gridSize_y);

                // 最初のクリック箇所と地雷の場所が重ならず、まだ地雷がない場合
                if ((x != firstClickX || y != firstClickY) && !grid[x, y].isMine)
                {
                    grid[x, y].isMine = true;
                    minesPlaced++;
                }
            }
        }

        // 隣接する地雷の数を計算
        private void CalculateAdjacentMines()
        {
            for (int x = 0; x < gridSize_x; x++)
            {
                for (int y = 0; y < gridSize_y; y++)
                {
                    if (grid[x, y].isMine) continue;

                    int count = 0;
                    // 周囲8セルをチェック
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (dx == 0 && dy == 0) continue;

                            int nx = x + dx;
                            int ny = y + dy;

                            // グリッドの範囲内かチェック
                            if (nx >= 0 && nx < gridSize_x && ny >= 0 && ny < gridSize_y && grid[nx, ny].isMine)
                            {
                                count++;
                            }
                        }
                    }
                    grid[x, y].adjacentMines = count;
                }
            }
        }

        // セルがクリックされた時の処理（左クリック・右クリック）
        private void Cell_MouseUp(object sender, MouseEventArgs e)
        {
            if (isGameOver) return;

            Button clickedButton = sender as Button;
            Point coords = (Point)clickedButton.Tag;
            int x = coords.X;
            int y = coords.Y;

            // --- 左クリックの処理 ---
            if (e.Button == MouseButtons.Left && !grid[x, y].isFlagged)
            {
                // 最初のクリックの場合、地雷を配置して数字を計算
                if (isFirstClick)
                {
                    PlaceMines(x, y);
                    CalculateAdjacentMines();
                    isFirstClick = false;
                    timer1.Start();
                    gameStopwatch.Start();
                }

                if (grid[x, y].isMine)
                {
                    GameOver(false); // 地雷を踏んだので負け
                }
                else
                {
                    if (grid[x, y].adjacentMines == 0)
                    {
                        // クリック地点を中心とした10x10の範囲を計算
                        // (Math.Max/Minでグリッドの端をはみ出さないように調整)
                        // 中心から±5マスで10x10の範囲
                        int minX = Math.Max(0, x - wide_x);
                        int maxX = Math.Min(gridSize_x - 1, x + wide_x);
                        int minY = Math.Max(0, y - wide_y);
                        int maxY = Math.Min(gridSize_y - 1, y + wide_y);

                        // 範囲を引数として渡してセルを開く
                        RevealCell(x, y, minX, maxX, minY, maxY);
                    }
                    else
                    {
                        // 0以外のセルは、そのマスだけを開く
                        grid[x, y].isRevealed = true;
                        buttons[x, y].Enabled = false;
                        buttons[x, y].BackColor = Color.LightGray;
                        buttons[x, y].Text = grid[x, y].adjacentMines.ToString();
                        buttons[x, y].ForeColor = GetNumberColor(grid[x, y].adjacentMines);
                    }
                    CheckForWin();
                }
            }
            // --- 右クリックの処理 ---
            else if (e.Button == MouseButtons.Right)
            {
                if (grid[x, y].isRevealed) return; // すでに開いているセルは無視

                grid[x, y].isFlagged = !grid[x, y].isFlagged;
                clickedButton.Text = grid[x, y].isFlagged ? "🚩" : "";
                clickedButton.ForeColor = Color.Red;
            }
        }

        // セルを開く処理（再帰的に開く）
        private void RevealCell(int x, int y, int minX, int maxX, int minY, int maxY)
        {
            // グリッドの範囲外、または既に開いているセル、または指定範囲外は処理しない
            if (x < minX || x > maxX || y < minY || y > maxY || grid[x, y].isRevealed)
            {
                return;
            }
            if (grid[x, y].isFlagged) return;

            grid[x, y].isRevealed = true;
            Button button = buttons[x, y];
            button.Enabled = false;
            button.BackColor = Color.LightGray;

            if (grid[x, y].adjacentMines > 0)
            {
                button.Text = grid[x, y].adjacentMines.ToString();
                button.ForeColor = GetNumberColor(grid[x, y].adjacentMines);
            }
            else // 隣接する地雷が0なら、周囲のセルも開く（ただし範囲内で）
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        // 再帰呼び出し時も、範囲情報を引き継ぐ
                        RevealCell(x + dx, y + dy, minX, maxX, minY, maxY);
                    }
                }
            }
        }

        // ゲームオーバー処理
        private void GameOver(bool won)
        {
            isGameOver = true;
            timer1.Stop();
            gameStopwatch.Stop();
            // すべての地雷の場所を表示
            for (int x = 0; x < gridSize_x; x++)
            {
                for (int y = 0; y < gridSize_y; y++)
                {
                    if (grid[x, y].isMine)
                    {
                        buttons[x, y].Text = "💣";
                        buttons[x, y].BackColor = grid[x, y].isRevealed ? Color.Red : Color.LightPink;
                    }
                }
            }

            if (won)
            {
                
                gameOverPanel.Visible = true;
                lblGameOverTitle.Visible = false;
                lblClearTitle.Visible = true;
                //MessageBox.Show("クリア！おめでとう！", "勝利");
            }
            else
            {
                gameOverPanel.Visible = true;
                lblGameOverTitle.Visible = true;
                lblClearTitle.Visible = false;
                //MessageBox.Show("ゲームオーバー", "敗北");
            }
        }

        // 勝利条件のチェック
        private void CheckForWin()
        {
            int revealedCount = 0;
            for (int x = 0; x < gridSize_x; x++)
            {
                for (int y = 0; y < gridSize_y; y++)
                {
                    if (grid[x, y].isRevealed)
                    {
                        revealedCount++;
                    }
                }
            }

            if (revealedCount == (gridSize_x * gridSize_y) - mineCount)
            {
                GameOver(true);
            }
        }

        // 数字の色を取得
        private Color GetNumberColor(int number)
        {
            switch (number)
            {
                case 1: return Color.Blue;
                case 2: return Color.Green;
                case 3: return Color.Red;
                case 4: return Color.DarkBlue;
                case 5: return Color.Maroon;
                case 6: return Color.Turquoise;
                case 7: return Color.Black;
                case 8: return Color.Gray;
                default: return Color.Black;
            }
        }

        private void button_Return_Click(object sender, EventArgs e)
        {
           
            Func.CreateMiniGame(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Stopwatchから経過時間を取得
            TimeSpan ts = gameStopwatch.Elapsed;

            // 2. ラベルのテキストを更新する
            label_Time.Text = $"Time: {ts:mm\\:ss\\.ff}";
        }

        private void button_explain_Click(object sender, EventArgs e)
        {
            settingsPanel.Visible = false;
            instructionPanel.Visible = true;
        }
                

        private void button_keep_Click(object sender, EventArgs e)
        {
            settingsPanel.Visible = false;
            instructionPanel.Visible = true;
            gridSize_y = (int)numericUpDown1.Value;
            gridSize_x = (int)numericUpDown2.Value;
            mineCount = (int)numericUpDown3.Value;

            if (comboBox1.SelectedItem == "たくさん")
            {
                wide_x = gridSize_x/2;
                wide_y = gridSize_y/2;
            }
            else if(comboBox1.SelectedItem == "ふつう")
            {
                wide_x = gridSize_x / 3;
                wide_y = gridSize_y / 3;
            }
            else if (comboBox1.SelectedItem == "すこし")
            {
                wide_x = 3;
                wide_y = 3;
            }

           


        }
        private void button_back_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 20;
            numericUpDown2.Value = 20;
            numericUpDown3.Value = 40;
            comboBox1.SelectedItem = "たくさん";
        }

        private async Task InstructionPanel()
        {
            // 1. パネル（土台）を作成する
            
            instructionPanel.Name = "instructionPanel";
            instructionPanel = new Panel { Size = new Size(660, 576), BackColor = Color.FromArgb(220, 240, 240, 240), BorderStyle = BorderStyle.FixedSingle, Visible = true, Font = new Font("Meiryo UI", 12F) };
            this.Controls.Add(instructionPanel);
            //instructionPanel.Size = new Size(880, 720); // パネルのサイズ
            //instructionPanel.Size = new System.Drawing.Size(880, 720);
            //instructionPanel.BorderStyle = BorderStyle.FixedSingle; // 枠線をつける
            //instructionPanel.Visible = true;

            // フォームの中央に配置する
            instructionPanel.Location = new Point(
                (Location_x - instructionPanel.Width) / 2,
                (Location_y - instructionPanel.Height) / 2);

            // 2. ラベル（説明文）を作成する
            Label Label1 = new Label { Text = "ゲームのせつめい", Font = new Font("Meiryo UI", 30), Size = new Size(600, 50), Location = new Point(instructionPanel.Width / 2-150, 20), };
            instructionPanel.Controls.Add(Label1);
            //playerPictureBox = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 100), SizeMode = PictureBoxSizeMode.Zoom };
            Label Label2 = new Label { Text = "1. マスをクリック", Font = new Font("Meiryo UI", 12F, FontStyle.Bold), Location = new Point(50, 80), Size = new Size(400, 30) };
            //instructionPanel.Controls.Add(playerPictureBox);
            instructionPanel.Controls.Add(Label2);
            //instructionPanel = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 160), SizeMode = PictureBoxSizeMode.Zoom };
            Label Label3 = new Label { Text = "さいしょはどこかすきなマスを選んでクリックしよう", Location = new Point(90, 110), Size = new Size(500, 30) };
            //instructionPanel.Controls.Add(obstaclePictureBox);
            instructionPanel.Controls.Add(Label3);
            Label Label4 = new Label { Text = "2. すうじはヒント ", Font = new Font("Meiryo UI", 12F, FontStyle.Bold), Location = new Point(50, 140), Size = new Size(400, 30) };            
            instructionPanel.Controls.Add(Label4);            
            Label Label5 = new Label { Text = "まわりのばくだんのかずをおしえてくれるよ", Location = new Point(90, 170), Size = new Size(500, 30) };            
            instructionPanel.Controls.Add(Label5);
            Label Label6 = new Label { Text = "3. ばくだんにはたを立てよう ", Font = new Font("Meiryo UI", 12F, FontStyle.Bold), Location = new Point(50, 200), Size = new Size(400, 30) };
            instructionPanel.Controls.Add(Label6);
            Label Label7 = new Label { Text = "ばくだんだとおもうマスをみぎクリックしてはたをたてよう", Location = new Point(90, 230), Size = new Size(500, 30) };
            instructionPanel.Controls.Add(Label7);
            Label Label8 = new Label { Text = "4. ばくだんいがいをぜんぶひらけばクリア！！ ", Font = new Font("Meiryo UI", 12F, FontStyle.Bold), Location = new Point(50, 260), Size = new Size(400, 30) };
            instructionPanel.Controls.Add(Label8);
            Label Label9 = new Label { Text = "まちがえてばくだんをクリックするとゲームオーバーだよ", Location = new Point(90, 290), Size = new Size(500, 30) };
            instructionPanel.Controls.Add(Label9);

            Button backButton = new Button { Text = "もどる", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(120, 60), Location = new Point(instructionPanel.Width-150, 20)};
            backButton.Visible = false;            
            backButton.Click += (s, e) =>
            {
                instructionPanel.Visible = false;

            };
            instructionPanel.Controls.Add(backButton);
            backButton.BringToFront();

            Button startButton = new Button { Text = "あたらしくはじめる", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(350, 460), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            
            startButton.Click += async(s, e) =>
            {
                //instructionPanel.SuspendLayout();
                instructionPanel.Visible = false;
                //instructionPanel.ResumeLayout();
                Application.DoEvents();
                //await Task.Yield();
                //LayoutControls();
                ResetGame();
                button_explain.Enabled = true;
                button_Reset.Enabled = true;
                backButton.Visible = true;

            };
            instructionPanel.Controls.Add(startButton);
            Button optionsButton = new Button { Text = "オプション", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(60, 460), BackColor = Color.White, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            optionsButton.Click += (s, e) =>
            {
                instructionPanel.Visible = false;
                settingsPanel.Visible = true;
            };
            instructionPanel.Controls.Add(optionsButton);
            

            // 5. 最後に、パネルをフォームの上に乗せる
            this.Controls.Add(instructionPanel);

            // パネルを一番手前に表示する
            instructionPanel.BringToFront();


        }

        private void CreateGameOver()
        {
            gameOverPanel = new Panel { Size = new Size(400, 300), BackColor = Color.FromArgb(220, 255, 255, 255), BorderStyle = BorderStyle.FixedSingle, Location = new Point((Location_x - 400) / 2,(Location_y - 300) / 2), Visible = false };
            lblGameOverTitle = new Label { Text = "Game Over", Font = new Font("Arial", 48, FontStyle.Bold), ForeColor = Color.SteelBlue, AutoSize = false, Size = new Size(400, 70), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(0, 30) };
            lblClearTitle = new Label { Text = "Clear!　おめでとう！", Font = new Font("Arial", 48, FontStyle.Bold), ForeColor = Color.SteelBlue, AutoSize = false, Size = new Size(400, 70), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(0, 30) };
            Button btnRestart = new Button { Text = "リスタート", Font = new Font("Meiryo UI", 14, FontStyle.Bold), Size = new Size(180, 60), Location = new Point(20, 210), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            btnRestart.Click += (s, e) => { instructionPanel.Visible = true; gameOverPanel.Visible = false; };
            Button btnBackToList = new Button { Text = "いちらんにもどる", Font = new Font("Meiryo UI", 14, FontStyle.Bold), Size = new Size(180, 60), Location = new Point(200, 210), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            btnBackToList.Click += (s, e) => Func.CreateMiniGame(this);
            gameOverPanel.Controls.Add(lblGameOverTitle);
            gameOverPanel.Controls.Add(lblClearTitle);
            gameOverPanel.Controls.Add(btnRestart);
            gameOverPanel.Controls.Add(btnBackToList);
            this.Controls.Add(gameOverPanel);
            gameOverPanel.BringToFront();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown3.Maximum = (int)numericUpDown1.Value* (int)numericUpDown2.Value;
            label3.Text = "ばくだんのかず （0-"+ numericUpDown1.Value * numericUpDown2.Value + "）";
            label7.Text = "（おすすめ: "+ (int)numericUpDown1.Value* (int)numericUpDown2.Value*0.1 +"）";
            //numericUpDown3.Value= numericUpDown1.Value* numericUpDown2.Value*0.1M;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown3.Maximum = (int)numericUpDown1.Value * (int)numericUpDown2.Value;
            label3.Text = "ばくだんのかず （0-" + numericUpDown1.Value * numericUpDown2.Value + "）";
            label7.Text = "（おすすめ: " + (int)numericUpDown1.Value * (int)numericUpDown2.Value * 0.1 + "）";
            //numericUpDown3.Value = numericUpDown1.Value * numericUpDown2.Value * 0.1M;
        }
    }
}
