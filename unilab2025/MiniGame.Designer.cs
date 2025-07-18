namespace unilab2025
{
    partial class MiniGame
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniGame));
            this.button_Mario = new System.Windows.Forms.Button();
            this.button_MineSweeper = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button_Mario
            // 
            this.button_Mario.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_Mario.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_Mario.BackgroundImage")));
            this.button_Mario.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Mario.Location = new System.Drawing.Point(93, 160);
            this.button_Mario.Name = "button_Mario";
            this.button_Mario.Size = new System.Drawing.Size(200, 200);
            this.button_Mario.TabIndex = 0;
            this.button_Mario.UseVisualStyleBackColor = true;
            this.button_Mario.Click += new System.EventHandler(this.button_Mario_Click);
            // 
            // button_MineSweeper
            // 
            this.button_MineSweeper.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.button_MineSweeper.Location = new System.Drawing.Point(517, 160);
            this.button_MineSweeper.Name = "button_MineSweeper";
            this.button_MineSweeper.Size = new System.Drawing.Size(200, 200);
            this.button_MineSweeper.TabIndex = 1;
            this.button_MineSweeper.Text = "マインスイーパー";
            this.button_MineSweeper.UseVisualStyleBackColor = true;
            // 
            // MiniGame
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(810, 486);
            this.Controls.Add(this.button_MineSweeper);
            this.Controls.Add(this.button_Mario);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MiniGame";
            this.Text = "MiniGame";
            this.Load += new System.EventHandler(this.MiniGame_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_Mario;
        private System.Windows.Forms.Button button_MineSweeper;
    }
}