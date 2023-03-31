using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectClonner {
    public partial class Form1 : Form {

        Regex UrlMatchOnlyHttps = new Regex(@"(?i)(http(s)?:\/\/)(\w{2,25}\.)+\w{3}([a-z0-9\-?=$-_.+!*()]+)(?i)", RegexOptions.Singleline);
        promtdialog_Form promtdialog_Form = new promtdialog_Form();

        public Form1() {
            InitializeComponent();
        }


        void ExecuteCommand(string path, string gitURL, string projectdirecotry, string drivename, string folderName) {
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo(path, "\"" + gitURL + "\" \"" + projectdirecotry + "\" " + drivename + " " + folderName) {
                CreateNoWindow = false,
                UseShellExecute = false,
            };

            process = Process.Start(processInfo);
            process.WaitForExit();

            process.Close();
            promtdialog_Form.Show();
        }

        private void gitClone_button_Click(object sender, EventArgs e) {

            error_label.Text = "";
            Console.WriteLine(Application.ExecutablePath);
            string path = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "BaseProjectClonner.bat");

            if (!File.Exists(path)) {
                error_label.Text = ("bat file does not exist");
                return;
            }

            if (gitURL_textBox.Text == "" || string.IsNullOrEmpty(gitURL_textBox.Text)) {
                error_label.Text = ("url should not be empty");
                return;
            }

            if (!UrlMatchOnlyHttps.IsMatch(gitURL_textBox.Text)) {
                error_label.Text = ("url is not valid");
                return;
            }

            if (project_direcotorytextBox.Text == "" || string.IsNullOrEmpty(project_direcotorytextBox.Text)) {
                error_label.Text = ("project direcotory should not be empty");
                return;
            }

            if (!Directory.Exists(project_direcotorytextBox.Text)) {
                error_label.Text = ("directory should be real");
                return;
            }


            string folderName = Path.GetFileNameWithoutExtension(gitURL_textBox.Text) + "-base-project";
            string folderpath = Path.Combine(project_direcotorytextBox.Text, folderName);
            if (Directory.Exists(folderpath)) {
                error_label.Text = ("already directory exist");
                return;
            }

            error_label.Text = "";
            string drivename = project_direcotorytextBox.Text.Substring(0, 2);
            Debug.WriteLine(folderName);

            ExecuteCommand(path, gitURL_textBox.Text, project_direcotorytextBox.Text, drivename, folderName);
        }

        private void button1_Click(object sender, EventArgs e) {
            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();
            if (dialogResult == DialogResult.OK) {
                project_direcotorytextBox.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
