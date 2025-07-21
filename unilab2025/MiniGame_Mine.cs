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
        private readonly int gridSize = 20; // グリッドのサイズ (10x10)
        private readonly int mineCount = 40; // 地雷の数

        public int Location_x;
        public int Location_y;

        private Cell[,] grid;       // セルのロジックを管理する2次元配列
        private Button[,] buttons;  // 画面に表示するボタンの2次元配列
        private bool isFirstClick;  // 最初のクリックかどうかを判定するフラグ
        private bool isGameOver;    // ゲームオーバー状態を管理するフラグ

        private Stopwatch gameStopwatch;//時間

        PictureBox pictureBox_Conv;
        byte[] Capt;
        List<Conversation> Message;
        bool isMessageMode;
        public List<Conversation> currentConversation;

        Panel instructionPanel = new Panel();


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
            pictureBox1.Size = new Size(gridSize * 30 + 100, gridSize * 30 + 30 + 200);
            int Location_X = (this.ClientSize.Width - pictureBox1.Width) / 2;
            int Location_Y = (this.ClientSize.Height - pictureBox1.Height) / 2;
            pictureBox1.Location = new Point(Location_X, Location_Y);

            settingsPanel.Location = new Point(
                (Location_x - settingsPanel.Width) / 2,
                (Location_y - settingsPanel.Height) / 2);

            //currentConversation = Dictionaries.Conversations["mine"];
            //if (currentConversation != null && currentConversation.Count > 0)
            //{
            //    Capt = await Func.PlayConv(this, pictureBox_Conv, currentConversation);
            //}
            InstructionPanel();
            //InitializeGame();
        }

        // ゲームの初期化
        private void InitializeGame()
        {
                    

            this.Text = "Minesweeper";
                                  

            // グリッドとボタンの配列を作成
            grid = new Cell[gridSize, gridSize];
            buttons = new Button[gridSize, gridSize];
            isFirstClick = true;
            isGameOver = false;


            int offsetX = 50; // フォームの左端から50ピクセル右にずらす
            int offsetY = 100; // メニューバーの下から100ピクセル下にずらす


            // ボタンを動的に生成してフォームに配置
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
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
            this.ClientSize = new Size(gridSize * 30 + offsetX, gridSize * 30 + 30 + offsetY);

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


            // 古いボタンを削除
            pictureBox1.Controls.Clear();
            InitializeGame();
        }

        // 地雷の配置（最初のクリック後に行う）
        private void PlaceMines(int firstClickX, int firstClickY)
        {
            var random = new Random();
            int minesPlaced = 0;
            while (minesPlaced < mineCount)
            {
                int x = random.Next(gridSize);
                int y = random.Next(gridSize);

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
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
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
                            if (nx >= 0 && nx < gridSize && ny >= 0 && ny < gridSize && grid[nx, ny].isMine)
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
                        int areaSize = 10; // 中心から±5マスで10x10の範囲
                        int minX = Math.Max(0, x - areaSize);
                        int maxX = Math.Min(gridSize - 1, x + areaSize);
                        int minY = Math.Max(0, y - areaSize);
                        int maxY = Math.Min(gridSize - 1, y + areaSize);

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
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
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
                MessageBox.Show("クリア！おめでとう！", "勝利");
            }
            else
            {
                MessageBox.Show("ゲームオーバー", "敗北");
            }
        }

        // 勝利条件のチェック
        private void CheckForWin()
        {
            int revealedCount = 0;
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (grid[x, y].isRevealed)
                    {
                        revealedCount++;
                    }
                }
            }

            if (revealedCount == (gridSize * gridSize) - mineCount)
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

        private void button_option_Click(object sender, EventArgs e)
        {
            instructionPanel.Visible = false;
            settingsPanel.Visible = true;
        }

        private void button_keep_Click(object sender, EventArgs e)
        {
            settingsPanel.Visible = false;
            instructionPanel.Visible = true;
        }
        private void button_back_Click(object sender, EventArgs e)
        {

        }

        private void InstructionPanel()
        {
            // 1. パネル（土台）を作成する
            
            instructionPanel.Name = "instructionPanel";
            instructionPanel = new Panel { Size = new Size(660, 576), BackColor = Color.FromArgb(220, 240, 240, 240), BorderStyle = BorderStyle.FixedSingle, Visible = true, Font = new Font("MS UI Gothic", 15) };
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
            Label Label1 = new Label { Text = "ゲームのせつめい", Font = new Font("MS UI Gothic", 30), Size = new Size(600, 50), Location = new Point(instructionPanel.Width / 2-150, 20) };
            instructionPanel.Controls.Add(Label1);
            //playerPictureBox = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 100), SizeMode = PictureBoxSizeMode.Zoom };
            Label playerLabel = new Label { Text = "◀ このペンギンをそうさします", Location = new Point(130, 110), Size = new Size(400, 30) };
            //instructionPanel.Controls.Add(playerPictureBox);
            instructionPanel.Controls.Add(playerLabel);
            //instructionPanel = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 160), SizeMode = PictureBoxSizeMode.Zoom };
            Label obstacleLabel = new Label { Text = "◀ このかべはよけてください", Location = new Point(130, 170), Size = new Size(400, 30) };
            //instructionPanel.Controls.Add(obstaclePictureBox);
            instructionPanel.Controls.Add(obstacleLabel);


            Button startButton = new Button { Text = "ゲームスタート！", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(320, 460), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            startButton.Click += (s, e) =>
            {
                instructionPanel.Visible = false;
                InitializeGame();

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
        private void ShowOptionsPanel()
        {
            settingsPanel.Visible = true;
        }

        
    }
}
