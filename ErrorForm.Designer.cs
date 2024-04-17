namespace ProjectCloner {
    partial class ErrorForm {
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
            ErrorLable = new Label();
            Close_button = new Button();
            SuspendLayout();
            // 
            // ErrorLable
            // 
            ErrorLable.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ErrorLable.AutoSize = true;
            ErrorLable.Location = new Point(343, 78);
            ErrorLable.Name = "ErrorLable";
            ErrorLable.Size = new Size(38, 15);
            ErrorLable.TabIndex = 0;
            ErrorLable.Text = "label1";
            // 
            // Close_button
            // 
            Close_button.Location = new Point(343, 362);
            Close_button.Name = "Close_button";
            Close_button.Size = new Size(75, 23);
            Close_button.TabIndex = 1;
            Close_button.Text = "Close";
            Close_button.UseVisualStyleBackColor = true;
            Close_button.Click += Close_button_Click;
            // 
            // ErrorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            ControlBox = false;
            Controls.Add(Close_button);
            Controls.Add(ErrorLable);
            Name = "ErrorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Error";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label ErrorLable;
        private Button Close_button;
    }
}