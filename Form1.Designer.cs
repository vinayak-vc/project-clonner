namespace ProjectCloner {
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
            gitURL_textBox = new TextBox();
            labelgitURL = new Label();
            gitClone_button = new Button();
            project_direcotorytextBox = new TextBox();
            label1 = new Label();
            folderBrowserDialog1 = new FolderBrowserDialog();
            button1 = new Button();
            error_label = new Label();
            projectBranchTextbox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            baseprojectbrachTextbox = new TextBox();
            openLogButton = new Button();
            SuspendLayout();
            // 
            // gitURL_textBox
            // 
            gitURL_textBox.Location = new Point(124, 21);
            gitURL_textBox.Name = "gitURL_textBox";
            gitURL_textBox.Size = new Size(301, 23);
            gitURL_textBox.TabIndex = 0;
            gitURL_textBox.Text = "https://github.com/viitoradmin/hologram-stage-viewer-application.git";
            // 
            // labelgitURL
            // 
            labelgitURL.AutoSize = true;
            labelgitURL.Location = new Point(12, 21);
            labelgitURL.Name = "labelgitURL";
            labelgitURL.Size = new Size(43, 15);
            labelgitURL.TabIndex = 1;
            labelgitURL.Text = "GitURL";
            // 
            // gitClone_button
            // 
            gitClone_button.Location = new Point(301, 155);
            gitClone_button.Name = "gitClone_button";
            gitClone_button.Size = new Size(124, 40);
            gitClone_button.TabIndex = 2;
            gitClone_button.Text = "Clone";
            gitClone_button.UseVisualStyleBackColor = true;
            gitClone_button.Click += gitClone_button_Click;
            // 
            // project_direcotorytextBox
            // 
            project_direcotorytextBox.Location = new Point(124, 64);
            project_direcotorytextBox.Name = "project_direcotorytextBox";
            project_direcotorytextBox.Size = new Size(240, 23);
            project_direcotorytextBox.TabIndex = 3;
            project_direcotorytextBox.Text = "D:\\Vinayak_ViitorCloud\\Temp";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 64);
            label1.Name = "label1";
            label1.Size = new Size(95, 15);
            label1.TabIndex = 4;
            label1.Text = "Project Directory";
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyDocuments;
            // 
            // button1
            // 
            button1.Location = new Point(370, 61);
            button1.Name = "button1";
            button1.Size = new Size(55, 27);
            button1.TabIndex = 5;
            button1.Text = "Browse";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // error_label
            // 
            error_label.AutoSize = true;
            error_label.ForeColor = Color.Red;
            error_label.Location = new Point(12, 137);
            error_label.Name = "error_label";
            error_label.Size = new Size(43, 15);
            error_label.TabIndex = 6;
            error_label.Text = "asdasd";
            // 
            // projectBranchTextbox
            // 
            projectBranchTextbox.Location = new Point(124, 107);
            projectBranchTextbox.Name = "projectBranchTextbox";
            projectBranchTextbox.Size = new Size(87, 23);
            projectBranchTextbox.TabIndex = 7;
            projectBranchTextbox.Text = "developement";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 110);
            label2.Name = "label2";
            label2.Size = new Size(84, 15);
            label2.TabIndex = 8;
            label2.Text = "Project Branch";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(224, 110);
            label3.Name = "label3";
            label3.Size = new Size(108, 15);
            label3.TabIndex = 10;
            label3.Text = "BaseProject Branch";
            // 
            // baseprojectbrachTextbox
            // 
            baseprojectbrachTextbox.Location = new Point(338, 107);
            baseprojectbrachTextbox.Name = "baseprojectbrachTextbox";
            baseprojectbrachTextbox.Size = new Size(87, 23);
            baseprojectbrachTextbox.TabIndex = 9;
            baseprojectbrachTextbox.Text = "2022.3.11f";
            // 
            // openLogButton
            // 
            openLogButton.Location = new Point(12, 155);
            openLogButton.Name = "openLogButton";
            openLogButton.Size = new Size(124, 40);
            openLogButton.TabIndex = 11;
            openLogButton.Text = "Open Log";
            openLogButton.UseVisualStyleBackColor = true;
            openLogButton.Click += openLogButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(446, 201);
            Controls.Add(openLogButton);
            Controls.Add(label3);
            Controls.Add(baseprojectbrachTextbox);
            Controls.Add(label2);
            Controls.Add(projectBranchTextbox);
            Controls.Add(error_label);
            Controls.Add(button1);
            Controls.Add(label1);
            Controls.Add(project_direcotorytextBox);
            Controls.Add(gitClone_button);
            Controls.Add(labelgitURL);
            Controls.Add(gitURL_textBox);
            FormBorderStyle = FormBorderStyle.SizableToolWindow;
            Icon = (Icon)resources.GetObject("$this.Icon");
            ImeMode = ImeMode.Disable;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Git Clonner";
            TopMost = true;
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
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
        private TextBox projectBranchTextbox;
        private Label label2;
        private Label label3;
        private TextBox baseprojectbrachTextbox;
        private Button openLogButton;
    }
}