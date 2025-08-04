using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class MiniGame_Nono_Stage : Form
    {
        #region メンバ変数
        public int StageId { get; set; }    // ステージレベル
        private int[,] solutionData;        // 正答
        private int[,] playerData;          // プレイヤーの解答
        private int cellSize;               // マス目サイズ
        private List<List<int>> topHints;
        private List<List<int>> sideHints;
        private List<List<bool>> dimmedTopHints;
        private List<List<bool>> dimmedSideHints;
        private bool isDragging = false;
        private Point lastDraggedCell = new Point(-1, -1);
        private int dragPaintMode = 0; // 1:黒く塗る, 2:×を付ける
        private Stack<int[,]> history = new Stack<int[,]>();
        #endregion

        public MiniGame_Nono_Stage()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        private void MiniGame_NonoStage_Load(object sender, EventArgs e)
        {
            LoadSolution(this.StageId);
            if (this.IsDisposed) return; // 読み込み失敗時はフォームが閉じられる

            pictureBox_Grid.Paint += pictureBox_Grid_Paint;
            pictureBox_TopHints.Paint += pictureBox_TopHints_Paint;
            pictureBox_SideHints.Paint += pictureBox_SideHints_Paint;

            pictureBox_Grid.MouseDown += pictureBox_Grid_MouseDown;
            pictureBox_Grid.MouseMove += pictureBox_Grid_MouseMove;
            pictureBox_Grid.MouseUp += pictureBox_Grid_MouseUp;

            CalculateAllHints();
            AutoFillZeroHintLines();
            pictureBox_Grid.Refresh();
        }

        private void pictureBox_Grid_MouseDown(object sender, MouseEventArgs e)
        {
            if (playerData == null) return;
            // 変更前の盤面状態を、履歴にコピーして保存する
            history.Push((int[,])playerData.Clone());

            isDragging = true;
            lastDraggedCell = new Point(-1, -1); // ドラッグ開始時にリセット

            // 塗るモードを決定（左クリック: 黒, 右クリック: ×）
            if (e.Button == MouseButtons.Left)
            {
                dragPaintMode = 1;
            }
            else if (e.Button == MouseButtons.Right)
            {
                dragPaintMode = 2;
            }
            // 最初の1マスを塗る
            ApplyPaint(e.Location);
        }

        private void pictureBox_Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isDragging || playerData == null) return;
            // ドラッグ中に連続して塗る
            ApplyPaint(e.Location);
        }

        private void pictureBox_Grid_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
            // ドラッグ終了時にクリア判定
            CheckForWin();
        }

        #region　描画
        private void pictureBox_Grid_Paint(object sender, PaintEventArgs e)
        {
            if (playerData == null) return;
            Graphics g = e.Graphics;
            g.Clear(Color.White);
            int width = playerData.GetLength(0);
            int height = playerData.GetLength(1);

            // マスの状態に応じて塗りつぶし
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (playerData[x, y] == 1) // 黒マス
                    {
                        g.FillRectangle(Brushes.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                    }
                    else if (playerData[x, y] == 2) // ×印
                    {
                        g.DrawLine(Pens.Gray, x * cellSize, y * cellSize, (x + 1) * cellSize, (y + 1) * cellSize);
                        g.DrawLine(Pens.Gray, (x + 1) * cellSize, y * cellSize, x * cellSize, (y + 1) * cellSize);
                    }
                }
            }
            // 格子線を描画 (太い線と細い線)
            for (int i = 0; i <= width; i++)
                g.DrawLine(i % 5 == 0 ? Pens.Black : Pens.LightGray, i * cellSize, 0, i * cellSize, height * cellSize);
            for (int i = 0; i <= height; i++)
                g.DrawLine(i % 5 == 0 ? Pens.Black : Pens.LightGray, 0, i * cellSize, width * cellSize, i * cellSize);
        }

        private void pictureBox_TopHints_Paint(object sender, PaintEventArgs e)
        {
            // 上側ヒントの描画
            DrawHints(e.Graphics, topHints, dimmedTopHints, true);
        }

        private void pictureBox_SideHints_Paint(object sender, PaintEventArgs e)
        {
            // 左側ヒントの描画
            DrawHints(e.Graphics, sideHints, dimmedSideHints, false);
        }
        #endregion

        // 正答CSV読み込み
        private void LoadSolution(int stageId)
        {
            string filePath = Path.Combine(Application.StartupPath, "Nono", $"stage{stageId}.csv");
            if (!File.Exists(filePath))
            {
                MessageBox.Show("問題ファイルが見つかりません。");
                this.Close();
                return;
            }
            var lines = File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
            if (lines.Count == 0) { this.Close(); return; }

            int height = lines.Count;
            int width = lines[0].Split(',').Length;
            solutionData = new int[width, height];
            playerData = new int[width, height];

            for (int y = 0; y < height; y++)
            {
                string[] values = lines[y].Split(',');
                for (int x = 0; x < width; x++)
                {
                    solutionData[x, y] = int.Parse(values[x]);
                    playerData[x, y] = 0;
                }
            }

            int maxCellSizeW = pictureBox_Grid.ClientSize.Width / width;
            int maxCellSizeH = pictureBox_Grid.ClientSize.Height / height;
            this.cellSize = Math.Min(maxCellSizeW, maxCellSizeH);
        }

        private void CalculateAllHints()
        {
            int width = solutionData.GetLength(0);
            int height = solutionData.GetLength(1);
            topHints = new List<List<int>>();
            sideHints = new List<List<int>>();
            dimmedTopHints = new List<List<bool>>();
            dimmedSideHints = new List<List<bool>>();

            // 上側ヒント
            for (int x = 0; x < width; x++)
            {
                var hints = CalculateHintForLine(Enumerable.Range(0, height).Select(y => solutionData[x, y]));
                topHints.Add(hints);
                dimmedTopHints.Add(new List<bool>(new bool[hints.Count]));
            }
            // 左側ヒント
            for (int y = 0; y < height; y++)
            {
                var hints = CalculateHintForLine(Enumerable.Range(0, width).Select(x => solutionData[x, y]));
                sideHints.Add(hints);
                dimmedSideHints.Add(new List<bool>(new bool[hints.Count]));
            }
        }


        private void AutoFillZeroHintLines()
        {
            // 上側ヒント（列）のチェック
            for (int x = 0; x < topHints.Count; x++)
            {
                if (topHints[x].Count == 1 && topHints[x][0] == 0)
                {
                    for (int y = 0; y < playerData.GetLength(1); y++)
                    {
                        playerData[x, y] = 2; // ×で埋める
                    }
                }
            }
            // 左側ヒント（行）のチェック
            for (int y = 0; y < sideHints.Count; y++)
            {
                if (sideHints[y].Count == 1 && sideHints[y][0] == 0)
                {
                    for (int x = 0; x < playerData.GetLength(0); x++)
                    {
                        playerData[x, y] = 2; // ×で埋める
                    }
                }
            }
        }

        private List<int> CalculateHintForLine(IEnumerable<int> line)
        {
            var hints = new List<int>();
            int count = 0;
            foreach (var cell in line)
            {
                if (cell == 1) count++;
                else { if (count > 0) hints.Add(count); count = 0; }
            }
            if (count > 0) hints.Add(count);
            if (hints.Count == 0) hints.Add(0);
            return hints;
        }

        // マウス位置からマス目を更新し、関連処理を呼び出す
        private void ApplyPaint(Point mouseLocation)
        {
            int cellX = mouseLocation.X / cellSize;
            int cellY = mouseLocation.Y / cellSize;
            if (cellX < 0 || cellX >= playerData.GetLength(0) || cellY < 0 || cellY >= playerData.GetLength(1)) return;
            if (lastDraggedCell.X == cellX && lastDraggedCell.Y == cellY) return;

            int currentVal = playerData[cellX, cellY];


            if (dragPaintMode == 1) // 黒塗りモードの場合
            {
                // マスが×でなければ、白と黒を切り替える
                if (currentVal != 2)
                {
                    playerData[cellX, cellY] = (currentVal == 1) ? 0 : 1;
                }
            }
            else if (dragPaintMode == 2) // ×印モードの場合
            {
                // マスが黒でなければ、白と×を切り替える
                if (currentVal != 1)
                {
                    playerData[cellX, cellY] = (currentVal == 2) ? 0 : 2;
                }
            }

            lastDraggedCell = new Point(cellX, cellY);
            pictureBox_Grid.Refresh();
            UpdateHintsState(cellX, cellY);
        }

        // ヒントの状態（オート×埋め、数消し）を更新する
        private void UpdateHintsState(int x, int y)
        {
            UpdateSingleHintLine(x, true); // 列を更新
            UpdateSingleHintLine(y, false); // 行を更新
        }

        private void UpdateSingleHintLine(int index, bool isColumn)
        {
            // --- 準備 ---
            int width = solutionData.GetLength(0);
            int height = solutionData.GetLength(1);
            var lineData = isColumn ? Enumerable.Range(0, height).Select(y => playerData[index, y]).ToList()
                                      : Enumerable.Range(0, width).Select(x => playerData[x, index]).ToList();
            var hints = isColumn ? topHints[index] : sideHints[index];
            var dimmedHints = isColumn ? dimmedTopHints[index] : dimmedSideHints[index];

            // --- 判定開始 ---

            // 1. まず、すべてのヒントを「濃い文字」状態にリセット
            for (int i = 0; i < dimmedHints.Count; i++) dimmedHints[i] = false;

            var playerHints = CalculateHintForLine(lineData.Select(c => c == 1 ? 1 : 0));
            bool allFilled = !lineData.Any(c => c == 0);

            if (allFilled)
            {
                // 2. 【ルールB】行がすべて埋まっている場合
                if (playerHints.SequenceEqual(hints))
                {
                    for (int i = 0; i < dimmedHints.Count; i++) dimmedHints[i] = true;
                }
            }
            else
            {
                // 3. 【ルールA】行がまだ埋まっていない場合

                // 3a. 左（上）からのチェック
                int forwardHintIndex = 0;
                int currentCellIndex = 0;
                // 端から続く×をスキップ
                while (currentCellIndex < lineData.Count && lineData[currentCellIndex] == 2) currentCellIndex++;

                // 端（または×の隣）から黒マスブロックが始まっている限りチェックを続ける
                while (currentCellIndex < lineData.Count && lineData[currentCellIndex] == 1 && forwardHintIndex < hints.Count)
                {
                    int blockLength = 0;
                    int tempCellIndex = currentCellIndex;
                    while (tempCellIndex < lineData.Count && lineData[tempCellIndex] == 1)
                    {
                        blockLength++;
                        tempCellIndex++;
                    }

                    if (blockLength == hints[forwardHintIndex])
                    {
                        dimmedHints[forwardHintIndex] = true;
                        forwardHintIndex++;
                        currentCellIndex = tempCellIndex;
                        // 次のブロックに移る前に、×をスキップ
                        while (currentCellIndex < lineData.Count && lineData[currentCellIndex] == 2) currentCellIndex++;
                    }
                    else break; // パターンが一致しなかったら終了
                }

                // 3b. 右（下）からのチェック
                int backwardHintIndex = hints.Count - 1;
                currentCellIndex = lineData.Count - 1;
                // 端から続く×をスキップ
                while (currentCellIndex >= 0 && lineData[currentCellIndex] == 2) currentCellIndex--;

                while (currentCellIndex >= 0 && lineData[currentCellIndex] == 1 && backwardHintIndex >= 0)
                {
                    int blockLength = 0;
                    int tempCellIndex = currentCellIndex;
                    while (tempCellIndex >= 0 && lineData[tempCellIndex] == 1)
                    {
                        blockLength++;
                        tempCellIndex--;
                    }

                    if (blockLength == hints[backwardHintIndex])
                    {
                        dimmedHints[backwardHintIndex] = true;
                        backwardHintIndex--;
                        currentCellIndex = tempCellIndex;
                        // 次のブロックに移る前に、×をスキップ
                        while (currentCellIndex >= 0 && lineData[currentCellIndex] == 2) currentCellIndex--;
                    }
                    else break; // パターンが一致しなかったら終了
                }
            }

            // 4. オート×埋め処理 (判定をplayerHintsに統一)
            if (playerHints.SequenceEqual(hints))
            {
                for (int i = 0; i < lineData.Count; i++)
                {
                    if (isColumn && playerData[index, i] == 0) playerData[index, i] = 2;
                    if (!isColumn && playerData[i, index] == 0) playerData[i, index] = 2;
                }
                pictureBox_Grid.Refresh();
            }

            // --- 描画の更新 ---
            if (isColumn) pictureBox_TopHints.Refresh();
            else pictureBox_SideHints.Refresh();
        }

        // ヒントを描画する共通メソッド
        private void DrawHints(Graphics g, List<List<int>> hintLines, List<List<bool>> dimmedLines, bool isTop)
        {
            g.Clear(this.BackColor);
            using (Font font = new Font("Arial", cellSize * 0.5f, FontStyle.Bold))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                for (int i = 0; i < hintLines.Count; i++)
                {
                    var hints = hintLines[i];
                    var dimmed = dimmedLines[i];
                    for (int j = 0; j < hints.Count; j++)
                    {
                        Brush brush = dimmed[j] ? Brushes.LightGray : Brushes.Black;

                        if (isTop) // 上側ヒントの場合 (下詰め)
                        {
                            // 描画Y座標を、下端から逆算する
                            float yPos = pictureBox_TopHints.Height - ((hints.Count - j) * cellSize);
                            g.DrawString(hints[j].ToString(), font, brush, new RectangleF(i * cellSize, yPos, cellSize, cellSize), sf);
                        }
                        else // 左側ヒントの場合 (右詰め)
                        {
                            // 描画X座標を、右端から逆算する
                            float xPos = pictureBox_SideHints.Width - ((hints.Count - j) * cellSize);
                            g.DrawString(hints[j].ToString(), font, brush, new RectangleF(xPos, i * cellSize, cellSize, cellSize), sf);
                        }
                    }
                }
            }
        }

        private void CheckForWin()
        {
            int width = solutionData.GetLength(0);
            int height = solutionData.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // プレイヤーが塗ったマス(1)と、正解(1)が一致しているか
                    // (プレイヤー盤面の×(2)は白(0)とみなす)
                    int playerMark = (playerData[x, y] == 1) ? 1 : 0;
                    if (playerMark != solutionData[x, y])
                    {
                        return; // 1マスでも違えば終了
                    }
                }
            }

            NonogramProgress.IsCleared[this.StageId] = true;

            MessageBox.Show("クリア！");
            pictureBox_Grid.Enabled = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        #region　各ボタン
        private void button_Undo_Click(object sender, EventArgs e)
        {
            // 履歴が1件以上ある場合のみ処理
            if (history.Count > 0)
            {
                // 履歴から一番新しい盤面を取り出して、現在の盤面に上書きする
                playerData = history.Pop();

                // ヒントの状態や盤面表示をすべて更新する
                for (int x = 0; x < playerData.GetLength(0); x++)
                {
                    UpdateSingleHintLine(x, true); 
                }
                // すべての行のヒント状態を更新
                for (int y = 0; y < playerData.GetLength(1); y++)
                {
                    UpdateSingleHintLine(y, false);
                }
                pictureBox_Grid.Refresh();
            }
        }

        private void button_Reset_Click(object sender, EventArgs e)
        {
            if (playerData == null) return;

            // 履歴をすべてクリア
            history.Clear();

            // プレイヤーの盤面をすべて0（白マス）に戻す
            for (int y = 0; y < playerData.GetLength(1); y++)
            {
                for (int x = 0; x < playerData.GetLength(0); x++)
                {
                    playerData[x, y] = 0;
                }
            }

            // ヒントの数消し状態などもすべてリセットして再描画
            CalculateAllHints();
            pictureBox_TopHints.Refresh();
            pictureBox_SideHints.Refresh();
            pictureBox_Grid.Refresh();
        }
        #endregion

        private void button_Back_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
