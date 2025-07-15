namespace unilab2025
{
    partial class AnotherWorld
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnotherWorld));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_Japan = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1482, 753);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button_Japan
            // 
            this.button_Japan.Location = new System.Drawing.Point(666, 295);
            this.button_Japan.Name = "button_Japan";
            this.button_Japan.Size = new System.Drawing.Size(149, 71);
            this.button_Japan.TabIndex = 1;
            this.button_Japan.Text = "日本";
            this.button_Japan.UseVisualStyleBackColor = true;
            this.button_Japan.Click += new System.EventHandler(this.button_Japan_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(459, 245);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(126, 75);
            this.button5.TabIndex = 2;
            this.button5.Text = "中国";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.buttonI_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(518, 625);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(151, 72);
            this.button6.TabIndex = 3;
            this.button6.Text = "南極";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.buttonI_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(1121, 445);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(137, 56);
            this.button7.TabIndex = 4;
            this.button7.Text = "アマゾン";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.buttonI_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(249, 377);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(154, 76);
            this.button8.TabIndex = 5;
            this.button8.Text = "エジプト";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.buttonI_Click);
            // 
            // AnotherWorld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1482, 753);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button_Japan);
            this.Controls.Add(this.pictureBox1);
            this.Name = "AnotherWorld";
            this.Text = "AnotherWorld";
            this.Load += new System.EventHandler(this.AnotherWorld_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_Japan;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
    }
}