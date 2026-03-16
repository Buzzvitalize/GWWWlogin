@echo off
setlocal

set "ROOT=%~dp0.."
cd /d "%ROOT%"

set "VENV_PY=%ROOT%\api\.venv\Scripts\python.exe"

if not exist "%VENV_PY%" (
  echo [INFO] Creando venv...
  py -3 -m venv "%ROOT%\api\.venv"
  if errorlevel 1 (
    echo [ERROR] No se pudo crear venv con py -3. Instala Python Launcher o Python 3 en PATH.
    exit /b 1
  )
  call "%VENV_PY%" -m pip install -r "%ROOT%\api\requirements.txt"
)

echo [INFO] Iniciando Game Emulator...
start "EternaGuerra-WorldEmulator" cmd /k "cd /d \"%ROOT%\" && \"%VENV_PY%\" \"%ROOT%\emulator\world_emulator.py\""

echo [INFO] Iniciando Login/API...
start "EternaGuerra-LoginAPI" cmd /k "cd /d \"%ROOT%\api\" && \"%VENV_PY%\" -m uvicorn app:app --host 0.0.0.0 --port 8080"

echo [OK] Stack local iniciado. Verifica:
echo   http://127.0.0.1:8080/health
echo   http://127.0.0.1:8080/emulator/status

endlocal
