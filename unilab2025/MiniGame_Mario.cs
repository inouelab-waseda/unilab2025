using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace unilab2025
{
    #region ゲーム設定クラス
    public class GameSettings
    {
        public const int DefaultPlayerSize = 40;
        public const int DefaultObstacleSize = 30;
        public const int DefaultPlayerSpeed = 3;
        public const int DefaultBaseScrollSpeed = 1;
        // パーセント表記 (1-100%) に変更
        public const int DefaultItemSpawnChance = 2; // 2%
        public const int DefaultObstacleSpawnChance = 10; // 10%

        public int PlayerSize { get; set; }
        public int ObstacleSize { get; set; }
        public int PlayerSpeed { get; set; }
        public int BaseScrollSpeed { get; set; }
        public int ItemSpawnChance { get; set; }
        public int ObstacleSpawnChance { get; set; }

        public GameSettings()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            PlayerSize = DefaultPlayerSize;
            ObstacleSize = DefaultObstacleSize;
            PlayerSpeed = DefaultPlayerSpeed;
            BaseScrollSpeed = DefaultBaseScrollSpeed;
            ItemSpawnChance = DefaultItemSpawnChance;
            ObstacleSpawnChance = DefaultObstacleSpawnChance;
        }
    }
    #endregion

    public partial class MiniGame_Mario : Form
    {
        #region 変数
        private GameSettings gameSettings = new GameSettings();
        Dictionary<string, Image> playerImages = new Dictionary<string, Image>();
        Image currentPlayerImage;
        string lastDirection;
        Image obstacleImage;
        Image backgroundImage;
        Image itemImage;
        Rectangle playerRect;
        List<Rectangle> obstacles = new List<Rectangle>();
        List<Rectangle> items = new List<Rectangle>();
        private class FloatingTextEffect
        {
            public string Text;
            public PointF Position;
            public int Lifetime;
            public Font EffectFont; // 追加
            public Brush EffectBrush; // 追加

            public void Update()
            {
                Position = new PointF(Position.X, Position.Y - 1.5f);
                Lifetime--;
            }
        }
        private List<FloatingTextEffect> effects = new List<FloatingTextEffect>();
        private Font effectFont = new Font("Arial", 16, FontStyle.Bold);
        private Brush effectBrush = Brushes.Gold;
        // 追加
        private Font speedUpFont = new Font("Impact", 28, FontStyle.Italic);
        private Brush speedUpBrush = Brushes.OrangeRed;

        bool goUp, goDown, goLeft, goRight, isGameOver;
        int score;
        int elapsedTime;
        Random rnd = new Random();
        int obstacleSpeed;
        private int lastSpawnX;
        private int backgroundX = 0;
        Panel gameOverPanel;
        Label lblFinalScore;
        Panel explanationPanel;
        PictureBox playerPictureBox;
        PictureBox obstaclePictureBox;
        PictureBox itemPictureBox;
        Panel optionsPanel;
        NumericUpDown nudPlayerSize, nudObstacleSize, nudPlayerSpeed, nudScrollSpeed, nudItemChance, nudObstacleChance;
        #endregion

        public MiniGame_Mario()
        {
            InitializeComponent();
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
            CreateGameOverUI();
            CreateExplanationUI();
            CreateOptionsUI();
            InitializeGame();
        }

        #region 初期化処理
        private void InitializeGame()
        {
            gameTimer.Stop();
            isGameOver = false;
            goUp = goDown = goLeft = goRight = false;
            score = 0;
            elapsedTime = 0;
            backgroundX = 0;
            obstacleSpeed = gameSettings.BaseScrollSpeed;
            lblScore.Text = "Score: 0";
            lblScore.Parent = pbCanvas;
            lblScore.BackColor = Color.Transparent;
            playerPictureBox.Image = playerImages["Right"];
            obstaclePictureBox.Image = obstacleImage;
            itemPictureBox.Image = itemImage;

            int playerSize = gameSettings.PlayerSize;
            playerRect = new Rectangle(150, this.ClientSize.Height / 2 - playerSize / 2, playerSize, playerSize);

            obstacles.Clear();
            items.Clear();
            effects.Clear();

            int safeZoneEndX = playerRect.X + 300;
            int obstacleSize = gameSettings.ObstacleSize;
            int worldWidth = this.ClientSize.Width + obstacleSize;
            for (int x = 0; x < worldWidth; x += obstacleSize)
            {
                bool inSafeZone = x > playerRect.X - obstacleSize && x < safeZoneEndX;
                if (!inSafeZone)
                {
                    int numberOfLanes = pbCanvas.Height / obstacleSize + 1;
                    for (int i = 0; i < numberOfLanes; i++)
                    {
                        int y = i * obstacleSize;
                        if (rnd.Next(1, 101) <= gameSettings.ObstacleSpawnChance)
                        {
                            obstacles.Add(new Rectangle(x, y, obstacleSize, obstacleSize));
                        }
                        else if (rnd.Next(1, 101) <= gameSettings.ItemSpawnChance)
                        {
                            items.Add(new Rectangle(x, y, obstacleSize, obstacleSize));
                        }
                    }
                }
            }
            lastSpawnX = worldWidth;

            this.ActiveControl = pbCanvas;
            gameOverPanel.Visible = false;
            optionsPanel.Visible = false;

            explanationPanel.Visible = true;
            explanationPanel.BringToFront();
            pbCanvas.Visible = true;
            pbCanvas.Invalidate();
        }
        #endregion

        #region ゲームループと描画処理
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (isGameOver) return;
            playerRect.Width = gameSettings.PlayerSize;
            playerRect.Height = gameSettings.PlayerSize;
            elapsedTime++;
            score++;
            lblScore.Text = "Score: " + score;
            backgroundX -= obstacleSpeed;
            if (goUp && playerRect.Top > 0) playerRect.Y -= gameSettings.PlayerSpeed;
            if (goDown && playerRect.Bottom < pbCanvas.Height) playerRect.Y += gameSettings.PlayerSpeed;
            if (goLeft && playerRect.Left > 0) playerRect.X -= gameSettings.PlayerSpeed;
            if (goRight && playerRect.Right < pbCanvas.Width) playerRect.X += gameSettings.PlayerSpeed;
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
                    // 変更
                    effects.Add(new FloatingTextEffect
                    {
                        Text = "+500",
                        Position = new PointF(rect.X, rect.Y),
                        Lifetime = 60,
                        EffectFont = this.effectFont,
                        EffectBrush = this.effectBrush
                    });
                    items.RemoveAt(i);
                    continue;
                }
                if (rect.Right < 0) { items.RemoveAt(i); }
            }
            for (int i = effects.Count - 1; i >= 0; i--)
            {
                effects[i].Update();
                if (effects[i].Lifetime <= 0) { effects.RemoveAt(i); }
            }
            int obstacleSize = gameSettings.ObstacleSize;
            lastSpawnX -= obstacleSpeed;
            while (lastSpawnX < this.ClientSize.Width)
            {
                int x = lastSpawnX;
                int numberOfLanes = pbCanvas.Height / obstacleSize + 1;
                for (int i = 0; i < numberOfLanes; i++)
                {
                    int y = i * obstacleSize;
                    if (rnd.Next(1, 101) <= gameSettings.ObstacleSpawnChance)
                    {
                        obstacles.Add(new Rectangle(x, y, obstacleSize, obstacleSize));
                    }
                    else if (rnd.Next(1, 101) <= gameSettings.ItemSpawnChance)
                    {
                        items.Add(new Rectangle(x, y, obstacleSize, obstacleSize));
                    }
                }
                lastSpawnX += obstacleSize;
            }

            // 変更
            int oldSpeed = obstacleSpeed;

            int maxSpeed = gameSettings.PlayerSpeed + 2;
            int calculatedSpeed = gameSettings.BaseScrollSpeed + (elapsedTime / 1800);
            obstacleSpeed = Math.Min(calculatedSpeed, maxSpeed);

            if (obstacleSpeed > oldSpeed)
            {
                effects.Add(new FloatingTextEffect
                {
                    Text = "SPEED UP!!",
                    Position = new PointF(pbCanvas.Width / 2 - 100, pbCanvas.Height / 2 - 50),
                    Lifetime = 90,
                    EffectFont = this.speedUpFont,
                    EffectBrush = this.speedUpBrush
                });
            }

            pbCanvas.Invalidate();
        }
        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (backgroundImage != null)
            {
                int tileWidth = gameSettings.ObstacleSize;
                int tileHeight = gameSettings.ObstacleSize;
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
            if (obstacleImage != null) { foreach (Rectangle rect in obstacles) { e.Graphics.DrawImage(obstacleImage, rect); } }
            if (itemImage != null) { foreach (Rectangle rect in items) { e.Graphics.DrawImage(itemImage, rect); } }
            if (currentPlayerImage != null) { e.Graphics.DrawImage(currentPlayerImage, playerRect); }
            else { e.Graphics.FillRectangle(Brushes.Red, playerRect); }
            foreach (var effect in effects)
            {
                // 変更
                e.Graphics.DrawString(effect.Text, effect.EffectFont, effect.EffectBrush, effect.Position);
            }
        }
        #endregion

        #region UIとゲーム状態
        private void CreateExplanationUI()
        {
            explanationPanel = new Panel { Size = new Size(600, 550), BackColor = Color.FromArgb(220, 240, 240, 240), BorderStyle = BorderStyle.FixedSingle, Visible = false, Font = new Font("Meiryo UI", 12F) };
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
            Button startButton = new Button { Text = "ゲームスタート！", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(320, 460), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            startButton.Click += (s, e) =>
            {
                explanationPanel.Visible = false;
                gameTimer.Start();
            };
            explanationPanel.Controls.Add(startButton);
            Button optionsButton = new Button { Text = "オプション", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(60, 460), BackColor = Color.White, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            optionsButton.Click += (s, e) =>
            {
                explanationPanel.Visible = false;
                ShowOptionsPanel();
            };
            explanationPanel.Controls.Add(optionsButton);
        }

        private void SaveSettingsAndClose(object sender, EventArgs e)
        {
            gameSettings.PlayerSize = (int)nudPlayerSize.Value;
            gameSettings.ObstacleSize = (int)nudObstacleSize.Value;
            gameSettings.PlayerSpeed = (int)nudPlayerSpeed.Value;
            gameSettings.BaseScrollSpeed = (int)nudScrollSpeed.Value;
            gameSettings.ObstacleSpawnChance = (int)nudObstacleChance.Value;
            gameSettings.ItemSpawnChance = (int)nudItemChance.Value;
            InitializeGame();
        }

        private void ShowOptionsPanel()
        {
            nudPlayerSize.Value = gameSettings.PlayerSize;
            nudObstacleSize.Value = gameSettings.ObstacleSize;
            nudPlayerSpeed.Value = gameSettings.PlayerSpeed;
            nudScrollSpeed.Value = gameSettings.BaseScrollSpeed;
            nudObstacleChance.Value = gameSettings.ObstacleSpawnChance;
            nudItemChance.Value = gameSettings.ItemSpawnChance;
            optionsPanel.Visible = true;
            optionsPanel.BringToFront();
        }

        private void GameOver()
        {
            isGameOver = true;
            gameTimer.Stop();
            lblFinalScore.Text = "SCORE: " + score;
            gameOverPanel.Visible = true;
            gameOverPanel.BringToFront();
        }

        private void CreateOptionsUI()
        {
            optionsPanel = new Panel { Size = new Size(600, 550), BackColor = Color.FromArgb(220, 240, 240, 240), BorderStyle = BorderStyle.FixedSingle, Visible = false, Font = new Font("Meiryo UI", 10F) };
            this.Controls.Add(optionsPanel);
            Label titleLabel = new Label { Text = "⚙ ゲームせってい ⚙", Font = new Font("Meiryo UI", 24F, FontStyle.Bold), ForeColor = Color.SteelBlue, TextAlign = ContentAlignment.MiddleCenter, Size = new Size(600, 50), Location = new Point(0, 20) };
            optionsPanel.Controls.Add(titleLabel);
            int startY = 100;
            int spacingY = 45;
            AddOptionControl("ペンギンの大きさ (10-300px):", startY, out nudPlayerSize, GameSettings.DefaultPlayerSize, 10, 300, 1);
            AddOptionControl("ブロックの大きさ (20-100px):", startY + spacingY, out nudObstacleSize, GameSettings.DefaultObstacleSize, 20, 100, 1);
            AddOptionControl("プレイヤーのはやさ (1-10):", startY + spacingY * 2, out nudPlayerSpeed, GameSettings.DefaultPlayerSpeed, 1, 10, 1);
            AddOptionControl("スクロールのはやさ (1-8):", startY + spacingY * 3, out nudScrollSpeed, GameSettings.DefaultBaseScrollSpeed, 1, 8, 1);
            AddOptionControl("かべのでてくる量 (1-100%):", startY + spacingY * 4, out nudObstacleChance, GameSettings.DefaultObstacleSpawnChance, 1, 100, 1);
            AddOptionControl("うさぎちゃんの出てくる量 (1-100%):", startY + spacingY * 5, out nudItemChance, GameSettings.DefaultItemSpawnChance, 1, 100, 1);
            Button saveButton = new Button { Text = "ほぞんしてもどる", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(320, 460), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            saveButton.Click += SaveSettingsAndClose;
            optionsPanel.Controls.Add(saveButton);
            Button defaultButton = new Button { Text = "デフォルトにもどす", Font = new Font("Meiryo UI", 14F, FontStyle.Bold), Size = new Size(220, 60), Location = new Point(60, 460), BackColor = Color.White, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            defaultButton.Click += (s, e) => SetOptionsToDefault();
            optionsPanel.Controls.Add(defaultButton);
        }

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
            if (gameOverPanel != null) { gameOverPanel.Location = new Point(ClientSize.Width / 2 - gameOverPanel.Width / 2, ClientSize.Height / 2 - gameOverPanel.Height / 2); }
            if (explanationPanel != null) { explanationPanel.Location = new Point(ClientSize.Width / 2 - explanationPanel.Width / 2, ClientSize.Height / 2 - explanationPanel.Height / 2); }
            if (optionsPanel != null) { optionsPanel.Location = new Point(ClientSize.Width / 2 - optionsPanel.Width / 2, ClientSize.Height / 2 - optionsPanel.Height / 2); }
        }
        private void AddOptionControl(string labelText, int y, out NumericUpDown nud, int defaultValue, int min, int max, int increment)
        {
            Label label = new Label { Text = labelText, Location = new Point(50, y + 5), Size = new Size(250, 20) };
            optionsPanel.Controls.Add(label);
            nud = new NumericUpDown { Minimum = min, Maximum = max, Value = defaultValue, Increment = increment, Location = new Point(310, y), Size = new Size(80, 20) };
            optionsPanel.Controls.Add(nud);
            Label defaultLabel = new Label { Text = $"(デフォルト: {defaultValue})", Location = new Point(410, y + 5), Size = new Size(150, 20), ForeColor = Color.Gray };
            optionsPanel.Controls.Add(defaultLabel);
        }
        private void SetOptionsToDefault()
        {
            nudPlayerSize.Value = GameSettings.DefaultPlayerSize;
            nudObstacleSize.Value = GameSettings.DefaultObstacleSize;
            nudPlayerSpeed.Value = GameSettings.DefaultPlayerSpeed;
            nudScrollSpeed.Value = GameSettings.DefaultBaseScrollSpeed;
            nudObstacleChance.Value = GameSettings.DefaultObstacleSpawnChance;
            nudItemChance.Value = GameSettings.DefaultItemSpawnChance;
        }
        private void CreateGameOverUI()
        {
            gameOverPanel = new Panel { Size = new Size(400, 300), BackColor = Color.FromArgb(220, 255, 255, 255), BorderStyle = BorderStyle.FixedSingle, Location = new Point(ClientSize.Width / 2 - 200, ClientSize.Height / 2 - 150), Visible = false };
            Label lblGameOverTitle = new Label { Text = "Game Over", Font = new Font("Arial", 48, FontStyle.Bold), ForeColor = Color.SteelBlue, AutoSize = false, Size = new Size(400, 70), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(0, 30) };
            lblFinalScore = new Label { Text = "SCORE: 0", Font = new Font("Arial", 30, FontStyle.Bold), ForeColor = Color.DodgerBlue, AutoSize = false, Size = new Size(400, 50), TextAlign = ContentAlignment.MiddleCenter, Location = new Point(0, 120) };
            Button btnRestart = new Button { Text = "リスタート", Font = new Font("Meiryo UI", 14, FontStyle.Bold), Size = new Size(180, 60), Location = new Point(20, 210), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
            btnRestart.Click += (s, e) => InitializeGame();
            Button btnBackToList = new Button { Text = "いちらんにもどる", Font = new Font("Meiryo UI", 14, FontStyle.Bold), Size = new Size(180, 60), Location = new Point(200, 210), BackColor = Color.LightCyan, FlatStyle = FlatStyle.Flat, ForeColor = Color.SteelBlue };
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