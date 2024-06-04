using LibGit2Sharp;
using Octokit;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProjectCloner;
using Repository = LibGit2Sharp.Repository;
using Branch = LibGit2Sharp.Branch;
using Signature = LibGit2Sharp.Signature;
using Credentials = LibGit2Sharp.Credentials;

namespace ViitorCloud.ProjectCloner {

    public class GitMechanism {
        private static Form1 mainForm;

        private static string currentRepositoryPath;
        private static string currentGitRepositoryPath;

        private static Credentials credentials;

        public static void Initialize(Form1 mainFormm) {
            mainForm = mainFormm;
            string token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            if (!string.IsNullOrEmpty(token)) {
                SetCredential(token, (isValid) => {
                    if (!isValid) {
                        mainForm.HandleError("Invalid GitHub token provided in environment variables.");
                    }
                });
            } else {
                Environment.SetEnvironmentVariable("GITHUB_TOKEN", "*******");
                mainForm.HandleError("No GitHub token provided in environment variables.");
            }
        }

        public static void SetCredential(string token, Action<bool> callback) {
            if (IsGitHubTokenValid(token) || true) {
                credentials = new UsernamePasswordCredentials {
                    Username = token,
                    Password = token
                };
                callback(true);
            } else {
                {
                    mainForm.HandleError("Invalid GitHub Personal Access Token. It should be a 40-character hexadecimal string.");
                    callback(false);
                };
            }
        }

        public static void PerformClone(string gitRepoURL, string clonePath, Action<bool, string> callback) {
            try {
                // Clone the repository with force option
                using (var repository = new Repository()) {
                    Repository.Clone(gitRepoURL, clonePath, new CloneOptions {
                        IsBare = false,
                        Checkout = true,
                        OnTransferProgress = OnTransferProgress,
                        CredentialsProvider = (_url, _user, _cred) => credentials
                    });

                    mainForm.HandleError("Repository cloned successfully.");
                }

                //Repository.Clone(gitRepoURL, clonePath, new CloneOptions {
                //    CredentialsProvider = (_url, _user, _cred) => credentials
                //});

                mainForm.HandleError("Clone operation was successful.");
                callback(true, "Clone operation was successful.");
            } catch (Exception ex) {
                mainForm.HandleError($"Clone operation failed: {ex}");
                callback(false, $"Clone operation failed: {ex}");
            }
        }

        private static bool OnTransferProgress(TransferProgress progress) {
            // Handle the transfer progress if needed
            //mainForm.HandleError($"Received: {progress.ReceivedObjects} / {progress.TotalObjects}");
            //mainForm.HandleError($"Indexed: {progress.IndexedObjects} / {progress.TotalObjects}");
            //mainForm.HandleError($"ReceivedBytes: {progress.ReceivedBytes}");

            return true;
        }

        public static void PerformCheckout(string repositoryPath, string branchName, Action<bool, string> callback) {
            currentRepositoryPath = repositoryPath;
            try {
                using (Repository repo = new Repository(repositoryPath)) {
                    Branch branch = repo.Branches["refs/remotes/origin/" + branchName];
                    if (branch == null) {
                        mainForm.HandleError($"'{branchName}' does not exist.");
                        callback(false, $"'{branchName}' does not exist.");
                        return;
                    }

                    repo.Branches.Update(branch, b => {
                        b.Remote = "origin";
                        b.UpstreamBranch = $"refs/heads/{branchName}";
                    });
                    Commands.Checkout(repo, branch);

                    mainForm.HandleError($"Checkout from branch '{branchName}' was successful.");
                    callback(true, $"Checkout from branch '{branchName}' was successful.");
                }
            } catch (Exception ex) {
                mainForm.HandleError($"Pull from branch '{branchName}' failed: {ex.Message}");
                callback(false, $"Checkout from branch '{branchName}' failed: {ex.Message}");
            }
        }

        public static void PerformPull(string repositoryPath, string branchName, Action<bool, string> callback) {
            currentRepositoryPath = repositoryPath;

            try {
                using (Repository repo = new Repository(repositoryPath)) {
                    Branch branch = repo.Branches[branchName];
                    if (branch == null) {
                        mainForm.HandleError($"'{branchName}' does not exist.");
                        callback(false, $"'{branchName}' does not exist.");
                        return;
                    }

                    repo.Branches.Update(branch, b => {
                        b.Remote = "origin";
                        b.UpstreamBranch = $"refs/heads/{branchName}";
                    });
                    Commands.Checkout(repo, branch);

                    Commands.Pull(repo, new Signature("ProjectBuilderBot", "ProjectBuilderBot@viitorcloud.com", DateTimeOffset.Now), new PullOptions {
                        FetchOptions = new FetchOptions {
                            CredentialsProvider = (_url, _user, cred) => credentials
                        },
                    });
                    mainForm.HandleError($"Pull from branch '{branchName}' was successful.");
                    callback(true, $"Pull from branch '{branchName}' was successful.");
                }
            } catch (Exception ex) {
                mainForm.HandleError($"Pull from branch '{branchName}' failed: {ex.Message}");
                callback(false, $"Pull from branch '{branchName}' failed: {ex.Message}");
            }
        }

        public static void PerformFetch(string repositoryPath, Action<bool, string> callback) {
            currentGitRepositoryPath = "";
            try {
                using (Repository repo = new Repository(repositoryPath)) {
                    Remote remote = repo.Network.Remotes["origin"];
                    currentGitRepositoryPath = remote.Url;

                    FetchOptions fetchOptions = new FetchOptions {
                        CredentialsProvider = (_url, _user, _cred) => credentials
                    };

                    Commands.Fetch(repo, remote.Name, new string[] { }, fetchOptions, null);
                    mainForm.HandleError($"Fetch operation was successful. {currentGitRepositoryPath}");
                    callback(true, $"Fetch operation was successful. {currentGitRepositoryPath}");
                }
            } catch (Exception ex) {
                mainForm.HandleError($"Fetch operation failed: {ex.Message}");
                callback(false, $"Fetch operation failed: {ex.Message}");
            }
        }

        public static async Task<User> FetchNameOfTheAuthorAsync() {
            var githubClient = new GitHubClient(new ProductHeaderValue(Path.GetFileNameWithoutExtension(currentRepositoryPath))) {
                Credentials = new Octokit.Credentials((credentials as UsernamePasswordCredentials).Username)
            };

            try {
                return await githubClient.User.Current();
            } catch (Exception ex) {
                mainForm.HandleError($"Failed to fetch GitHub user: {ex.Message}");
                return null;
            }
        }

        public static bool IsGitHubTokenValid(string inputToken) {
            string pattern = "^[0-9a-fA-F]{40}$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(inputToken.Trim());
        }

        public static bool IsValidGitDirectory(string path) {
            string gitDirectory = Path.Combine(path, ".git");
            bool isValid = Directory.Exists(gitDirectory) && (File.GetAttributes(gitDirectory) & FileAttributes.Directory) == FileAttributes.Directory;

            if (!isValid) {
                mainForm.HandleError($"{path} : is not a valid git repository");
            } else {
                mainForm.HandleError($"{path} : is valid git repository");
            }

            return isValid;
        }
    }
}