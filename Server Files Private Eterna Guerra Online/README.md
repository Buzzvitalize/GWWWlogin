# Server Files Private Eterna Guerra Online

Implementación base de **Login Emulator + Game Emulator** para pruebas locales en `127.0.0.1:5999`.

## Objetivo de esta versión

Queda adaptado para tu escenario local:

- **SQL Server**: `localhost\SQLEXPRESS`
- **Usuario**: `ECOSEA-DO\SQLEXPRESS`
- **Sin contraseña** (autenticación integrada de Windows / trusted connection)

Y mantiene opción editable para cambiar a SQL Auth o otro server en el futuro.

## Estructura

- `sql/01_create_database.sql`: crea BD, cuentas y sesiones.
- `sql/02_auth_procedures.sql`: SP de login.
- `sql/03_seed_demo.sql`: usuario demo inicial.
- `sql/04_gameplay_schema.sql`: tabla de personajes y relación con sesión.
- `sql/05_gameplay_procedures.sql`: SP de register/create/select character.
- `api/app.py`: API FastAPI (`/auth`, `/characters`, `/session/enter-world`, `/emulator/status`, `/db/config`, `/localization/en-us/login-bundle`).
- `api/.env.example`: configuración local y futura.
- `emulator/world_emulator.py`: world emulator TCP simple que responde `SYSTEM_ONLINE`.
- `scripts/start_world_emulator.sh` / `.bat`: arranque del game emulator.
- `api/assets/en_us_login_bundle.json`: bundle de textos/login+errores tomado del cliente legacy.
- `tools/build_en_us_login_bundle.py`: regenera el bundle desde `Localization/en_us/Text`.
- `scripts/start_local_stack.bat`: arranque todo-en-uno para Windows.

## 1) Preparar SQL Server 2025

Ejecuta en orden:

1. `sql/01_create_database.sql`
2. `sql/02_auth_procedures.sql`
3. `sql/03_seed_demo.sql`
4. `sql/04_gameplay_schema.sql`
5. `sql/05_gameplay_procedures.sql`

## 2) Configurar entorno local

Copia `api/.env.example` a `api/.env` y deja por defecto:

```env
DB_SERVER=localhost\SQLEXPRESS
DB_NAME=EternaGuerraAuth
DB_AUTH_MODE=trusted
DB_USER=ECOSEA-DO\SQLEXPRESS
DB_PASSWORD=
DB_DRIVER=ODBC Driver 18 for SQL Server
DB_CONNECTION_STRING=
API_PORT=8080
WORLD_IP=127.0.0.1
WORLD_PORT=5999
```

> En `trusted`, `DB_USER`/`DB_PASSWORD` no se usan para conectar; se usa tu sesión Windows.

## 3) Encendido rápido (Windows)

```bat
cd "Server Files Private Eterna Guerra Online"
scripts\start_local_stack.bat
```

Esto abre:

1. Game Emulator (`127.0.0.1:5999`)
2. Login/API (`127.0.0.1:8080`)


### Solución si antes fallaba el World Emulator

Se reforzó el `.bat` para evitar errores de sintaxis de ruta/volumen.
Ahora usa ruta absoluta normalizada, `start /D` y ejecuta Python del venv directamente (sin `cmd /k` anidado).


## Diagnóstico para error "Verifying" -> "Connection error. Please refresh The Game"


### Confirmación técnica importante (GodsWar.exe)

El cliente **NO consume** la API HTTP (`/auth/login`) directamente; usa protocolo socket propietario en `Net.dll`.
Por eso, aunque la API esté arriba, si el handshake binario inicial no coincide, aparece:

- `Connection error. Please refresh the game`

Qué agregamos para cerrar esta brecha:

- servidor-first hello configurable (`EMULATOR_SEND_HELLO_ON_CONNECT=1`)
- secuencia inicial configurable (`EMULATOR_HELLO_SEQUENCE_HEX`, default `47 57 68 7C`)
- texto inicial configurable (`EMULATOR_HELLO_TEXT`)
- logging detallado de paquetes para calibrar handshake real

Si todavía falla, el siguiente paso es ajustar **paquete por paquete** con `logs/world_emulator_packets.log` hasta replicar el protocolo de login original.


Se detectó una causa probable: el emulador anterior cerraba la conexión demasiado pronto y no mantenía sesión/socket.

Ahora `world_emulator.py` trabaja en modo persistente y soporta:

- `EMULATOR_MODE=hybrid` (default): texto => `SYSTEM_ONLINE`, binario => echo
- `EMULATOR_MODE=echo`: siempre eco de bytes
- `EMULATOR_MODE=text`: siempre `SYSTEM_ONLINE`

Además guarda trazas de paquetes en `logs/world_emulator_packets.log` para depurar protocolo legacy.

Si sigue fallando:

1. Verifica que `Eterna Guerra Online/config.ini` apunte a `127.0.0.1:5999`.
2. Inicia stack con `scripts\start_local_stack.bat`.
3. Intenta login en cliente.
4. Comparte `logs/world_emulator_packets.log` para ajustar handshake del emulador al protocolo real del cliente.

## 3.5) Sincronizar textos reales del cliente (en_us)

```bash
python tools/build_en_us_login_bundle.py
```

Luego puedes consultar:

- `GET http://127.0.0.1:8080/localization/en-us/login-bundle`


### Lectura de tu log actual (qué significa)

Cuando ves esto:

- `CONNECT ...`
- `RECV ... len=140 ...`
- `SEND ... len=140` (echo)
- `DISCONNECT`

significa que **sí conecta por TCP**, pero el cliente rechaza la respuesta de protocolo y cierra.
No es un problema de IP/puerto, es de **handshake binario**.

Por eso ahora el default recomendado en `.env` es:

- `EMULATOR_MODE=hybrid`
- `EMULATOR_BINARY_REPLY_MODE=ack`
- `EMULATOR_BINARY_REPLY_HEX=47 57 68 7C`

(antes estaba respondiendo eco para binario y eso normalmente falla en cliente legacy).

## 4) Validación de estado online

- `GET http://127.0.0.1:8080/health`
- `GET http://127.0.0.1:8080/db/config`
- `GET http://127.0.0.1:8080/emulator/status`

Para pruebas correctas, debe aparecer `"message": "Sistema Online"`.

## 5) Flujo completo para entrar

### A) Registro (opcional)

`POST /auth/register`

### B) Login

`POST /auth/login` (usa `admin / Admin123*` si corriste el seed)

### C) Crear personaje

`POST /characters` con `Authorization: Bearer <access_token>`

### D) Seleccionar personaje

`POST /characters/{character_id}/select`

### E) Entrar al mundo

`POST /session/enter-world`

> Si el Game Emulator está apagado, devuelve `WORLD_EMULATOR_OFFLINE`.

## Notas

- Password hash en SQL con `HASHBYTES('SHA2_256', salt + ':' + password)`.
- Límite por cuenta: máximo 3 personajes.
- Todo está orientado a **emulación local** y pruebas, no a producción oficial.


### Caso: "The server is not yet available" y no aparece log

Eso suele indicar que el cliente está intentando otro puerto además de `5999` o no está escribiendo en la ruta de log esperada.

Se ajustó el emulador para escuchar múltiples puertos en paralelo:

- `EMULATOR_PORTS=5999,6000,29000`

Y para escribir log en ruta absoluta por defecto:

- `Server Files Private Eterna Guerra Online/logs/world_emulator_packets.log`

Pasos recomendados:

1. Copia `api/.env.example` a `api/.env` (si no lo hiciste después de esta actualización).
2. Inicia `scripts\start_local_stack.bat`.
3. Revisa si hay conexiones en cualquiera de esos puertos en la consola del emulador.
4. Ejecuta:

```bash
python tools/analyze_world_log.py
```

para ver resumen por cliente y firma del primer paquete.


### Confirmación adicional (cuando no hay logs TCP)

En algunos builds legacy, el cliente primero valida disponibilidad por UDP y solo después abre TCP.
Por eso se añadió también listener UDP en los mismos puertos del emulador.

Config clave:

- `EMULATOR_PORTS=5999,6000,29000`
- `EMULATOR_ENABLE_UDP=1`
- `EMULATOR_UDP_REPLY_HEX=47 57 68 7C`

Con esto, aunque el cliente no inicie TCP al principio, verás `UDP_RECV`/`UDP_SEND` en el log.
