# Windows quickstart ultra práctico: abre esto, pega esto, verifica esto

## Objetivo

Esta guía es la versión más directa posible. Está pensada para seguirse casi sin interpretar nada: abrir PowerShell, pegar comandos, abrir VS Code y arrancar el backend.

## Lo que vas a lograr al final

Si todo sale bien, terminarás con:

- PostgreSQL arriba;
- Redis arriba;
- `LoginService` arriba;
- `GameServer` arriba;
- `GatewayService` arriba;
- el cliente apuntando al gateway en `6001`.

## Bloque 1 - Instala lo mínimo

Haz esto primero, en este orden:

1. instala **Git** desde <https://git-scm.com/download/win>
2. instala **.NET 8 SDK** desde <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
3. instala **Docker Desktop** desde <https://www.docker.com/products/docker-desktop/>
4. instala **VS Code** desde <https://code.visualstudio.com/>
5. en VS Code instala la extensión **C#** de Microsoft

## Bloque 2 - Verifica que Windows ya reconoce todo

Abre **PowerShell** y pega esto:

```powershell
git --version
dotnet --version
docker --version
docker compose version
```

### Qué debe pasar

- los 4 comandos deben devolver una versión;
- si alguno falla, no sigas todavía: instala o reinicia antes de continuar.

## Bloque 3 - Descarga el repo

En PowerShell, pega esto cambiando la URL si hace falta:

```powershell
cd C:\Users\$env:USERNAME\Desktop
git clone <URL-DE-TU-REPO>
cd GWWWlogin
```

### Qué debe pasar

- debe aparecer la carpeta `GWWWlogin` en tu escritorio;
- debes quedar parado dentro de esa carpeta.

## Bloque 4 - Abre el repo en VS Code

En la misma PowerShell, pega esto:

```powershell
code .
```

### Si no abre

Hazlo manualmente:

1. abre VS Code;
2. clic en **File**;
3. clic en **Open Folder**;
4. selecciona la carpeta `GWWWlogin`.

## Bloque 5 - Abre Docker Desktop

1. pulsa `Win`;
2. escribe `Docker Desktop`;
3. ábrelo;
4. espera a que diga que Docker está running.

No sigas hasta que Docker realmente termine de iniciar.

## Bloque 6 - Levanta la base de datos

En la terminal integrada de VS Code, pega esto:

```powershell
cd server
docker compose up -d
docker compose ps
```

### Qué debe pasar

- debe levantarse `postgres`;
- debe levantarse `redis`;
- `docker compose ps` debe mostrar los contenedores arriba.

## Bloque 7 - Qué base de datos estamos usando hoy

La base de datos actual es **PostgreSQL**.

Hoy guarda:

- cuentas;
- personajes;
- sesiones.

`GameServer` todavía no usa una base de datos propia obligatoria para arrancar; por ahora trabaja principalmente en memoria.

## Bloque 8 - Arranca LoginService

Vuelve a la raíz del repo y pega esto en una terminal nueva:

```powershell
cd C:\Users\$env:USERNAME\Desktop\GWWWlogin
dotnet run --project server/src/LoginService/LoginService.csproj
```

### Qué debe pasar

- el proceso debe quedar corriendo;
- si PostgreSQL está bien, `LoginService` debe crear el schema inicial automáticamente.

## Bloque 9 - Arranca GameServer

Abre otra terminal nueva y pega esto:

```powershell
cd C:\Users\$env:USERNAME\Desktop\GWWWlogin
dotnet run --project server/src/GameServer/GameServer.csproj
```

### Qué debe pasar

- el proceso debe quedar corriendo;
- más adelante debe responder en `http://127.0.0.1:5100`.

## Bloque 10 - Arranca GatewayService

Abre una tercera terminal nueva y pega esto:

```powershell
cd C:\Users\$env:USERNAME\Desktop\GWWWlogin
dotnet run --project server/src/GatewayService/GatewayService.csproj
```

### Qué debe pasar

- el proceso debe quedar corriendo;
- el gateway TCP debe terminar escuchando en el puerto del juego (`6001`).

## Bloque 11 - Verifica que los tres servicios respondan

Abre una cuarta terminal y pega esto:

```powershell
curl http://127.0.0.1:5000/health
curl http://127.0.0.1:5100/health
curl http://127.0.0.1:5100/world/config
curl http://127.0.0.1:5100/world/maps
curl http://127.0.0.1:5200/health
```

### Qué debe pasar

- todos deben responder;
- si uno falla, ese servicio todavía no quedó online.

## Bloque 12 - Revisa la configuración del cliente

Abre el archivo:

`Gw Client/config.ini`

Y confirma que esté así:

```ini
[SERVER]
PORT=6001
IP=127.1.1.110
```

## Bloque 13 - Qué debe coincidir obligatoriamente

Antes de intentar entrar con el cliente, confirma esto:

1. `GatewayService` realmente está escuchando en `6001`;
2. `LoginService` anuncia el host correcto;
3. el cliente usa ese mismo host/puerto;
4. `GameServer` está arriba en `5100`;
5. `GameServerBridge__BaseUrl` apunta a `http://127.0.0.1:5100`.

## Bloque 14 - Primer objetivo realista

No intentes “tener todo el MMO” de una vez. Tu meta inmediata debería ser esta:

1. que PostgreSQL arranque;
2. que `LoginService` arranque;
3. que `GameServer` arranque;
4. que `GatewayService` arranque;
5. que los `/health` respondan;
6. que puedas crear cuenta;
7. que puedas crear personaje;
8. que puedas pedir ticket;
9. que el cliente pueda conectar al gateway.

## Bloque 15 - Si algo falla

### Si falla Docker

Pega esto:

```powershell
docker --version
docker compose version
```

### Si falla PostgreSQL

Desde `server/`, pega esto:

```powershell
docker compose logs postgres
```

### Si falla LoginService

Revisa:

- `server/src/LoginService/appsettings.json`
- que Docker siga arriba
- que PostgreSQL esté corriendo

### Si falla GatewayService

Revisa:

- `server/src/GatewayService/appsettings.json`
- que `GameServer` esté arriba en `5100`
- que el puerto `6001` no esté ocupado por otra app

## Bloque 16 - Qué sigue después

Cuando esta guía te funcione de punta a punta, ahí sí el siguiente paso recomendado es:

1. probar registro/login/personaje completos;
2. validar entrada al mapa;
3. endurecer `TRAVEL`;
4. seguir con `GameServer`, sobre todo persistencia real de mundo y spawns.
