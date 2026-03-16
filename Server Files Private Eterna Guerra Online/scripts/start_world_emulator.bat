@echo off
setlocal

set "ROOT=%~dp0.."
for %%I in ("%ROOT%") do set "ROOT=%%~fI"
cd /d "%ROOT%"

set "VENV_PY=%ROOT%\api\.venv\Scripts\python.exe"
if exist "%VENV_PY%" (
  "%VENV_PY%" "%ROOT%\emulator\world_emulator.py"
) else (
  python "%ROOT%\emulator\world_emulator.py"
)

endlocal
