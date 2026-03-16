@echo off
setlocal
cd /d "%~dp0\.."

if not exist "api\.venv\Scripts\python.exe" (
  echo [INFO] Creando venv...
  py -3 -m venv api\.venv
  call api\.venv\Scripts\python -m pip install -r api\requirements.txt
)

echo [INFO] Iniciando Game Emulator...
start "EternaGuerra-WorldEmulator" cmd /k "cd /d "%CD%" && python emulator\world_emulator.py"

echo [INFO] Iniciando Login/API...
start "EternaGuerra-LoginAPI" cmd /k "cd /d "%CD%\api" && .venv\Scripts\python -m uvicorn app:app --host 0.0.0.0 --port 8080"

echo [OK] Stack local iniciado. Verifica:
echo   http://127.0.0.1:8080/health
echo   http://127.0.0.1:8080/emulator/status
endlocal
