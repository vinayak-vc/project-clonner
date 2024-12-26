using System.Diagnostics;
using System.Security;
using System.Windows.Forms;

namespace ProjectCloner {

    internal class UnityOprations {
        private static Form1 form1;

        internal void Init(Form1 form) {
            form1 = form;
        }

        public void StartUnityOperations(string unityVersion, string projectpath) {
            try {
                string unityPath = FindUnityExecutable(unityVersion); // Change this to your desired version
                if (unityPath != null) {
                    StartUnityProcess(unityPath, projectpath);
                } else {
                    MessageBox.Show($"Unity version {unityVersion} is not installed on this machine.");
                }
            } catch (UnauthorizedAccessException) {
                MessageBox.Show("You don't have permission to run Unity. Please run the application as an administrator.");
                form1.HandleError("You don't have permission to run Unity. Please run the application as an administrator.");
            } catch (SecurityException) {
                MessageBox.Show("Security error occurred while trying to run Unity. Please check your security settings.");
                form1.HandleError("Security error occurred while trying to run Unity. Please check your security settings.");
            } catch (FileNotFoundException) {
                MessageBox.Show("Unity executable not found.");
                form1.HandleError("Unity executable not found.");
            } catch (Exception ex) {
                MessageBox.Show($"Error: {ex.Message}");
                form1.HandleError($"Error: {ex.Message}");
            }
        }

        public static string? FindUnityExecutable(string version) {
            //version = "2022.3.11f";
            string[] unityInstallationPaths = {
                $@"C:\Program Files\Unity\Hub\Editor\{version}1\Editor\",
                $@"C:\Program Files (x86)\Unity\Hub\Editor\{version}1\Editor\"
            };

            string[] unityExecutableNames = {
                "Unity.exe",
                "Unity"
            };

            foreach (string path in unityInstallationPaths) {
                foreach (string executable in unityExecutableNames) {
                    string fullPath = Path.Combine(path, executable);
                    if (File.Exists(fullPath)) {
                        form1.HandleError($"Unity Version Found at {fullPath}", false, true);
                        return fullPath;
                    }
                }
            }

            return null;
        }

        private void StartUnityProcess(string unityPath, string projectPath) {
            Debug.Print("unityPath: " + unityPath);
            Debug.Print("projectPath: " + projectPath);
            string unityArgs = $" -projectPath {projectPath} -executeMethod ViitorCloud.Base.BaseScripts.PluginObjects.Editor.CheckforPlugin.DownloadThePlugin"; // Change this to your method

            ProcessStartInfo startInfo = new ProcessStartInfo {
                FileName = unityPath,
                Arguments = unityArgs,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process unityProcess = new Process {
                StartInfo = startInfo
            };

            unityProcess.EnableRaisingEvents = true;
            unityProcess.Exited += UnityProcess_Exited;

            try {
                form1.HandleError("Unity Process Starting", false, true);
                unityProcess.Start();
            } catch (UnauthorizedAccessException e) {
                form1.HandleError("Error occurred : " + e);
                throw; // Re-throw the exception to be caught in the button click handler
            } catch (SecurityException e) {
                form1.HandleError("Error occurred : " + e);
                throw; // Re-throw the exception to be caught in the button click handler
            } catch (Exception ex) {
                form1.HandleError("Error occurred : " + ex);
                throw new Exception($"Error starting Unity process: {ex.Message}");
            }
        }

        private void UnityProcess_Exited(object? sender, EventArgs e) {
            Process unityProcess = (Process)sender;

            // Check if Unity process exited successfully
            if (unityProcess.ExitCode == 0) {
                MessageBox.Show("Unity operation completed successfully!");
                form1.HandleError("Unity operation completed successfully!", true, true);
            } else {
                // Check if Unity process crashed unexpectedly
                if (!unityProcess.HasExited) {
                    MessageBox.Show("Unity process crashed unexpectedly!");
                    form1.HandleError("Unity process crashed unexpectedly!");
                } else {
                    MessageBox.Show("Unity operation failed! : " + unityProcess.ExitCode);
                    form1.HandleError("Unity operation failed! : " + unityProcess.ExitCode);
                }
            }
        }
    }
}