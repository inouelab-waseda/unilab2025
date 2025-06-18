namespace unilab2025
{
    partial class StageSelect
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
            this.pictureBox_Background = new System.Windows.Forms.PictureBox();
            this.button_ToMap = new System.Windows.Forms.Button();
            this.button_Stage1 = new System.Windows.Forms.Button();
            this.button_Stage2 = new System.Windows.Forms.Button();
            this.button_Stage3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Background)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Background
            // 
            this.pictureBox_Background.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox_Background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_Background.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Background.Name = "pictureBox_Background";
            this.pictureBox_Background.Size = new System.Drawing.Size(1500, 806);
            this.pictureBox_Background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Background.TabIndex = 0;
            this.pictureBox_Background.TabStop = false;
            // 
            // button_ToMap
            // 
            this.button_ToMap.Location = new System.Drawing.Point(8, 8);
            this.button_ToMap.Name = "button_ToMap";
            this.button_ToMap.Size = new System.Drawing.Size(244, 102);
            this.button_ToMap.TabIndex = 1;
            this.button_ToMap.Text = "マップに戻る";
            this.button_ToMap.UseVisualStyleBackColor = true;
            this.button_ToMap.Click += new System.EventHandler(this.button_ToMap_Click);
            // 
            // button_Stage1
            // 
            this.button_Stage1.Location = new System.Drawing.Point(561, 239);
            this.button_Stage1.Name = "button_Stage1";
            this.button_Stage1.Size = new System.Drawing.Size(403, 117);
            this.button_Stage1.TabIndex = 2;
            this.button_Stage1.Text = "チュートリアル";
            this.button_Stage1.UseVisualStyleBackColor = true;
            this.button_Stage1.Click += new System.EventHandler(this.button_StageI_Click);
            // 
            // button_Stage2
            // 
            this.button_Stage2.Location = new System.Drawing.Point(561, 433);
            this.button_Stage2.Name = "button_Stage2";
            this.button_Stage2.Size = new System.Drawing.Size(403, 114);
            this.button_Stage2.TabIndex = 3;
            this.button_Stage2.Text = "ステージ";
            this.button_Stage2.UseVisualStyleBackColor = true;
            this.button_Stage2.Click += new System.EventHandler(this.button_StageI_Click);
            // 
            // button_Stage3
            // 
            this.button_Stage3.Location = new System.Drawing.Point(561, 593);
            this.button_Stage3.Name = "button_Stage3";
            this.button_Stage3.Size = new System.Drawing.Size(403, 107);
            this.button_Stage3.TabIndex = 4;
            this.button_Stage3.Text = "button3";
            this.button_Stage3.UseVisualStyleBackColor = true;
            this.button_Stage3.Click += new System.EventHandler(this.button_StageI_Click);
            // 
            // StageSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 806);
            this.Controls.Add(this.button_Stage3);
            this.Controls.Add(this.button_Stage2);
            this.Controls.Add(this.button_Stage1);
            this.Controls.Add(this.button_ToMap);
            this.Controls.Add(this.pictureBox_Background);
            this.Name = "StageSelect";
            this.Text = "StageSelect";
            this.Load += new System.EventHandler(this.StageSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Background)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Background;
        private System.Windows.Forms.Button button_ToMap;
        private System.Windows.Forms.Button button_Stage1;
        private System.Windows.Forms.Button button_Stage2;
        private System.Windows.Forms.Button button_Stage3;
    }
}