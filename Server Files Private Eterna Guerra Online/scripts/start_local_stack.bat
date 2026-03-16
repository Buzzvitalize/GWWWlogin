@echo off
setlocal

set "ROOT=%~dp0.."
for %%I in ("%ROOT%") do set "ROOT=%%~fI"
cd /d "%ROOT%"

set "VENV_PY=%ROOT%\api\.venv\Scripts\python.exe"

if not exist "%VENV_PY%" (
  echo [INFO] Creando venv...
  py -3 -m venv "%ROOT%\api\.venv"
  if errorlevel 1 (
    echo [ERROR] No se pudo crear venv con py -3. Instala Python Launcher o Python 3 en PATH.
    exit /b 1
  )
)

if not exist "%VENV_PY%" (
  echo [ERROR] No se encontro Python del venv en: %VENV_PY%
  exit /b 1
)

echo [INFO] Instalando/actualizando dependencias API...
call "%VENV_PY%" -m pip install -r "%ROOT%\api\requirements.txt"
if errorlevel 1 (
  echo [ERROR] Fallo instalacion de dependencias.
  exit /b 1
)

echo [INFO] Iniciando Game Emulator...
echo [INFO] Sincronizando config del cliente con WORLD_IP/WORLD_PORT...
call "%VENV_PY%" "%ROOT%\..\tools\sync_client_server_config.py"

start "EternaGuerra-WorldEmulator" /D "%ROOT%" "%VENV_PY%" "%ROOT%\emulator\world_emulator.py"

echo [INFO] Iniciando Login/API...
start "EternaGuerra-LoginAPI" /D "%ROOT%\api" "%VENV_PY%" -m uvicorn app:app --host 0.0.0.0 --port 8080

echo [OK] Stack local iniciado. Verifica:
echo   http://127.0.0.1:8080/health
echo   http://127.0.0.1:8080/emulator/status

echo [TIP] Si una ventana cierra, ejecuta manualmente:
echo   "%VENV_PY%" "%ROOT%\emulator\world_emulator.py"
echo   cd /d "%ROOT%\api" ^&^& "%VENV_PY%" -m uvicorn app:app --host 0.0.0.0 --port 8080

endlocal
