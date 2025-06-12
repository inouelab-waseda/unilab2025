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
            this.button_Try = new System.Windows.Forms.Button();
            this.button_Stage = new System.Windows.Forms.Button();
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
            // button_Try
            // 
            this.button_Try.Location = new System.Drawing.Point(561, 239);
            this.button_Try.Name = "button_Try";
            this.button_Try.Size = new System.Drawing.Size(403, 117);
            this.button_Try.TabIndex = 2;
            this.button_Try.Text = "チュートリアル";
            this.button_Try.UseVisualStyleBackColor = true;
            // 
            // button_Stage
            // 
            this.button_Stage.Location = new System.Drawing.Point(561, 433);
            this.button_Stage.Name = "button_Stage";
            this.button_Stage.Size = new System.Drawing.Size(403, 114);
            this.button_Stage.TabIndex = 3;
            this.button_Stage.Text = "ステージ";
            this.button_Stage.UseVisualStyleBackColor = true;
            // 
            // StageSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1500, 806);
            this.Controls.Add(this.button_Stage);
            this.Controls.Add(this.button_Try);
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
        private System.Windows.Forms.Button button_Try;
        private System.Windows.Forms.Button button_Stage;
    }
}