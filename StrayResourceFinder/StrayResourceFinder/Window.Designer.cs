namespace StrayResourceFinder
{
    partial class Window
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
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxAllRooms = new System.Windows.Forms.CheckBox();
            this.button_open = new System.Windows.Forms.Button();
            this.button_refresh = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // openFile
            // 
            this.openFile.DefaultExt = "gmx";
            this.openFile.FileName = "Open File";
            // 
            // checkBoxAllRooms
            // 
            this.checkBoxAllRooms.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAllRooms.AutoSize = true;
            this.checkBoxAllRooms.Location = new System.Drawing.Point(546, 12);
            this.checkBoxAllRooms.Name = "checkBoxAllRooms";
            this.checkBoxAllRooms.Size = new System.Drawing.Size(110, 17);
            this.checkBoxAllRooms.TabIndex = 4;
            this.checkBoxAllRooms.Text = "Search All Rooms";
            this.checkBoxAllRooms.UseVisualStyleBackColor = true;
            // 
            // button_open
            // 
            this.button_open.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_open.Location = new System.Drawing.Point(12, 6);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(427, 23);
            this.button_open.TabIndex = 5;
            this.button_open.Text = "Open Project";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // button_refresh
            // 
            this.button_refresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_refresh.Location = new System.Drawing.Point(451, 6);
            this.button_refresh.Name = "button_refresh";
            this.button_refresh.Size = new System.Drawing.Size(75, 23);
            this.button_refresh.TabIndex = 6;
            this.button_refresh.Text = "Refresh";
            this.button_refresh.UseVisualStyleBackColor = true;
            this.button_refresh.Click += new System.EventHandler(this.button_refresh_Click);
            // 
            // treeView
            // 
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.Location = new System.Drawing.Point(12, 36);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(644, 448);
            this.treeView.TabIndex = 7;
            // 
            // Window
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 496);
            this.Controls.Add(this.treeView);
            this.Controls.Add(this.button_refresh);
            this.Controls.Add(this.button_open);
            this.Controls.Add(this.checkBoxAllRooms);
            this.Name = "Window";
            this.Text = "Stray Resource Finder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.CheckBox checkBoxAllRooms;
        private System.Windows.Forms.Button button_open;
        private System.Windows.Forms.Button button_refresh;
        private System.Windows.Forms.TreeView treeView;
    }
}

