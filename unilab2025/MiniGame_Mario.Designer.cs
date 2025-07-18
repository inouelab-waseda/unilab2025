namespace unilab2025
{
    partial class MiniGame_Mario
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pbCanvas = new System.Windows.Forms.PictureBox();
            this.Button_Up = new System.Windows.Forms.Button();
            this.Button_Down = new System.Windows.Forms.Button();
            this.lblScore = new System.Windows.Forms.Label();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.Button_Right = new System.Windows.Forms.Button();
            this.Button_Left = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).BeginInit();
            this.SuspendLayout();
            // 
            // pbCanvas
            // 
            this.pbCanvas.BackColor = System.Drawing.Color.Transparent;
            this.pbCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbCanvas.Location = new System.Drawing.Point(0, 0);
            this.pbCanvas.Margin = new System.Windows.Forms.Padding(2);
            this.pbCanvas.Name = "pbCanvas";
            this.pbCanvas.Size = new System.Drawing.Size(1350, 729);
            this.pbCanvas.TabIndex = 1;
            this.pbCanvas.TabStop = false;
            // 
            // Button_Up
            // 
            this.Button_Up.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Up.Font = new System.Drawing.Font("MS UI Gothic", 45F);
            this.Button_Up.Location = new System.Drawing.Point(1094, 504);
            this.Button_Up.Margin = new System.Windows.Forms.Padding(2);
            this.Button_Up.Name = "Button_Up";
            this.Button_Up.Size = new System.Drawing.Size(90, 100);
            this.Button_Up.TabIndex = 2;
            this.Button_Up.Text = "▲";
            this.Button_Up.UseVisualStyleBackColor = true;
            this.Button_Up.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_Up_MouseDown);
            this.Button_Up.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_Up_MouseUp);
            // 
            // Button_Down
            // 
            this.Button_Down.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Down.Font = new System.Drawing.Font("MS UI Gothic", 45F);
            this.Button_Down.Location = new System.Drawing.Point(1094, 608);
            this.Button_Down.Margin = new System.Windows.Forms.Padding(2);
            this.Button_Down.Name = "Button_Down";
            this.Button_Down.Size = new System.Drawing.Size(90, 100);
            this.Button_Down.TabIndex = 3;
            this.Button_Down.Text = "▼";
            this.Button_Down.UseVisualStyleBackColor = true;
            this.Button_Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Button_Down_MouseDown);
            this.Button_Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Button_Down_MouseUp);
            // 
            // lblScore
            // 
            this.lblScore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("MS UI Gothic", 30F);
            this.lblScore.Location = new System.Drawing.Point(1250, 6);
            this.lblScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(155, 40);
            this.lblScore.TabIndex = 4;
            this.lblScore.Text = "Score: 0";
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 20;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // Button_Right
            // 
            this.Button_Right.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Right.Font = new System.Drawing.Font("MS UI Gothic", 45F);
            this.Button_Right.Location = new System.Drawing.Point(1188, 560);
            this.Button_Right.Margin = new System.Windows.Forms.Padding(2);
            this.Button_Right.Name = "Button_Right";
            this.Button_Right.Size = new System.Drawing.Size(90, 100);
            this.Button_Right.TabIndex = 5;
            this.Button_Right.Text = "▶";
            this.Button_Right.UseVisualStyleBackColor = true;
            // 
            // Button_Left
            // 
            this.Button_Left.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_Left.Font = new System.Drawing.Font("MS UI Gothic", 45F);
            this.Button_Left.Location = new System.Drawing.Point(1000, 560);
            this.Button_Left.Margin = new System.Windows.Forms.Padding(2);
            this.Button_Left.Name = "Button_Left";
            this.Button_Left.Size = new System.Drawing.Size(90, 100);
            this.Button_Left.TabIndex = 6;
            this.Button_Left.Text = "◀";
            this.Button_Left.UseVisualStyleBackColor = true;
            // 
            // MiniGame_Mario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 729);
            this.Controls.Add(this.Button_Left);
            this.Controls.Add(this.Button_Right);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.Button_Down);
            this.Controls.Add(this.Button_Up);
            this.Controls.Add(this.pbCanvas);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MiniGame_Mario";
            this.Text = "MiniGame_Mario";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MiniGame_Mario_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MiniGame_Mario_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.pbCanvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbCanvas;
        private System.Windows.Forms.Button Button_Up;
        private System.Windows.Forms.Button Button_Down;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Button Button_Right;
        private System.Windows.Forms.Button Button_Left;
    }
}