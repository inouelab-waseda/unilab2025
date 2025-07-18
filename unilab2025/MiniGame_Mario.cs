using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace unilab2025
{
    public partial class MiniGame_Mario : Form
    {
        #region 変数

        Dictionary<string, Image> playerImages = new Dictionary<string, Image>();
        Image currentPlayerImage;
        string lastDirection;

        Image obstacleImage;
        Image backgroundImage;
        Image itemImage;

        Rectangle playerRect;
        List<Rectangle> obstacles = new List<Rectangle>();
        List<Rectangle> items = new List<Rectangle>();

        // ★★★ エフェクト関連の変数を追加 ★★★
        private class FloatingTextEffect
        {
            public string Text;
            public PointF Position;
            public int Lifetime;
            public void Update()
            {
                Position = new PointF(Position.X, Position.Y - 1.5f); // 少しずつ上に移動
                Lifetime--;
            }
        }
        private List<FloatingTextEffect> effects = new List<FloatingTextEffect>();
        private Font effectFont = new Font("Arial", 16, FontStyle.Bold);
        private Brush effectBrush = Brushes.Gold;


        bool goUp, goDown, goLeft, goRight, isGameOver;
        int score;
        int elapsedTime;
        Random rnd = new Random();

        int playerSpeed = 3;
        int obstacleBaseSpeed = 1;
        int obstacleSpeed;

        private int lastSpawnX;
        private int backgroundX = 0;

        const int OBSTACLE_COLUMN_WIDTH = 30;
        const int OBSTACLE_LANE_HEIGHT = 30;

        Panel gameOverPanel;
        Label lblFinalScore;

        Panel explanationPanel;
        PictureBox playerPictureBox;
        PictureBox obstaclePictureBox;
        PictureBox itemPictureBox;
        private bool isFirstGame = true;

        #endregion

        public MiniGame_Mario()
        {
            InitializeComponent();

            #region イベントハンドラ
            this.gameTimer.Interval = 16;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.Resize += new EventHandler(MiniGame_Mario_Resize);
            this.KeyDown += new KeyEventHandler(MiniGame_Mario_KeyDown);
            this.KeyUp += new KeyEventHandler(MiniGame_Mario_KeyUp);
            this.gameTimer.Tick += new EventHandler(gameTimer_Tick);
            this.pbCanvas.Paint += new PaintEventHandler(pbCanvas_Paint);
            this.Button_Up.MouseDown += new MouseEventHandler(Button_Up_MouseDown);
            this.Button_Up.MouseUp += new MouseEventHandler(Button_Up_MouseUp);
            this.Button_Down.MouseDown += new MouseEventHandler(Button_Down_MouseDown);
            this.Button_Down.MouseUp += new MouseEventHandler(Button_Down_MouseUp);
            this.Button_Left.MouseDown += new MouseEventHandler(Button_Left_MouseDown);
            this.Button_Left.MouseUp += new MouseEventHandler(Button_Left_MouseUp);
            this.Button_Right.MouseDown += new MouseEventHandler(Button_Right_MouseDown);
            this.Button_Right.MouseUp += new MouseEventHandler(Button_Right_MouseUp);
            this.KeyPreview = true;
            this.DoubleBuffered = true;
            #endregion

            CreateGameOverUI();
            CreateExplanationUI();
            InitializeGame();
        }

        #region 初期化処理
        private void InitializeGame()
        {
            gameTimer.Stop();

            isGameOver = false;
            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            score = 0;
            elapsedTime = 0;
            backgroundX = 0;
            obstacleSpeed = obstacleBaseSpeed;
            lblScore.Text = "Score: 0";
            lblScore.Parent = pbCanvas;
            lblScore.BackColor = Color.Transparent;

            try
            {
                playerImages["Up"] = Image.FromFile(@"Image\DotPic\Penguin\Penguin_後ろ.png");
                Image downImage = (Image)playerImages["Up"].Clone();
                downImage.RotateFlip(RotateFlipType.RotateNoneFlipY);
                playerImages["Down"] = downImage;
                playerImages["Left"] = Image.FromFile(@"Image\DotPic\Penguin\Penguin_滑る左.png");
                playerImages["Right"] = Image.FromFile(@"Image\DotPic\Penguin\Penguin_滑る右.png");
                lastDirection = "Right";
                currentPlayerImage = playerImages["Right"];
                obstacleImage = Image.FromFile(@"Object\Img_Object_102.png");
                backgroundImage = Image.FromFile(@"Object\Img_Object_107.png");
                itemImage = Image.FromFile(@"Object\Img_Object_109.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("ゲームに必要な画像ファイルが見つかりません。\n" + ex.Message);
                this.Close();
                return;
            }

            playerPictureBox.Image = playerImages["Right"];
            obstaclePictureBox.Image = obstacleImage;
            itemPictureBox.Image = itemImage;

            int playerSize = 40;
            playerRect = new Rectangle(150, this.ClientSize.Height / 2 - playerSize / 2, playerSize, playerSize);

            obstacles.Clear();
            items.Clear();
            effects.Clear(); // ★ エフェクトリストをクリア

            int safeZoneEndX = playerRect.X + 300;
            int worldWidth = this.ClientSize.Width + OBSTACLE_COLUMN_WIDTH;

            for (int x = 0; x < worldWidth; x += OBSTACLE_COLUMN_WIDTH)
            {
                bool inSafeZone = x > playerRect.X - OBSTACLE_COLUMN_WIDTH && x < safeZoneEndX;
                if (!inSafeZone)
                {
                    int numberOfLanes = pbCanvas.Height / OBSTACLE_LANE_HEIGHT + 1;
                    for (int i = 0; i < numberOfLanes; i++)
                    {
                        int y = i * OBSTACLE_LANE_HEIGHT;
                        if (rnd.Next(0, 18) == 0)
                        {
                            obstacles.Add(new Rectangle(x, y, OBSTACLE_COLUMN_WIDTH, OBSTACLE_LANE_HEIGHT));
                        }
                        else if (rnd.Next(0, 100) == 0)
                        {
                            items.Add(new Rectangle(x, y, OBSTACLE_COLUMN_WIDTH, OBSTACLE_LANE_HEIGHT));
                        }
                    }
                }
            }
            lastSpawnX = worldWidth;

            this.ActiveControl = pbCanvas;
            gameOverPanel.Visible = false;

            if (isFirstGame)
            {
                explanationPanel.Visible = true;
                explanationPanel.BringToFront();
                isFirstGame = false;
            }
            else
            {
                gameTimer.Start();
            }
        }
        #endregion

        #region ゲームループと描画処理
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver) return;

            elapsedTime++;
            score++;
            lblScore.Text = "Score: " + score;

            backgroundX -= obstacleSpeed;

            if (goUp && playerRect.Top > 0) playerRect.Y -= playerSpeed;
            if (goDown && playerRect.Bottom < pbCanvas.Height) playerRect.Y += playerSpeed;
            if (goLeft && playerRect.Left > 0) playerRect.X -= playerSpeed;
            if (goRight && playerRect.Right < pbCanvas.Width) playerRect.X += playerSpeed;

            if (goUp) { lastDirection = "Up"; }
            else if (goDown) { lastDirection = "Down"; }
            else if (goLeft) { lastDirection = "Left"; }
            else if (goRight) { lastDirection = "Right"; }
            if (playerImages.ContainsKey(lastDirection)) { currentPlayerImage = playerImages[lastDirection]; }

            for (int i = obstacles.Count - 1; i >= 0; i--)
            {
                Rectangle rect = obstacles[i];
                rect.X -= obstacleSpeed;
                obstacles[i] = rect;

                Rectangle playerHitBox = new Rectangle(playerRect.X + playerRect.Width / 4, playerRect.Y + playerRect.Height / 4, playerRect.Width / 2, playerRect.Height / 2);
                if (playerHitBox.IntersectsWith(rect))
                {
                    GameOver();
                    return;
                }
                if (rect.Right < 0) { obstacles.RemoveAt(i); }
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                Rectangle rect = items[i];
                rect.X -= obstacleSpeed;
                items[i] = rect;

                if (playerRect.IntersectsWith(rect))
                {
                    score += 500;

                    // ★★★ エフェクトを生成 ★★★
                    effects.Add(new FloatingTextEffect
                    {
                        Text = "+500",
                        Position = new PointF(rect.X, rect.Y),
                        Lifetime = 60 // 約1秒間表示
                    });

                    items.RemoveAt(i);
                    continue;
                }
                if (rect.Right < 0) { items.RemoveAt(i); }
            }

            // ★★★ エフェクトの更新と削除 ★★★
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                effects[i].Update();
                if (effects[i].Lifetime <= 0)
                {
                    effects.RemoveAt(i);
                }
            }

            lastSpawnX -= obstacleSpeed;
            while (lastSpawnX < this.ClientSize.Width)
            {
                int x = lastSpawnX;
                int numberOfLanes = pbCanvas.Height / OBSTACLE_LANE_HEIGHT + 1;
                for (int i = 0; i < numberOfLanes; i++)
                {
                    int y = i * OBSTACLE_LANE_HEIGHT;
                    if (rnd.Next(0, 18) == 0)
                    {
                        obstacles.Add(new Rectangle(x, y, OBSTACLE_COLUMN_WIDTH, OBSTACLE_LANE_HEIGHT));
                    }
                    else if (rnd.Next(0, 100) == 0)
                    {
                        items.Add(new Rectangle(x, y, OBSTACLE_COLUMN_WIDTH, OBSTACLE_LANE_HEIGHT));
                    }
                }
                lastSpawnX += OBSTACLE_COLUMN_WIDTH;
            }

            int maxSpeed = playerSpeed + 2;
            int calculatedSpeed = obstacleBaseSpeed + (elapsedTime / 1800);
            obstacleSpeed = Math.Min(calculatedSpeed, maxSpeed);

            pbCanvas.Invalidate();
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // レイヤー1: 背景
            if (backgroundImage != null)
            {
                int tileWidth = OBSTACLE_COLUMN_WIDTH;
                int tileHeight = OBSTACLE_LANE_HEIGHT;
                int startX = backgroundX % tileWidth;
                if (startX > 0) startX -= tileWidth;
                for (int y = 0; y < this.ClientSize.Height; y += tileHeight)
                {
                    for (int x = startX; x < this.ClientSize.Width; x += tileWidth)
                    {
                        e.Graphics.DrawImage(backgroundImage, x, y, tileWidth, tileHeight);
                    }
                }
            }

            // レイヤー2: 障害物
            if (obstacleImage != null) { foreach (Rectangle rect in obstacles) { e.Graphics.DrawImage(obstacleImage, rect); } }

            // レイヤー3: アイテム
            if (itemImage != null) { foreach (Rectangle rect in items) { e.Graphics.DrawImage(itemImage, rect); } }

            // レイヤー4: プレイヤー
            if (currentPlayerImage != null)
                e.Graphics.DrawImage(currentPlayerImage, playerRect);
            else
                e.Graphics.FillRectangle(Brushes.Red, playerRect);

            // ★★★ レイヤー5: エフェクト (一番手前に描画) ★★★
            foreach (var effect in effects)
            {
                e.Graphics.DrawString(effect.Text, effectFont, effectBrush, effect.Position);
            }
        }
        #endregion

        #region UIとゲーム状態
        private void MiniGame_Mario_Resize(object sender, EventArgs e)
        {
            lblScore.Location = new Point(20, 20);

            int buttonSize = 70;
            int margin = 25;
            int gap = 5;

            Button_Up.Size = new Size(buttonSize, buttonSize);
            Button_Down.Size = new Size(buttonSize, buttonSize);
            Button_Left.Size = new Size(buttonSize, buttonSize);
            Button_Right.Size = new Size(buttonSize, buttonSize);

            Point downButtonLocation = new Point(this.ClientSize.Width - (buttonSize * 2) - margin, this.ClientSize.Height - buttonSize - margin);
            Button_Down.Location = downButtonLocation;

            Button_Up.Location = new Point(downButtonLocation.X, downButtonLocation.Y - buttonSize - gap);
            Button_Left.Location = new Point(downButtonLocation.X - buttonSize - gap, downButtonLocation.Y);
            Button_Right.Location = new Point(downButtonLocation.X + buttonSize + gap, downButtonLocation.Y);

            if (gameOverPanel != null)
            {
                gameOverPanel.Location = new Point(ClientSize.Width / 2 - gameOverPanel.Width / 2, ClientSize.Height / 2 - gameOverPanel.Height / 2);
            }
            if (explanationPanel != null)
            {
                explanationPanel.Location = new Point(ClientSize.Width / 2 - explanationPanel.Width / 2, ClientSize.Height / 2 - explanationPanel.Height / 2);
            }
        }

        private void GameOver()
        {
            isGameOver = true;
            gameTimer.Stop();

            lblFinalScore.Text = "SCORE: " + score;
            gameOverPanel.Visible = true;
            gameOverPanel.BringToFront();
        }

        private void CreateExplanationUI()
        {
            explanationPanel = new Panel { Size = new Size(600, 550), BackColor = Color.FromArgb(240, 240, 240), BorderStyle = BorderStyle.FixedSingle, Visible = false, Font = new Font("Meiryo UI", 12F) };
            this.Controls.Add(explanationPanel);

            Label titleLabel = new Label { Text = "🐧 ゲームのせつめい 🐧", Font = new Font("Meiryo UI", 24F, FontStyle.Bold), ForeColor = Color.SteelBlue, TextAlign = ContentAlignment.MiddleCenter, Size = new Size(600, 50), Location = new Point(0, 20) };
            explanationPanel.Controls.Add(titleLabel);

            playerPictureBox = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 100), SizeMode = PictureBoxSizeMode.Zoom };
            Label playerLabel = new Label { Text = "◀ このペンギンをそうさします", Location = new Point(130, 110), Size = new Size(400, 30) };
            explanationPanel.Controls.Add(playerPictureBox);
            explanationPanel.Controls.Add(playerLabel);

            obstaclePictureBox = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 160), SizeMode = PictureBoxSizeMode.Zoom };
            Label obstacleLabel = new Label { Text = "◀ このかべはよけてください", Location = new Point(130, 170), Size = new Size(400, 30) };
            explanationPanel.Controls.Add(obstaclePictureBox);
            explanationPanel.Controls.Add(obstacleLabel);

            itemPictureBox = new PictureBox { Size = new Size(40, 40), Location = new Point(80, 220), SizeMode = PictureBoxSizeMode.Zoom };
            Label itemLabel = new Label { Text = "◀ うさぎちゃんをとおると +500点です", Location = new Point(130, 230), Size = new Size(400, 30) };
            explanationPanel.Controls.Add(itemPictureBox);
            explanationPanel.Controls.Add(itemLabel);

            Label controlsTitle = new Label { Text = "🎮 そうさほうほう 🎮", Font = new Font("Meiryo UI", 16F, FontStyle.Bold), ForeColor = Color.SteelBlue, Location = new Point(50, 300), Size = new Size(500, 30) };
            explanationPanel.Controls.Add(controlsTitle);

            Label keyboardLabel = new Label { Text = "キーボード： W (上) A (左) S (下) D (右)", Location = new Point(80, 350), Size = new Size(500, 30) };
            explanationPanel.Controls.Add(keyboardLabel);

            Label screenBtnLabel = new Label { Text = "みぎしたのボタンでもそうさできます。", Location = new Point(80, 390), Size = new Size(500, 30) };
            explanationPanel.Controls.Add(screenBtnLabel);

            Button startButton = new Button { Text = "ゲームスタート！", Font = new Font("Meiryo UI", 16F, FontStyle.Bold), Size = new Size(300, 60), Location = new Point(150, 460), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            startButton.Click += (s, e) => {
                explanationPanel.Visible = false;
                gameTimer.Start();
            };
            explanationPanel.Controls.Add(startButton);
        }

        private void CreateGameOverUI()
        {
            gameOverPanel = new Panel { Size = new Size(400, 300), BackColor = Color.WhiteSmoke, BorderStyle = BorderStyle.FixedSingle, Location = new Point(ClientSize.Width / 2 - 200, ClientSize.Height / 2 - 150), Visible = false };
            Label lblGameOverTitle = new Label { Text = "Game Over", Font = new Font("Arial", 48, FontStyle.Bold), ForeColor = Color.SteelBlue, AutoSize = false, Size = new Size(400, 70), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(0, 30) };
            lblFinalScore = new Label { Text = "SCORE: 0", Font = new Font("Arial", 30, FontStyle.Bold), ForeColor = Color.DodgerBlue, AutoSize = false, Size = new Size(400, 50), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(0, 120) };
            Button btnRestart = new Button { Text = "リスタート", Font = new Font("Meiryo UI", 14, FontStyle.Bold), Size = new Size(180, 60), Location = new Point(20, 210), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            btnRestart.Click += (s, e) => InitializeGame();
            Button btnBackToList = new Button { Text = "一覧に戻る", Font = new Font("Meiryo UI", 14, FontStyle.Bold), Size = new Size(180, 60), Location = new Point(200, 210), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            btnBackToList.Click += (s, e) => Func.CreateMiniGame(this);
            gameOverPanel.Controls.Add(lblGameOverTitle);
            gameOverPanel.Controls.Add(lblFinalScore);
            gameOverPanel.Controls.Add(btnRestart);
            gameOverPanel.Controls.Add(btnBackToList);
            this.Controls.Add(gameOverPanel);
        }

        #endregion

        #region 入力処理
        private void Button_Up_MouseDown(object sender, MouseEventArgs e) { if (!isGameOver) goUp = true; }
        private void Button_Up_MouseUp(object sender, MouseEventArgs e) { goUp = false; }
        private void Button_Down_MouseDown(object sender, MouseEventArgs e) { if (!isGameOver) goDown = true; }
        private void Button_Down_MouseUp(object sender, MouseEventArgs e) { goDown = false; }
        private void Button_Left_MouseDown(object sender, MouseEventArgs e) { if (!isGameOver) goLeft = true; }
        private void Button_Left_MouseUp(object sender, MouseEventArgs e) { goLeft = false; }
        private void Button_Right_MouseDown(object sender, MouseEventArgs e) { if (!isGameOver) goRight = true; }
        private void Button_Right_MouseUp(object sender, MouseEventArgs e) { goRight = false; }

        private void MiniGame_Mario_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) this.Close();
            if (isGameOver)
            {
                if (e.KeyCode == Keys.Enter) InitializeGame();
                return;
            }
            bool keyProcessed = false;
            if (e.KeyCode == Keys.W) { goUp = true; keyProcessed = true; }
            if (e.KeyCode == Keys.S) { goDown = true; keyProcessed = true; }
            if (e.KeyCode == Keys.A) { goLeft = true; keyProcessed = true; }
            if (e.KeyCode == Keys.D) { goRight = true; keyProcessed = true; }
            if (keyProcessed)
            {
                e.Handled = true;
            }
        }

        private void MiniGame_Mario_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) { goUp = false; }
            if (e.KeyCode == Keys.S) { goDown = false; }
            if (e.KeyCode == Keys.A) { goLeft = false; }
            if (e.KeyCode == Keys.D) { goRight = false; }
        }
        #endregion
    }
}