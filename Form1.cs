using Microsoft.Win32;

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

using ViitorCloud.ProjectCloner;

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
            GitMechanism.Initialize(this);
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

        private Task RunCommand(string gitURL, string projectDirectory, string folderName, string baseProjectBranchName, string projectBranchName, string projectName) {
            return Task.Run(() => {
                string folderPath = Path.Combine(projectDirectory, folderName);

                new DirectoryInfo(folderPath).Create();

                GitMechanism.PerformClone(@"https://github.com/viitoradmin/unityvc-base-project.git", folderPath, (success, errorMessage) => {
                    if (success) {
                        GitMechanism.PerformFetch(folderPath, (success, errorMessage) => {
                            GitMechanism.PerformCheckout(folderPath, baseProjectBranchName, (success, errorMessage) => {
                                if (success) {
                                    string unityvcBaseprojectDirector = Path.Combine(folderPath, "Assets", "Games", projectName);
                                    new DirectoryInfo(unityvcBaseprojectDirector).Create();
                                    GitMechanism.PerformClone(gitURL, unityvcBaseprojectDirector, (success, errorMessage) => {
                                        if (success) {
                                            GitMechanism.PerformFetch(unityvcBaseprojectDirector, (success, errorMessage) => {
                                                GitMechanism.PerformCheckout(unityvcBaseprojectDirector, projectBranchName, (success, errorMessage) => {
                                                    unityOprations.StartUnityOperations(unity_versionText.Text, Path.Combine(projectDirectory, folderName));
                                                });
                                            });
                                        }
                                    });
                                }
                            });
                        });
                    }
                });
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

        private async void gitClone_button_Click(object sender, EventArgs e) {
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

            if (token_textBox.Text != "" && !string.IsNullOrEmpty(token_textBox.Text)) {
                GitMechanism.SetCredential(token_textBox.Text, (isValid) => {
                    if (!isValid) {
                        HandleError("Invalid GitHub token provided in environment variables.", true);
                    }
                });
            }

            gitClone_button.Enabled = false;

            await RunCommand(gitURL_textBox.Text, project_direcotorytextBox.Text, folderName, baseprojectbrachTextbox.Text, projectBranchTextbox.Text, Path.GetFileNameWithoutExtension(gitURL_textBox.Text));
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
            RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\VC\ProjectClonner");
            if (key != null) {
                project_direcotorytextBox.Text = key.GetValue("project_direcotorytextBox").ToString();
                token_textBox.Text = key.GetValue("token_textBox").ToString();
                unity_versionText.Text = key.GetValue("unity_versionText").ToString();
                baseprojectbrachTextbox.Text = key.GetValue("baseprojectbrachTextbox").ToString();
                projectBranchTextbox.Text = key.GetValue("projectBranchTextbox").ToString();
                gitURL_textBox.Text = key.GetValue("gitURL_textBox").ToString();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\VC\ProjectClonner");
            key.SetValue("project_direcotorytextBox", project_direcotorytextBox.Text);
            key.SetValue("token_textBox", token_textBox.Text);
            key.SetValue("unity_versionText", unity_versionText.Text);
            key.SetValue("baseprojectbrachTextbox", baseprojectbrachTextbox.Text);
            key.SetValue("projectBranchTextbox", projectBranchTextbox.Text);
            key.SetValue("gitURL_textBox", gitURL_textBox.Text);
            key.Close();
        }

        private void baseprojectbrachTextbox_TextChanged(object sender, EventArgs e) {
        }
    }
}