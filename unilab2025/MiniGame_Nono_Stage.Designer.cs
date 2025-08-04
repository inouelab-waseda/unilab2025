namespace unilab2025
{
    partial class MiniGame_Nono_Stage
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
            this.pictureBox_Grid = new System.Windows.Forms.PictureBox();
            this.pictureBox_TopHints = new System.Windows.Forms.PictureBox();
            this.pictureBox_SideHints = new System.Windows.Forms.PictureBox();
            this.button_Undo = new System.Windows.Forms.Button();
            this.button_Reset = new System.Windows.Forms.Button();
            this.button_Back = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopHints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SideHints)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Grid
            // 
            this.pictureBox_Grid.Location = new System.Drawing.Point(550, 180);
            this.pictureBox_Grid.Name = "pictureBox_Grid";
            this.pictureBox_Grid.Size = new System.Drawing.Size(605, 605);
            this.pictureBox_Grid.TabIndex = 0;
            this.pictureBox_Grid.TabStop = false;
            // 
            // pictureBox_TopHints
            // 
            this.pictureBox_TopHints.Location = new System.Drawing.Point(550, 30);
            this.pictureBox_TopHints.Name = "pictureBox_TopHints";
            this.pictureBox_TopHints.Size = new System.Drawing.Size(605, 150);
            this.pictureBox_TopHints.TabIndex = 1;
            this.pictureBox_TopHints.TabStop = false;
            // 
            // pictureBox_SideHints
            // 
            this.pictureBox_SideHints.Location = new System.Drawing.Point(400, 180);
            this.pictureBox_SideHints.Name = "pictureBox_SideHints";
            this.pictureBox_SideHints.Size = new System.Drawing.Size(150, 605);
            this.pictureBox_SideHints.TabIndex = 2;
            this.pictureBox_SideHints.TabStop = false;
            // 
            // button_Undo
            // 
            this.button_Undo.Location = new System.Drawing.Point(1243, 149);
            this.button_Undo.Name = "button_Undo";
            this.button_Undo.Size = new System.Drawing.Size(159, 86);
            this.button_Undo.TabIndex = 3;
            this.button_Undo.Text = "１手戻る";
            this.button_Undo.UseVisualStyleBackColor = true;
            this.button_Undo.Click += new System.EventHandler(this.button_Undo_Click);
            // 
            // button_Reset
            // 
            this.button_Reset.Location = new System.Drawing.Point(1408, 149);
            this.button_Reset.Name = "button_Reset";
            this.button_Reset.Size = new System.Drawing.Size(153, 86);
            this.button_Reset.TabIndex = 4;
            this.button_Reset.Text = "最初から";
            this.button_Reset.UseVisualStyleBackColor = true;
            this.button_Reset.Click += new System.EventHandler(this.button_Reset_Click);
            // 
            // button_Back
            // 
            this.button_Back.Location = new System.Drawing.Point(12, 30);
            this.button_Back.Name = "button_Back";
            this.button_Back.Size = new System.Drawing.Size(295, 119);
            this.button_Back.TabIndex = 5;
            this.button_Back.Text = "button1";
            this.button_Back.UseVisualStyleBackColor = true;
            this.button_Back.Click += new System.EventHandler(this.button_Back_Click);
            // 
            // MiniGame_Nono_Stage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 841);
            this.Controls.Add(this.button_Back);
            this.Controls.Add(this.button_Reset);
            this.Controls.Add(this.button_Undo);
            this.Controls.Add(this.pictureBox_SideHints);
            this.Controls.Add(this.pictureBox_TopHints);
            this.Controls.Add(this.pictureBox_Grid);
            this.Name = "MiniGame_Nono_Stage";
            this.Text = "MiniGame_Nono_Stage";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopHints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SideHints)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_Grid;
        private System.Windows.Forms.PictureBox pictureBox_TopHints;
        private System.Windows.Forms.PictureBox pictureBox_SideHints;
        private System.Windows.Forms.Button button_Undo;
        private System.Windows.Forms.Button button_Reset;
        private System.Windows.Forms.Button button_Back;
    }
}