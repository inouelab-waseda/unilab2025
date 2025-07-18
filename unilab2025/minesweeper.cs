using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class minesweeper : Form
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
        private readonly int gridSize = 10; // グリッドのサイズ (10x10)
        private readonly int mineCount = 10; // 地雷の数


        private Cell[,] grid;       // セルのロジックを管理する2次元配列
        private Button[,] buttons;  // 画面に表示するボタンの2次元配列
        private bool isFirstClick;  // 最初のクリックかどうかを判定するフラグ
        private bool isGameOver;    // ゲームオーバー状態を管理するフラグ



        public minesweeper()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;

        }

        private void minesweeper_Load(object sender, EventArgs e)
        {
            InitializeGame();
        }

        // ゲームの初期化
        private void InitializeGame()
        {
            this.Text = "Minesweeper";
            this.ClientSize = new Size(gridSize * 30, gridSize * 30 + 30); // ウィンドウサイズ調整

            // UIコントロールを初期化
            var menuStrip = new MenuStrip();
            var gameMenu = new ToolStripMenuItem("Game");
            var newGameMenuItem = new ToolStripMenuItem("New Game", null, (s, e) => ResetGame());
            gameMenu.DropDownItems.Add(newGameMenuItem);
            menuStrip.Items.Add(gameMenu);
            this.Controls.Add(menuStrip);

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
                    this.Controls.Add(buttons[x, y]);
                }
            }
            this.ClientSize = new Size(gridSize * 30 + offsetX, gridSize * 30 + 30 + offsetY);

        }

        // ゲームのリセット
        private void ResetGame()
        {
            // 古いボタンを削除
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    this.Controls.Remove(buttons[x, y]);
                }
            }
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
                }

                if (grid[x, y].isMine)
                {
                    GameOver(false); // 地雷を踏んだので負け
                }
                else
                {
                    RevealCell(x, y);
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
        private void RevealCell(int x, int y)
        {
            // グリッドの範囲外、または既に開いているセルは処理しない
            if (x < 0 || x >= gridSize || y < 0 || y >= gridSize || grid[x, y].isRevealed)
            {
                return;
            }
            // 旗が立っているセルも処理しない
            if (grid[x, y].isFlagged) return;


            grid[x, y].isRevealed = true;
            Button button = buttons[x, y];
            button.Enabled = false; // ボタンを無効化
            button.BackColor = Color.LightGray;

            if (grid[x, y].adjacentMines > 0)
            {
                button.Text = grid[x, y].adjacentMines.ToString();
                button.ForeColor = GetNumberColor(grid[x, y].adjacentMines);
            }
            else // 隣接する地雷が0なら、周囲のセルも開く
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue;
                        RevealCell(x + dx, y + dy);
                    }
                }
            }
        }

        // ゲームオーバー処理
        private void GameOver(bool won)
        {
            isGameOver = true;
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





    }
}
