%3
cd %2%
git clone https://github.com/viitoradmin/unityvc-base-project.git
ren unityvc-base-project %4%
cd %4%
git checkout %5%
git branch --set-upstream-to=origin/%5%
cd Assets
cd Games
git clone %1%
cd %7
git checkout %6%
git branch --set-upstream-to=origin/%6%