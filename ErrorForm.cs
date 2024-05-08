namespace ProjectCloner {

    public partial class ErrorForm : Form {
        private string logPath;

        public ErrorForm() {
            InitializeComponent();
            logPath = Path.Combine(Application.UserAppDataPath, $"Log_{DateTime.Now.ToString("MM_dd_HH_mm")}.txt");
            ErrorText(
                   $"************************************************* This is New Run {DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")}*************************************************");

            ErrorText("Logfile Path = " + new Uri(logPath));
            fontNumericUpDown.Value = (decimal)ErrorLable.SelectionFont.Size;

            this.Text = "Log";
        }

        public void ErrorText(string text) {
            ErrorLable.AppendText(Environment.NewLine + DateTime.Now.ToString("HH:mm:ss") + " - " + text); // Append log entry to RichTextBox
            ErrorLable.SelectionStart = ErrorLable.TextLength;
            ErrorLable.ScrollToCaret();

            File.AppendAllText(logPath, text);
        }

        private void ErrorForm_Resize(object sender, EventArgs e) {
            AdjustRichTextBoxSize();
        }

        private void AdjustRichTextBoxSize() {
            ErrorLable.Width = ClientSize.Width - 40; // Adjust width
            ErrorLable.Height = ClientSize.Height - 80; // Adjust height
        }

        private void ErrorForm_Load(object sender, EventArgs e) {
            AdjustRichTextBoxSize();
        }

        private void fontNumericUpDown_ValueChanged(object sender, EventArgs e) {
            ErrorLable.Font = new Font(ErrorLable.Font.FontFamily, (int)fontNumericUpDown.Value);
        }
    }
}