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
            this.button_explain = new System.Windows.Forms.Button();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_keep = new System.Windows.Forms.Button();
            this.button_back = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
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
            this.button_Return.Location = new System.Drawing.Point(28, 9);
            this.button_Return.Name = "button_Return";
            this.button_Return.Size = new System.Drawing.Size(198, 82);
            this.button_Return.TabIndex = 1;
            this.button_Return.Text = "一覧に戻る";
            this.button_Return.UseVisualStyleBackColor = true;
            this.button_Return.Click += new System.EventHandler(this.button_Return_Click);
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(28, 175);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(333, 108);
            this.button_Reset.TabIndex = 2;
            this.button_Reset.Text = "あたらしくはじめる";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Visible = false;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // label_Time
            // 
            this.label_Time.AutoSize = true;
            this.label_Time.Font = new System.Drawing.Font("MS UI Gothic", 30F);
            this.label_Time.Location = new System.Drawing.Point(1346, 9);
            this.label_Time.Name = "label_Time";
            this.label_Time.Size = new System.Drawing.Size(254, 40);
            this.label_Time.TabIndex = 3;
            this.label_Time.Text = "Time: 00:00:00";
            // 
            // timer1
            // 
            this.timer1.Interval = 50;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button_explain
            // 
            this.button_explain.Location = new System.Drawing.Point(264, 9);
            this.button_explain.Name = "button_explain";
            this.button_explain.Size = new System.Drawing.Size(198, 82);
            this.button_explain.TabIndex = 4;
            this.button_explain.Text = "せつめい";
            this.button_explain.UseVisualStyleBackColor = true;
            this.button_explain.Click += new System.EventHandler(this.button_explain_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.settingsPanel.Controls.Add(this.label10);
            this.settingsPanel.Controls.Add(this.label9);
            this.settingsPanel.Controls.Add(this.label8);
            this.settingsPanel.Controls.Add(this.comboBox1);
            this.settingsPanel.Controls.Add(this.label7);
            this.settingsPanel.Controls.Add(this.label6);
            this.settingsPanel.Controls.Add(this.label5);
            this.settingsPanel.Controls.Add(this.button_keep);
            this.settingsPanel.Controls.Add(this.button_back);
            this.settingsPanel.Controls.Add(this.label4);
            this.settingsPanel.Controls.Add(this.label3);
            this.settingsPanel.Controls.Add(this.label2);
            this.settingsPanel.Controls.Add(this.label1);
            this.settingsPanel.Controls.Add(this.numericUpDown3);
            this.settingsPanel.Controls.Add(this.numericUpDown2);
            this.settingsPanel.Controls.Add(this.numericUpDown1);
            this.settingsPanel.Location = new System.Drawing.Point(503, 62);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(880, 720);
            this.settingsPanel.TabIndex = 6;
            this.settingsPanel.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            this.label10.Location = new System.Drawing.Point(107, 470);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(337, 14);
            this.label10.TabIndex = 14;
            this.label10.Text = "（クリックしたときにどれくらいたくさんマスがひらくかきめられるよ）";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label9.Location = new System.Drawing.Point(616, 431);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(168, 20);
            this.label9.TabIndex = 13;
            this.label9.Text = "（デフォルト: たくさん）";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label8.Location = new System.Drawing.Point(106, 431);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(147, 20);
            this.label8.TabIndex = 12;
            this.label8.Text = "ひろがりのおおきさ";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("MS UI Gothic", 20F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "たくさん",
            "ふつう",
            "すこし"});
            this.comboBox1.Location = new System.Drawing.Point(429, 422);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(150, 35);
            this.comboBox1.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label7.Location = new System.Drawing.Point(616, 335);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 20);
            this.label7.TabIndex = 11;
            this.label7.Text = "（デフォルト: 40）";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label6.Location = new System.Drawing.Point(616, 243);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(132, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "（デフォルト: 20）";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label5.Location = new System.Drawing.Point(616, 148);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "（デフォルト: 20）";
            // 
            // button_keep
            // 
            this.button_keep.Font = new System.Drawing.Font("MS UI Gothic", 20F);
            this.button_keep.Location = new System.Drawing.Point(490, 525);
            this.button_keep.Name = "button_keep";
            this.button_keep.Size = new System.Drawing.Size(315, 104);
            this.button_keep.TabIndex = 8;
            this.button_keep.Text = "ほぞんしてもどる";
            this.button_keep.UseVisualStyleBackColor = true;
            this.button_keep.Click += new System.EventHandler(this.button_keep_Click);
            // 
            // button_back
            // 
            this.button_back.Font = new System.Drawing.Font("MS UI Gothic", 20F);
            this.button_back.Location = new System.Drawing.Point(76, 525);
            this.button_back.Name = "button_back";
            this.button_back.Size = new System.Drawing.Size(305, 104);
            this.button_back.TabIndex = 7;
            this.button_back.Text = "デフォルトにもどす";
            this.button_back.UseVisualStyleBackColor = true;
            this.button_back.Click += new System.EventHandler(this.button_back_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("MS UI Gothic", 30F);
            this.label4.Location = new System.Drawing.Point(272, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(229, 40);
            this.label4.TabIndex = 6;
            this.label4.Text = "ゲームの設定";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label3.Location = new System.Drawing.Point(106, 341);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "ばくだんのかず";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label2.Location = new System.Drawing.Point(106, 243);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "はば（よこの長さ）";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 15F);
            this.label1.Location = new System.Drawing.Point(106, 148);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "たかさ（たての長さ）";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Font = new System.Drawing.Font("MS UI Gothic", 20F);
            this.numericUpDown3.Location = new System.Drawing.Point(429, 332);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(150, 34);
            this.numericUpDown3.TabIndex = 2;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Font = new System.Drawing.Font("MS UI Gothic", 20F);
            this.numericUpDown2.Location = new System.Drawing.Point(429, 235);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(150, 34);
            this.numericUpDown2.TabIndex = 1;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Font = new System.Drawing.Font("MS UI Gothic", 20F);
            this.numericUpDown1.Location = new System.Drawing.Point(429, 140);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(150, 34);
            this.numericUpDown1.TabIndex = 0;
            // 
            // MiniGame_Mine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 841);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.button_explain);
            this.Controls.Add(this.label_Time);
            this.Controls.Add(this.button_Reset);
            this.Controls.Add(this.button_Return);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MiniGame_Mine";
            this.Text = "minesweeper";
            this.Load += new System.EventHandler(this.minesweeper_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_Return;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Label label_Time;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button_explain;
        private System.Windows.Forms.Panel settingsPanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Button button_keep;
        private System.Windows.Forms.Button button_back;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}