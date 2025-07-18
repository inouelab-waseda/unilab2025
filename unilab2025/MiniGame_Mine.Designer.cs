namespace unilab2025
{
    partial class MiniGame_Mine
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_Return = new System.Windows.Forms.Button();
            this.button_Reset = new System.Windows.Forms.Button();
            this.label_Time = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(453, 27);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(971, 755);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button_Return
            // 
            this.button_Return.Location = new System.Drawing.Point(28, 58);
            this.button_Return.Name = "button_Return";
            this.button_Return.Size = new System.Drawing.Size(333, 122);
            this.button_Return.TabIndex = 1;
            this.button_Return.Text = "一覧に戻る";
            this.button_Return.UseVisualStyleBackColor = true;
            this.button_Return.Click += new System.EventHandler(this.button_Return_Click);
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(1463, 300);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(182, 93);
            this.button_Reset.TabIndex = 2;
            this.button_Reset.Text = "リセット";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // label_Time
            // 
            this.label_Time.AutoSize = true;
            this.label_Time.Font = new System.Drawing.Font("MS UI Gothic", 30F);
            this.label_Time.Location = new System.Drawing.Point(1345, 82);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(319, 50);
            this.label_Time.TabIndex = 3;
            this.label_Time.Text = "Time: 00:00:00";
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MiniGame_Mine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 841);
            this.Controls.Add(this.label_Time);
            this.Controls.Add(this.button_Reset);
            this.Controls.Add(this.button_Return);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MiniGame_Mine";
            this.Text = "minesweeper";
            this.Load += new System.EventHandler(this.minesweeper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_Return;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Label label_Time;
        private System.Windows.Forms.Timer timer1;
    }
}