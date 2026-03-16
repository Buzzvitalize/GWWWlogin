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
- `scripts/start_local_stack.bat`: arranque todo-en-uno para Windows (incluye sync automático de `Eterna Guerra Online/config.ini`).
- `../tools/sync_client_server_config.py`: sincroniza `WORLD_IP/WORLD_PORT` (`api/.env`) hacia `Eterna Guerra Online/config.ini`.

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
GAME_SERVER_IP=127.0.0.1
GAME_SERVER_PORT=7000
```

> En `trusted`, `DB_USER`/`DB_PASSWORD` no se usan para conectar; se usa tu sesión Windows.

## Compatibilidad con configuración LoginServer/GameServer (referencia externa)

Si estás siguiendo una configuración tipo:

- LoginServer: `127.0.0.1:5999`
- GameServer: `127.0.0.1:7000`

ajusta en `api/.env`:

- `WORLD_IP=127.0.0.1`
- `WORLD_PORT=5999`
- `GAME_SERVER_IP=127.0.0.1`
- `GAME_SERVER_PORT=7000`
- `EMULATOR_PORTS=5999,7000,6000,29000`

> En este repo la BD actual sigue siendo SQL Server (no MySQL) y no se usa Redis en el flujo base.

## Orden del flujo en este cliente (confirmado en archivos del cliente)

En `Localization/en_us/UI/XML/LoginUI.xml` la pantalla principal después de autenticar es `SelectServerWin` (ID `190017`) con lista `SvrContent` (ID `190019`) y botón `MoreServerBtn` (ID `190033`).

Esto indica que el orden práctico es:

1. Login
2. Selección de servidor recomendado
3. (Opcional) `More Servers` para abrir selector extendido (normalmente por región/realm)

No aparece una pantalla de región independiente *antes* del primer listado; la región parece estar dentro del flujo de "More Servers" y datos que llegan por protocolo socket del login.

## 3) Encendido rápido (Windows)

```bat
cd "Server Files Private Eterna Guerra Online"
scripts\start_local_stack.bat
```

Esto abre:

1. Sync de `Eterna Guerra Online/config.ini` con `WORLD_IP/WORLD_PORT`
2. Game Emulator (`127.0.0.1:5999`)
3. Login/API (`127.0.0.1:8080`)


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

Si en log ves `RECV len=140` y después `SEND len=4` + `DISCONNECT`, significa que el cliente sí llega al LoginServer pero rechaza el ACK corto (4 bytes) para ese paquete.
En ese caso prueba temporalmente:

- `EMULATOR_BINARY_REPLY_MODE=echo`

y vuelve a capturar `logs/world_emulator_packets.log` para aproximar la respuesta real esperada por el cliente.

Por eso ahora el default recomendado en `.env` es:

- `EMULATOR_MODE=hybrid`
- `EMULATOR_BINARY_REPLY_MODE=mirror_first`
- `EMULATOR_BINARY_REPLY_HEX=47 57 68 7C`

`mirror_first` devuelve el **primer paquete binario tal cual (mirror)** y luego usa ACK hex. Esto ayuda justo en casos como tu log (`RECV len=140` y desconexión rápida).

## 3.6) Checklist rápido de readiness de login

Ejecuta:

```bash
python tools/check_login_readiness.py
```

El script valida:

- existencia de `GodsWar.exe`, `Net.dll`, `LoginUI.xml`
- sincronía entre `Eterna Guerra Online/config.ini` y `WORLD_IP/WORLD_PORT`
- que `EMULATOR_BINARY_REPLY_MODE` esté en `mirror_first`
- que `EMULATOR_PORTS` incluya `GAME_SERVER_PORT`

Si falla **solo** en `Protocol completeness`, significa que la parte de archivos/config está bien y lo pendiente es replicar respuestas binarias del protocolo legacy para lista de servidor/región.

## 4) Validación de estado online

- `GET http://127.0.0.1:8080/health`
- `GET http://127.0.0.1:8080/db/config`
- `GET http://127.0.0.1:8080/emulator/status`
- `GET http://127.0.0.1:8080/directory/regions`

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

- `logs/world_emulator_packets.log`

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


Nota: Si en tu `.env` pusiste `Server Files Private Eterna Guerra Online/logs/...`, ahora el emulador lo normaliza para no duplicar ruta. Aun así, se recomienda dejarlo como `logs/world_emulator_packets.log`.
