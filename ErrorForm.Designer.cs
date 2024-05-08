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
            ErrorLable = new RichTextBox();
            fontNumericUpDown = new NumericUpDown();
            fontLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)fontNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // ErrorLable
            // 
            ErrorLable.Location = new Point(12, 50);
            ErrorLable.Name = "ErrorLable";
            ErrorLable.ReadOnly = true;
            ErrorLable.Size = new Size(776, 359);
            ErrorLable.TabIndex = 3;
            ErrorLable.Text = "";
            // 
            // fontNumericUpDown
            // 
            fontNumericUpDown.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            fontNumericUpDown.Location = new Point(745, 21);
            fontNumericUpDown.Name = "fontNumericUpDown";
            fontNumericUpDown.Size = new Size(43, 23);
            fontNumericUpDown.TabIndex = 4;
            fontNumericUpDown.ValueChanged += fontNumericUpDown_ValueChanged;
            // 
            // fontLabel
            // 
            fontLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            fontLabel.AutoSize = true;
            fontLabel.Location = new Point(685, 23);
            fontLabel.Name = "fontLabel";
            fontLabel.Size = new Size(54, 15);
            fontLabel.TabIndex = 5;
            fontLabel.Text = "Font Size";
            // 
            // ErrorForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(fontLabel);
            Controls.Add(fontNumericUpDown);
            Controls.Add(ErrorLable);
            Name = "ErrorForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Error";
            Load += ErrorForm_Load;
            Resize += ErrorForm_Resize;
            ((System.ComponentModel.ISupportInitialize)fontNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private RichTextBox ErrorLable;
        private NumericUpDown fontNumericUpDown;
        private Label fontLabel;
    }
}