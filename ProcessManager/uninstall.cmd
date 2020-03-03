cls
@ECHO OFF

set DOTNETFX2=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\InstallUtil
set PATH=%PATH%;%DOTNETFX2%

echo Installing IEPPAMS Win Service...
echo ---------------------------------------------------
%DOTNETFX2% /u "%~dp0ProcessManager.Service.exe"
echo ---------------------------------------------------
pause
echo Done.