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
        private promtdialog_Form promtdialog_Form;
        private ErrorForm errorForm;
        private Process process;
        private UnityOprations unityOprations;

        public Form1() {
            Loom.Init();
            InitializeComponent();
            error_label.Text = "";
            promtdialog_Form = new promtdialog_Form();
            errorForm = new ErrorForm();
            unityOprations = new UnityOprations();
            unityOprations.Init(this);
            errorForm.FormClosing += ErrorForm_FormClosed;
        }

        private void ErrorForm_FormClosed(object? sender, FormClosingEventArgs e) {
            errorForm = new ErrorForm();
            openLogButton.Enabled = true;
        }

        private void SmallErrosShow(string error) {
            error_label.Text = error;
        }

        public void HandleError(string errorMessage, bool isSmallError = false, bool isLog = false) {
            //  Loom.QueueOnMainThread(() => {
            errorForm.ErrorText(errorMessage);
            if (!isLog) {
                if (isSmallError) {
                    SmallErrosShow(errorMessage);
                } else {
                    SmallErrosShow("Check Logs for More Details.");
                }
            }
            //});
        }

        private Task ExecuteCommand(string path, string gitURL, string projectDirectory, string driveName, string folderName, string baseProjectBranchName, string projectBranchName, string projectName, string unityPath) {
            return Task.Run(() => {
                try {
                    // Check if the script file exists
                    if (!File.Exists(path)) {
                        HandleError("Batch file does not exist.", true);
                        return;
                    }

                    // Check if the Git URL is empty
                    if (string.IsNullOrWhiteSpace(gitURL)) {
                        HandleError("Git URL should not be empty.", true);
                        return;
                    }

                    // Check if the Git URL is valid
                    if (!UrlMatchOnlyHttps.IsMatch(gitURL)) {
                        HandleError("Invalid Git URL.");
                        return;
                    }
                    // Check if the project directory is empty or does not exist
                    if (string.IsNullOrWhiteSpace(projectDirectory) || !Directory.Exists(projectDirectory)) {
                        HandleError("Project directory does not exist.", true);
                        return;
                    }
                    // Check if the project directory is valid
                    string folderPath = Path.Combine(projectDirectory, folderName);
                    if (Directory.Exists(folderPath)) {
                        HandleError("Directory already exists.", true);
                        //return;
                    }
                    // Construct the process start info
                    ProcessStartInfo psi = new(path,
                        $"{gitURL} " +
                        //$"https://github.com/viitoradmin/unityvc-base-project.git " +
                        $"{projectDirectory}  " +
                        $"{driveName} " +
                        $"{folderName} " +
                        $"{baseProjectBranchName} " +
                        $"{projectBranchName} " +
                        $"{projectName} " +
                        $"\"{unityPath}\"  " +
                        $"{folderPath} ") {
                        CreateNoWindow = false,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        ErrorDialog = true
                    };
                    if (!File.Exists(Path.Combine(projectDirectory, "output.txt"))) {
                        new FileStream(Path.Combine(projectDirectory, "output.txt"), FileMode.CreateNew).Dispose();
                    }

                    HandleError(string.Format("[ShellScript] Attempting to execute: \ncmd({0})\nargs({1})\n", path, psi.Arguments));

                    // Start the process
                    Process process = new() {
                        StartInfo = psi
                    };

                    // Event handlers for capturing output
                    process.OutputDataReceived += (sender, e) => OutputToTextBox(e.Data);
                    process.ErrorDataReceived += (sender, e) => OutputToTextBox(e.Data);

                    process.Start();

                    // Begin asynchronously reading the output
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    process.WaitForExit();

                    if (process.ExitCode == 0) {
                        HandleError("Clone Successfull. Now Starting Unity", false, true);
                        unityOprations.StartUnityOperations(unity_versionText.Text, Path.Combine(projectDirectory, folderName));
                        return;
                    }
                } catch (Exception ex) {
                    // Catch any unexpected exceptions
                    HandleError($"An unexpected error occurred: {ex.Message}");
                }
            }).ContinueWith(task => {
                Loom.QueueOnMainThread(delegate {
                    if (task.IsFaulted) {
                        HandleError("Error Occurd :" + task.Exception);
                    } else {
                        gitClone_button.Enabled = true;
                    }
                });
            });
        }

        public void OutputToTextBox(string text) {
            if (!string.IsNullOrEmpty(text)) {
                HandleError(text);
            }
        }

        private void SplitLog(LogType type, string prefix, string postfix, string fullLog, int lineCap = 1024) {
            if (string.IsNullOrWhiteSpace(fullLog))
                return;

            string[] lines = fullLog.Split(separator: new char[] { '\n' });
            if (lines.Length < lineCap) {
                HandleError($"{type}: {prefix}{fullLog}{postfix}");
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
                    HandleError($"{type}: {combinedOut}");

                s = new StringBuilder();
            }

            HandleError($"{type}: {postfix}");
        }

        private void Process_Exited(object? sender, EventArgs e) {
            process.Exited -= Process_Exited;
            HandleError(process.ExitCode + "");
            promtdialog_Form.Show();
        }

        private async void gitClone_button_Click(object sender, EventArgs e) {
            Console.WriteLine(Application.ExecutablePath);
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BaseProjectClonner.bat");
            HandleError(path);

            if (!File.Exists(path)) {
                HandleError("Batch file does not exist.", true);
                return;
            }

            if (gitURL_textBox.Text == "" || string.IsNullOrEmpty(gitURL_textBox.Text)) {
                HandleError("Git URL should not be empty.", true);
                return;
            }

            if (!UrlMatchOnlyHttps.IsMatch(gitURL_textBox.Text)) {
                HandleError("Invalid Git URL.", true);
                return;
            }

            if (project_direcotorytextBox.Text == "" || string.IsNullOrEmpty(project_direcotorytextBox.Text)) {
                HandleError("Project directory should not be empty.", true);
                return;
            }

            if (!Directory.Exists(project_direcotorytextBox.Text)) {
                HandleError("Project directory does not exist.", true);
                return;
            }

            string folderName = Path.GetFileNameWithoutExtension(gitURL_textBox.Text) + "-base-project";
            string folderpath = Path.Combine(project_direcotorytextBox.Text, folderName);
            if (Directory.Exists(folderpath)) {
                HandleError("Directory already exists.");
                return;
            }
            string unityPath = UnityOprations.FindUnityExecutable(unity_versionText.Text);
            if (unityPath == null) {
                HandleError($"Unity version {unity_versionText.Text} not found. Install it first then Try again.");
            }

            string drivename = project_direcotorytextBox.Text[..2];
            Debug.WriteLine(folderName);

            gitClone_button.Enabled = false;

            await ExecuteCommand(path, gitURL_textBox.Text, project_direcotorytextBox.Text, drivename, folderName, baseprojectbrachTextbox.Text, projectBranchTextbox.Text, Path.GetFileNameWithoutExtension(gitURL_textBox.Text), unityPath);
        }

        private void button1_Click(object sender, EventArgs e) {
            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK)
                project_direcotorytextBox.Text = folderBrowserDialog1.SelectedPath;
        }

        private void openLogButton_Click(object sender, EventArgs e) {
            openLogButton.Enabled = false;
            try {
                if (errorForm == null) {
                    errorForm = new ErrorForm();
                }
                errorForm.Show();
            } catch {
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
        }
    }
}