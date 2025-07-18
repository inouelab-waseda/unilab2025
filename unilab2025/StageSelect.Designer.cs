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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StageSelect));
            this.button_ToMap = new System.Windows.Forms.Button();
            this.button_Stage3 = new unilab2025.CustomButton();
            this.button_Stage2 = new unilab2025.CustomButton();
            this.button_Stage1 = new unilab2025.CustomButton();
            this.SuspendLayout();
            // 
            // button_ToMap
            // 
            this.button_ToMap.BackColor = System.Drawing.Color.Transparent;
            this.button_ToMap.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_ToMap.BackgroundImage")));
            this.button_ToMap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_ToMap.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_ToMap.Location = new System.Drawing.Point(15, 14);
            this.button_ToMap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_ToMap.Name = "button_ToMap";
            this.button_ToMap.Size = new System.Drawing.Size(424, 85);
            this.button_ToMap.TabIndex = 1;
            this.button_ToMap.UseVisualStyleBackColor = false;
            this.button_ToMap.Click += new System.EventHandler(this.button_ToMap_Click);
            // 
            // button_Stage3
            // 
            this.button_Stage3.BackColor = System.Drawing.Color.Transparent;
            this.button_Stage3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Stage3.ConditionImage = null;
            this.button_Stage3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Stage3.ForeImage = null;
            this.button_Stage3.Location = new System.Drawing.Point(1180, 565);
            this.button_Stage3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Stage3.Name = "button_Stage3";
            this.button_Stage3.Size = new System.Drawing.Size(469, 128);
            this.button_Stage3.TabIndex = 4;
            this.button_Stage3.UseVisualStyleBackColor = false;
            this.button_Stage3.Click += new System.EventHandler(this.button_StageI_Click);
            // 
            // button_Stage2
            // 
            this.button_Stage2.BackColor = System.Drawing.Color.Transparent;
            this.button_Stage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Stage2.ConditionImage = null;
            this.button_Stage2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Stage2.ForeImage = null;
            this.button_Stage2.Location = new System.Drawing.Point(657, 565);
            this.button_Stage2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Stage2.Name = "button_Stage2";
            this.button_Stage2.Size = new System.Drawing.Size(469, 128);
            this.button_Stage2.TabIndex = 3;
            this.button_Stage2.UseVisualStyleBackColor = false;
            this.button_Stage2.Click += new System.EventHandler(this.button_StageI_Click);
            // 
            // button_Stage1
            // 
            this.button_Stage1.BackColor = System.Drawing.Color.Transparent;
            this.button_Stage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_Stage1.ConditionImage = null;
            this.button_Stage1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_Stage1.ForeImage = null;
            this.button_Stage1.Location = new System.Drawing.Point(117, 565);
            this.button_Stage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button_Stage1.Name = "button_Stage1";
            this.button_Stage1.Size = new System.Drawing.Size(469, 128);
            this.button_Stage1.TabIndex = 2;
            this.button_Stage1.UseVisualStyleBackColor = false;
            this.button_Stage1.Click += new System.EventHandler(this.button_StageI_Click);
            // 
            // StageSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1712, 911);
            this.Controls.Add(this.button_Stage3);
            this.Controls.Add(this.button_Stage2);
            this.Controls.Add(this.button_Stage1);
            this.Controls.Add(this.button_ToMap);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "StageSelect";
            this.Text = "StageSelect";
            this.Load += new System.EventHandler(this.StageSelect_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button_ToMap;
        private CustomButton button_Stage1;
        private CustomButton button_Stage2;
        private CustomButton button_Stage3;
    }
}