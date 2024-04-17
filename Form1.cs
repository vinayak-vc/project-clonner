using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectCloner {

    internal enum LogType {
        Error,
        Log,
        Warning
    }

    public partial class Form1 : Form {
        private readonly Regex UrlMatchOnlyHttps = new Regex(@"(?i)(http(s)?:\/\/)(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
        private readonly promtdialog_Form promtdialog_Form;
        private readonly ErrorForm errorForm;
        private Process process;

        public Form1() {
            InitializeComponent();
            promtdialog_Form = new promtdialog_Form();
            errorForm = new ErrorForm();
        }

        private void HandleError(string errorMessage) {
            error_label.Text = errorMessage;
        }

        private void ExecuteCommand(string path, string gitURL, string projectDirectory, string driveName, string folderName, string baseProjectBranchName, string projectBranchName, string projectName) {
            try {
                // Check if the script file exists
                if (!File.Exists(path))
                    HandleError("Batch file does not exist.");

                // Check if the Git URL is empty
                if (string.IsNullOrWhiteSpace(gitURL))
                    HandleError("Git URL should not be empty.");

                // Check if the Git URL is valid
                if (!UrlMatchOnlyHttps.IsMatch(gitURL))
                    HandleError("Invalid Git URL.");

                // Check if the project directory is empty or does not exist
                if (string.IsNullOrWhiteSpace(projectDirectory) || !Directory.Exists(projectDirectory))
                    HandleError("Project directory does not exist.");

                // Check if the project directory is valid
                string folderPath = Path.Combine(projectDirectory, folderName);
                if (Directory.Exists(folderPath))
                    HandleError("Directory already exists.");

                // Construct the process start info
                ProcessStartInfo psi = new(path, $"\"{gitURL}\" \"{projectDirectory}\" {driveName} {folderName} {baseProjectBranchName} {projectBranchName} {projectName}") {
                    CreateNoWindow = false,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    ErrorDialog = true
                };

                Debug.Print("[ShellScript] Attempting to execute: \ncmd({0})\nargs({1})\n", path, psi.Arguments);

                // Start the process
                Process process = new() {
                    StartInfo = psi
                };
                process.Start();

                // Read output and error streams
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0) {
                    // Process completed successfully
                    if (!string.IsNullOrWhiteSpace(error)) {
                        Debug.Print("[ShellScript] Success on Run({path}) - but had some errors (below)");
                        SplitLog(LogType.Log, $"vvvv StdErr vvvv: {path}\n", "\n^^^^ StdErr ^^^^", error);
                    }
                    UnityOprations.StartUnityOperations(baseProjectBranchName, Path.Combine(projectDirectory, folderName));
                    return;
                }

                // Process failed
                if (!string.IsNullOrWhiteSpace(error)) {
                    Debug.Print($"[ShellScript] Failure on Run({path}, exit={process.ExitCode}) - had some errors (below)");
                    SplitLog(LogType.Error, $"vvvv StdErr vvvv: {path}\n", "\n^^^^ StdErr ^^^^", error);
                }

                Debug.Print($"[ShellScript] Failure on Run({path}, exit={process.ExitCode}) - script output below (script stderr above)");
                SplitLog(LogType.Error, $"vvvv StdOut vvvv: {path}\n", "\n^^^^ StdOut ^^^^", output);
                Debug.WriteLine(output);
                HandleError("Error occurred during script execution.");
            } catch (Exception ex) {
                // Catch any unexpected exceptions
                HandleError($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void SplitLog(LogType type, string prefix, string postfix, string fullLog, int lineCap = 1024) {
            if (string.IsNullOrWhiteSpace(fullLog))
                return;

            string[] lines = fullLog.Split(separator: new char[] { '\n' });
            if (lines.Length < lineCap) {
                Debug.Print($"{type}: {prefix}{fullLog}{postfix}");
                return;
            }

            StringBuilder s = new();
            s.Append(prefix);
            for (int i = 0; i < lines.Length / lineCap; ++i) {
                for (int j = 0; j < lineCap; j++) {
                    string line = lines[i * lineCap + j];
                    if (!string.IsNullOrWhiteSpace(line))
                        s.AppendLine(line);
                }

                string combinedOut = s.ToString();
                if (!string.IsNullOrWhiteSpace(combinedOut))
                    Debug.Print($"{type}: {combinedOut}");

                s = new StringBuilder();
            }

            Debug.Print($"{type}: {postfix}");
        }

        private void Process_Exited(object? sender, EventArgs e) {
            process.Exited -= Process_Exited;
            Debug.Print(process.ExitCode + "");
            promtdialog_Form.Show();
        }

        private void gitClone_button_Click(object sender, EventArgs e) {
            Console.WriteLine(Application.ExecutablePath);
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BaseProjectClonner.bat");
            Debug.Print(path);

            if (!File.Exists(path))
                HandleError("Batch file does not exist.");

            if (gitURL_textBox.Text == "" || string.IsNullOrEmpty(gitURL_textBox.Text))
                HandleError("Git URL should not be empty.");

            if (!UrlMatchOnlyHttps.IsMatch(gitURL_textBox.Text))
                HandleError("Invalid Git URL.");

            if (project_direcotorytextBox.Text == "" || string.IsNullOrEmpty(project_direcotorytextBox.Text))
                HandleError("Project directory should not be empty.");

            if (!Directory.Exists(project_direcotorytextBox.Text))
                HandleError("Project directory does not exist.");

            string folderName = Path.GetFileNameWithoutExtension(gitURL_textBox.Text) + "-base-project";
            string folderpath = Path.Combine(project_direcotorytextBox.Text, folderName);
            if (Directory.Exists(folderpath))
                HandleError("Directory already exists.");

            string drivename = project_direcotorytextBox.Text[..2];
            Debug.WriteLine(folderName);

            ExecuteCommand(path, gitURL_textBox.Text, project_direcotorytextBox.Text, drivename, folderName, baseprojectbrachTextbox.Text, projectBranchTextbox.Text, Path.GetFileNameWithoutExtension(gitURL_textBox.Text));
        }

        private void button1_Click(object sender, EventArgs e) {
            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
                project_direcotorytextBox.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}