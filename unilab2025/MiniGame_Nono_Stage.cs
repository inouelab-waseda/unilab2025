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

            CalculateAllHints();

            // 各PictureBoxのPaintイベントハンドラを登録
            pictureBox_Grid.Paint += pictureBox_Grid_Paint;
            pictureBox_TopHints.Paint += pictureBox_TopHints_Paint;
            pictureBox_SideHints.Paint += pictureBox_SideHints_Paint;
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

            // マスの状態を更新
            int currentVal = playerData[cellX, cellY];
            if (dragPaintMode == 1) playerData[cellX, cellY] = currentVal == 1 ? 0 : 1; // 左クリック: 白/黒トグル
            if (dragPaintMode == 2) playerData[cellX, cellY] = currentVal == 2 ? 0 : 2; // 右クリック: 白/×トグル

            lastDraggedCell = new Point(cellX, cellY);
            pictureBox_Grid.Refresh();

            // 変更があった行と列のヒント状態を更新
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
            int width = solutionData.GetLength(0);
            int height = solutionData.GetLength(1);
            var lineData = isColumn ? Enumerable.Range(0, height).Select(y => playerData[index, y])
                                    : Enumerable.Range(0, width).Select(x => playerData[x, index]);
            var hints = isColumn ? topHints[index] : sideHints[index];
            var dimmedHints = isColumn ? dimmedTopHints[index] : dimmedSideHints[index];

            // オート×埋め
            int blackCount = lineData.Count(c => c == 1);
            if (blackCount == hints.Sum())
            {
                for (int i = 0; i < (isColumn ? height : width); i++)
                {
                    if (isColumn && playerData[index, i] == 0) playerData[index, i] = 2; // ×で埋める
                    if (!isColumn && playerData[i, index] == 0) playerData[i, index] = 2;
                }
                pictureBox_Grid.Refresh();
                lineData = isColumn ? Enumerable.Range(0, height).Select(y => playerData[index, y]) : Enumerable.Range(0, width).Select(x => playerData[x, index]);
            }

            // 数消し判定
            var playerHints = CalculateHintForLine(lineData.Select(c => c == 1 ? 1 : 0));
            bool allFilled = !lineData.Any(c => c == 0);

            for (int i = 0; i < dimmedHints.Count; i++) dimmedHints[i] = false; // いったんリセット

            if (allFilled && !playerHints.SequenceEqual(hints))
            {
                // 矛盾している場合は数消ししない
            }
            else if (playerHints.SequenceEqual(hints))
            {
                // 正解と完全に一致すればすべて消す
                for (int i = 0; i < dimmedHints.Count; i++) dimmedHints[i] = true;
            }

            // ヒント表示を更新
            if (isColumn) pictureBox_TopHints.Refresh();
            else pictureBox_SideHints.Refresh();
        }

        // ヒントを描画する共通メソッド
        private void DrawHints(Graphics g, List<List<int>> hintLines, List<List<bool>> dimmedLines, bool isTop)
        {
            g.Clear(this.BackColor);
            using (Font font = new Font("Arial", cellSize / 2))
            using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                for (int i = 0; i < hintLines.Count; i++)
                {
                    var hints = hintLines[i];
                    var dimmed = dimmedLines[i];
                    for (int j = 0; j < hints.Count; j++)
                    {
                        Brush brush = dimmed[j] ? Brushes.LightGray : Brushes.Black;
                        if (isTop)
                        {
                            g.DrawString(hints[j].ToString(), font, brush, new RectangleF(i * cellSize, j * cellSize, cellSize, cellSize), sf);
                        }
                        else
                        {
                            g.DrawString(hints[j].ToString(), font, brush, new RectangleF(j * cellSize, i * cellSize, cellSize, cellSize), sf);
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
