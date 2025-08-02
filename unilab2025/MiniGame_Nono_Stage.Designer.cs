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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_TopHints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_SideHints)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_Grid
            // 
            this.pictureBox_Grid.Location = new System.Drawing.Point(538, 366);
            this.pictureBox_Grid.Name = "pictureBox_Grid";
            this.pictureBox_Grid.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_Grid.TabIndex = 0;
            this.pictureBox_Grid.TabStop = false;
            // 
            // pictureBox_TopHints
            // 
            this.pictureBox_TopHints.Location = new System.Drawing.Point(548, 124);
            this.pictureBox_TopHints.Name = "pictureBox_TopHints";
            this.pictureBox_TopHints.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_TopHints.TabIndex = 1;
            this.pictureBox_TopHints.TabStop = false;
            // 
            // pictureBox_SideHints
            // 
            this.pictureBox_SideHints.Location = new System.Drawing.Point(105, 366);
            this.pictureBox_SideHints.Name = "pictureBox_SideHints";
            this.pictureBox_SideHints.Size = new System.Drawing.Size(100, 50);
            this.pictureBox_SideHints.TabIndex = 2;
            this.pictureBox_SideHints.TabStop = false;
            // 
            // MiniGame_Nono_Stage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1687, 841);
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
    }
}