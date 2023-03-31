namespace ProjectClonner {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gitURL_textBox = new System.Windows.Forms.TextBox();
            this.labelgitURL = new System.Windows.Forms.Label();
            this.gitClone_button = new System.Windows.Forms.Button();
            this.project_direcotorytextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.error_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gitURL_textBox
            // 
            this.gitURL_textBox.Location = new System.Drawing.Point(124, 43);
            this.gitURL_textBox.Name = "gitURL_textBox";
            this.gitURL_textBox.Size = new System.Drawing.Size(301, 23);
            this.gitURL_textBox.TabIndex = 0;
            // 
            // labelgitURL
            // 
            this.labelgitURL.AutoSize = true;
            this.labelgitURL.Location = new System.Drawing.Point(12, 43);
            this.labelgitURL.Name = "labelgitURL";
            this.labelgitURL.Size = new System.Drawing.Size(43, 15);
            this.labelgitURL.TabIndex = 1;
            this.labelgitURL.Text = "GitURL";
            // 
            // gitClone_button
            // 
            this.gitClone_button.Location = new System.Drawing.Point(301, 137);
            this.gitClone_button.Name = "gitClone_button";
            this.gitClone_button.Size = new System.Drawing.Size(124, 40);
            this.gitClone_button.TabIndex = 2;
            this.gitClone_button.Text = "Clone";
            this.gitClone_button.UseVisualStyleBackColor = true;
            this.gitClone_button.Click += new System.EventHandler(this.gitClone_button_Click);
            // 
            // project_direcotorytextBox
            // 
            this.project_direcotorytextBox.Location = new System.Drawing.Point(124, 86);
            this.project_direcotorytextBox.Name = "project_direcotorytextBox";
            this.project_direcotorytextBox.Size = new System.Drawing.Size(240, 23);
            this.project_direcotorytextBox.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Project Directory";
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyDocuments;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(370, 83);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(55, 27);
            this.button1.TabIndex = 5;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // error_label
            // 
            this.error_label.AutoSize = true;
            this.error_label.ForeColor = System.Drawing.Color.Red;
            this.error_label.Location = new System.Drawing.Point(12, 137);
            this.error_label.Name = "error_label";
            this.error_label.Size = new System.Drawing.Size(0, 15);
            this.error_label.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(446, 201);
            this.Controls.Add(this.error_label);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.project_direcotorytextBox);
            this.Controls.Add(this.gitClone_button);
            this.Controls.Add(this.labelgitURL);
            this.Controls.Add(this.gitURL_textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Git Clonner";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox gitURL_textBox;
        private Label labelgitURL;
        private Button gitClone_button;
        private TextBox project_direcotorytextBox;
        private Label label1;
        private FolderBrowserDialog folderBrowserDialog1;
        private Button button1;
        private Label error_label;
    }
}