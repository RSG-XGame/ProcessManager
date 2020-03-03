cls
@ECHO OFF

set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil
set PATH=%PATH%;%DOTNETFX2%

echo Installing IEPPAMS Win Service...
echo ---------------------------------------------------
%DOTNETFX2% /i "%~dp0ProcessManager.Service.exe"
net start ProcessManager.Service
echo ---------------------------------------------------
pause
echo Done.