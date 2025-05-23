namespace unilab2025
{
    partial class Stage
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox_Background = new System.Windows.Forms.PictureBox();
            this.pictureBox_Map = new System.Windows.Forms.PictureBox();
            this.button_Start = new System.Windows.Forms.Button();
            this.button_Reset = new System.Windows.Forms.Button();
            this.pictureBox_Car = new System.Windows.Forms.PictureBox();
            this.pictureBox_Order = new System.Windows.Forms.PictureBox();
            this.listBox_Order = new System.Windows.Forms.ListBox();
            this.listBox_Car = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button_left = new System.Windows.Forms.Button();
            this.button_up = new System.Windows.Forms.Button();
            this.button_right = new System.Windows.Forms.Button();
            this.button_down = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Background)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Map)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Car)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Order)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Background
            // 
            this.pictureBox_Background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_Background.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_Background.Name = "pictureBox_Background";
            this.pictureBox_Background.Size = new System.Drawing.Size(1485, 798);
            this.pictureBox_Background.TabIndex = 0;
            this.pictureBox_Background.TabStop = false;
            // 
            // pictureBox_Map
            // 
            this.pictureBox_Map.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pictureBox_Map.Location = new System.Drawing.Point(22, 23);
            this.pictureBox_Map.Name = "pictureBox_Map";
            this.pictureBox_Map.Size = new System.Drawing.Size(472, 455);
            this.pictureBox_Map.TabIndex = 1;
            this.pictureBox_Map.TabStop = false;
            // 
            // button_Start
            // 
            this.button_Start.Location = new System.Drawing.Point(272, 515);
            this.button_Start.Name = "button_Start";
            this.button_Start.Size = new System.Drawing.Size(188, 86);
            this.button_Start.TabIndex = 2;
            this.button_Start.Text = "出発！";
            this.button_Start.UseVisualStyleBackColor = true;
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(22, 515);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(183, 86);
            this.button_Reset.TabIndex = 3;
            this.button_Reset.Text = "リセット";
            this.button_Reset.UseVisualStyleBackColor = true;
            // 
            // pictureBox_Car
            // 
            this.pictureBox_Car.Location = new System.Drawing.Point(892, 23);
            this.pictureBox_Car.Name = "pictureBox_Car";
            this.pictureBox_Car.Size = new System.Drawing.Size(276, 382);
            this.pictureBox_Car.TabIndex = 4;
            this.pictureBox_Car.TabStop = false;
            // 
            // pictureBox_Order
            // 
            this.pictureBox_Order.Location = new System.Drawing.Point(556, 23);
            this.pictureBox_Order.Name = "pictureBox_Order";
            this.pictureBox_Order.Size = new System.Drawing.Size(288, 382);
            this.pictureBox_Order.TabIndex = 5;
            this.pictureBox_Order.TabStop = false;
            // 
            // listBox_Order
            // 
            this.listBox_Order.FormattingEnabled = true;
            this.listBox_Order.ItemHeight = 15;
            this.listBox_Order.Location = new System.Drawing.Point(589, 99);
            this.listBox_Order.Name = "listBox_Order";
            this.listBox_Order.Size = new System.Drawing.Size(222, 274);
            this.listBox_Order.TabIndex = 6;
            // 
            // listBox_Car
            // 
            this.listBox_Car.FormattingEnabled = true;
            this.listBox_Car.ItemHeight = 15;
            this.listBox_Car.Location = new System.Drawing.Point(923, 99);
            this.listBox_Car.Name = "listBox_Car";
            this.listBox_Car.Size = new System.Drawing.Size(216, 274);
            this.listBox_Car.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(962, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 28);
            this.label1.TabIndex = 8;
            this.label1.Text = "車の設定";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(628, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 28);
            this.label2.TabIndex = 9;
            this.label2.Text = "動きの設定";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(914, 471);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1050, 471);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(914, 548);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1050, 548);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 13;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(633, 471);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(118, 99);
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // button_left
            // 
            this.button_left.Location = new System.Drawing.Point(531, 515);
            this.button_left.Name = "button_left";
            this.button_left.Size = new System.Drawing.Size(75, 23);
            this.button_left.TabIndex = 15;
            this.button_left.Text = "button5";
            this.button_left.UseVisualStyleBackColor = true;
            // 
            // button_up
            // 
            this.button_up.Location = new System.Drawing.Point(656, 429);
            this.button_up.Name = "button_up";
            this.button_up.Size = new System.Drawing.Size(75, 23);
            this.button_up.TabIndex = 16;
            this.button_up.Text = "button6";
            this.button_up.UseVisualStyleBackColor = true;
            // 
            // button_right
            // 
            this.button_right.Location = new System.Drawing.Point(791, 515);
            this.button_right.Name = "button_right";
            this.button_right.Size = new System.Drawing.Size(75, 23);
            this.button_right.TabIndex = 17;
            this.button_right.Text = "button7";
            this.button_right.UseVisualStyleBackColor = true;
            // 
            // button_down
            // 
            this.button_down.Location = new System.Drawing.Point(656, 578);
            this.button_down.Name = "button_down";
            this.button_down.Size = new System.Drawing.Size(75, 23);
            this.button_down.TabIndex = 18;
            this.button_down.Text = "button8";
            this.button_down.UseVisualStyleBackColor = true;
            // 
            // Stage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1485, 798);
            this.Controls.Add(this.button_down);
            this.Controls.Add(this.button_right);
            this.Controls.Add(this.button_up);
            this.Controls.Add(this.button_left);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Car);
            this.Controls.Add(this.listBox_Order);
            this.Controls.Add(this.pictureBox_Order);
            this.Controls.Add(this.pictureBox_Car);
            this.Controls.Add(this.button_Reset);
            this.Controls.Add(this.button_Start);
            this.Controls.Add(this.pictureBox_Map);
            this.Controls.Add(this.pictureBox_Background);
            this.Name = "Stage";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Stage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Background)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Map)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Car)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Order)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Background;
        private System.Windows.Forms.PictureBox pictureBox_Map;
        private System.Windows.Forms.Button button_Start;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.PictureBox pictureBox_Car;
        private System.Windows.Forms.PictureBox pictureBox_Order;
        private System.Windows.Forms.ListBox listBox_Order;
        private System.Windows.Forms.ListBox listBox_Car;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button_left;
        private System.Windows.Forms.Button button_up;
        private System.Windows.Forms.Button button_right;
        private System.Windows.Forms.Button button_down;
    }
}

