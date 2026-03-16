# Server Files Private Eterna Guerra Online

Implementación base de **login + creación de personaje + entrada al mundo** conectada a **SQL Server 2025**.

## Estructura

- `sql/01_create_database.sql`: crea BD, cuentas y sesiones.
- `sql/02_auth_procedures.sql`: SP de login.
- `sql/03_seed_demo.sql`: usuario demo inicial.
- `sql/04_gameplay_schema.sql`: tabla de personajes y relación con sesión.
- `sql/05_gameplay_procedures.sql`: SP de register/create/select character.
- `api/app.py`: API FastAPI (`/auth`, `/characters`, `/session/enter-world`).
- `api/requirements.txt`: dependencias Python.
- `api/.env.example`: variables de entorno.

## 1) Preparar SQL Server 2025

Ejecuta en orden:

1. `sql/01_create_database.sql`
2. `sql/02_auth_procedures.sql`
3. `sql/03_seed_demo.sql`
4. `sql/04_gameplay_schema.sql`
5. `sql/05_gameplay_procedures.sql`

## 2) Levantar API

```bash
cd "Server Files Private Eterna Guerra Online/api"
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
cp .env.example .env
uvicorn app:app --host 0.0.0.0 --port 8080
```

## 3) Configuración `.env`

```env
DB_CONNECTION_STRING=DRIVER={ODBC Driver 18 for SQL Server};SERVER=localhost,1433;DATABASE=EternaGuerraAuth;UID=sa;PWD=YourStrong!Passw0rd;Encrypt=no;TrustServerCertificate=yes;
API_PORT=8080
WORLD_IP=127.0.0.1
WORLD_PORT=5999
```

## 4) Flujo real para entrar

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

`POST /auth/login`

```json
{
  "username": "admin",
  "password": "Admin123*",
  "region": 11,
  "locale": "en_us",
  "client_version": "gw_setup_2.48.001_usa"
}
```

Guarda `access_token`.

### C) Crear personaje

`POST /characters` con header `Authorization: Bearer <access_token>`

```json
{
  "character_name": "AresOne",
  "class_code": "Warrior",
  "faction": "Athens"
}
```

### D) Seleccionar personaje

`POST /characters/{character_id}/select` con el mismo Bearer token.

### E) Entrar al mundo

`POST /session/enter-world` con el mismo Bearer token.

Responde IP/puerto del world server + personaje activo.

## Notas

- Password hash en SQL con `HASHBYTES('SHA2_256', salt + ':' + password)`.
- Límite por cuenta: máximo 3 personajes.
- Si quieres conectar esto al launcher/UI Blueprint, usa estos endpoints en ese orden.
