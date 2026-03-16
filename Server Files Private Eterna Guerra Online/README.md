# Server Files Private Eterna Guerra Online

Implementación base de **Login Emulator + Game Emulator** para pruebas locales en `127.0.0.1:5999`.

## Estructura

- `sql/01_create_database.sql`: crea BD, cuentas y sesiones.
- `sql/02_auth_procedures.sql`: SP de login.
- `sql/03_seed_demo.sql`: usuario demo inicial.
- `sql/04_gameplay_schema.sql`: tabla de personajes y relación con sesión.
- `sql/05_gameplay_procedures.sql`: SP de register/create/select character.
- `api/app.py`: API FastAPI (`/auth`, `/characters`, `/session/enter-world`, `/emulator/status`).
- `emulator/world_emulator.py`: world emulator TCP simple que responde `SYSTEM_ONLINE`.
- `scripts/start_world_emulator.sh` / `.bat`: arranque rápido del game emulator.

## 1) Preparar SQL Server 2025

Ejecuta en orden:

1. `sql/01_create_database.sql`
2. `sql/02_auth_procedures.sql`
3. `sql/03_seed_demo.sql`
4. `sql/04_gameplay_schema.sql`
5. `sql/05_gameplay_procedures.sql`

## 2) Configurar entorno local

En `api/.env` (copiando de `.env.example`):

```env
DB_CONNECTION_STRING=DRIVER={ODBC Driver 18 for SQL Server};SERVER=localhost,1433;DATABASE=EternaGuerraAuth;UID=sa;PWD=YourStrong!Passw0rd;Encrypt=no;TrustServerCertificate=yes;
API_PORT=8080
WORLD_IP=127.0.0.1
WORLD_PORT=5999
```

## 3) Ejecutar emuladores paso por paso

### Paso A: levantar Game Emulator

Linux/macOS:

```bash
cd "Server Files Private Eterna Guerra Online"
./scripts/start_world_emulator.sh
```

Windows:

```bat
cd "Server Files Private Eterna Guerra Online"
scripts\start_world_emulator.bat
```

Cuando esté correcto verás en consola: `Estado: Sistema Online`.

### Paso B: levantar Login Emulator / API

```bash
cd "Server Files Private Eterna Guerra Online/api"
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
cp .env.example .env
uvicorn app:app --host 0.0.0.0 --port 8080
```

## 4) Validación de estado online

- `GET http://127.0.0.1:8080/health`
- `GET http://127.0.0.1:8080/emulator/status`

La respuesta esperada del segundo endpoint incluye:

```json
{
  "online": true,
  "message": "Sistema Online"
}
```

## 5) Flujo completo para entrar

### A) Registro (opcional)

`POST /auth/register`

```json
{
  "username": "admin2",
  "password": "Admin123*",
  "nickname": "AdminTwo",
  "region": 11,
  "locale": "en_us"
}
```

### B) Login

`POST /auth/login` (usa `admin / Admin123*` si corriste el seed)

### C) Crear personaje

`POST /characters` con `Authorization: Bearer <access_token>`

### D) Seleccionar personaje

`POST /characters/{character_id}/select`

### E) Entrar al mundo

`POST /session/enter-world`

> Si el Game Emulator está apagado, este paso devuelve `WORLD_EMULATOR_OFFLINE`.

## Notas

- Password hash en SQL con `HASHBYTES('SHA2_256', salt + ':' + password)`.
- Límite por cuenta: máximo 3 personajes.
- Todo está orientado a **emulación local** y pruebas, no a producción oficial.
