@echo off
setlocal

echo Assign variables from command-line arguments >> output.txt
set "repoPath=%1"
set "gitURL=https://github.com/viitoradmin/unityvc-base-project.git"
set "projectDirectory=%2"
set "driveName=%3"
set "folderName=%4"
set "baseProjectBranchName=%5"
set "projectBranchName=%6"
set "projectName=%7"
set "unityPath=%8"
set "folderPath=%9"
set "outputFilePath=%projectDirectory%/output.txt"

echo Handling spaces by wrapping paths in quotes where necessary >> %outputFilePath%
set "quotedGitURL=%gitURL%"
set "quotedProjectDirectory=%projectDirectory%"
set "quotedUnityPath=%unityPath%"
set "quotedFolderPath=%folderPath%"

echo Check if project directory exists >> %outputFilePath% 
if not exist "%quotedProjectDirectory%" (
    echo Directory "%quotedProjectDirectory%" does not exist. >> %outputFilePath% 
    exit /b 1
)
echo Successfully found directory "%quotedProjectDirectory%". >> %outputFilePath% 

cd "%quotedProjectDirectory%"
if %errorlevel% neq 0 (
    echo Failed to navigate to directory "%quotedProjectDirectory%". >> %outputFilePath% 
    exit /b 1
)
echo Successfully navigated to directory "%quotedProjectDirectory%". >> %outputFilePath% 

echo Check if git is installed >> %outputFilePath% 
git --version >nul 2>&1
if %errorlevel% neq 0 (
    echo Git is not installed or not found in PATH. >> %outputFilePath% 
    exit /b 9009
)
echo Git is installed. >> %outputFilePath% 

git clone %quotedGitURL%
if %errorlevel% neq 0 (
    echo Failed to clone the repository from %quotedGitURL%. >> %outputFilePath% 
    exit /b 1
)
echo Successfully cloned the repository from %quotedGitURL%. >> %outputFilePath% 

ren unityvc-base-project "%folderName%"
if %errorlevel% neq 0 (
    echo Failed to rename the directory to "%folderName%". >> %outputFilePath% 
    exit /b 1
)
echo Successfully renamed the directory to "%folderName%". >> %outputFilePath% 

cd "%folderName%"
if %errorlevel% neq 0 (
    echo Failed to navigate to directory "%folderName%". >> %outputFilePath% 
    exit /b 1
)
echo Successfully navigated to directory "%folderName%". >> %outputFilePath% 

git checkout %baseProjectBranchName%
if %errorlevel% neq 0 (
    echo Failed to checkout branch %baseProjectBranchName%. >> %outputFilePath% 
    exit /b 1
)
echo Successfully checked out branch %baseProjectBranchName%. >> %outputFilePath% 

git branch --set-upstream-to=origin/%baseProjectBranchName%
if %errorlevel% neq 0 (
    echo Failed to set upstream branch to origin/%baseProjectBranchName%. >> %outputFilePath% 
    exit /b 1
)
echo Successfully set upstream branch to origin/%baseProjectBranchName%. >> %outputFilePath% 

echo Check if Unity executable exists >> %outputFilePath% 
if not exist %quotedUnityPath% (
    echo Unity executable "%quotedUnityPath%" does not exist. >> %outputFilePath% 
    exit /b 1
)
echo Unity executable %quotedUnityPath% found. >> %outputFilePath% 

start /wait "" %quotedUnityPath% -batchmode -quit -projectPath %quotedFolderPath%
if %errorlevel% neq 0 (
    echo Unity command failed.
    exit /b 1
)
echo Unity command executed successfully. >> %outputFilePath% 

cd Assets\Games
if %errorlevel% neq 0 (
    echo Failed to navigate to Assets\Games. >> %outputFilePath% 
    exit /b 1
)
echo Successfully navigated to Assets\Games. >> %outputFilePath% 

git clone "%repoPath%"
if %errorlevel% neq 0 (
    echo Failed to clone child repository "%repoPath%". >> %outputFilePath% 
    exit /b 1
)
echo Successfully cloned child repository "%repoPath%". >> %outputFilePath% 

cd "%projectName%"
if %errorlevel% neq 0 (
    echo Failed to navigate to directory "%projectName%". >> %outputFilePath% 
    exit /b 1
)
echo Successfully navigated to directory "%projectName%". >> %outputFilePath% 

git checkout %projectBranchName%
if %errorlevel% neq 0 (
    echo Failed to checkout branch %projectBranchName%. >> %outputFilePath% 
    exit /b 1
)
echo Successfully checked out branch %projectBranchName%. >> %outputFilePath% 

git branch --set-upstream-to=origin/%projectBranchName%
if %errorlevel% neq 0 (
    echo Failed to set upstream branch to origin/%projectBranchName%. >> %outputFilePath% 
    exit /b 1
)
echo Successfully set upstream branch to origin/%projectBranchName%. >> %outputFilePath% 
endlocal

exit /b 0
