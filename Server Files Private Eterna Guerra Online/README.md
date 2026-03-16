# Server Files Private Eterna Guerra Online

Implementación base de **login server** conectada directamente a **SQL Server 2025**.

## Estructura

- `sql/01_create_database.sql`: crea BD, tablas e índices.
- `sql/02_auth_procedures.sql`: stored procedures de autenticación.
- `sql/03_seed_demo.sql`: usuario demo inicial.
- `api/app.py`: API FastAPI para `/auth/login`.
- `api/requirements.txt`: dependencias Python.
- `api/.env.example`: variables de entorno.

## 1) Preparar SQL Server 2025

Ejecuta en orden:

1. `sql/01_create_database.sql`
2. `sql/02_auth_procedures.sql`
3. `sql/03_seed_demo.sql`

## 2) Levantar API de login

```bash
cd "Server Files Private Eterna Guerra Online/api"
python -m venv .venv
source .venv/bin/activate
pip install -r requirements.txt
cp .env.example .env
uvicorn app:app --host 0.0.0.0 --port 8080
```

## 3) Endpoint

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

Respuesta `200`:

```json
{
  "access_token": "...",
  "refresh_token": "...",
  "expires_in": 3600,
  "player_id": 1,
  "nickname": "Admin",
  "faction": "Athens"
}
```

## Notas

- Password hash en SQL con `HASHBYTES('SHA2_256', ...)`.
- Tokens quedan persistidos en `dbo.AuthSession`.
- Se puede extender a refresh/logout con más SPs.
