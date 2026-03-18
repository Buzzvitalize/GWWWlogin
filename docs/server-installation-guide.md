# Guía completa de instalación y arranque local

## Objetivo

Tener los tres servicios (`LoginService`, `GatewayService`, `GameServer`) corriendo en tu computadora con la base de datos lista y el backend online para pruebas locales.

## 1. Qué instala realmente este repo

Hoy el backend está dividido así:

- `LoginService`: cuentas, login, personajes, sesiones y ticket de gateway;
- `GatewayService`: entrada TCP del juego y puente con el mundo;
- `GameServer`: runtime del mundo, monstruos, transiciones y eventos;
- `PostgreSQL`: persistencia actual de cuentas, personajes y sesiones;
- `Redis`: está preparado en Docker como dependencia futura, pero hoy no es crítico para el flujo principal.

## 2. Sí, sí tenemos base de datos

La base de datos actual es **PostgreSQL**.

Se usa hoy para guardar:

- cuentas;
- personajes;
- sesiones.

Actualmente **no** estamos persistiendo todavía el estado completo del mundo en una DB separada. El `GameServer` sigue trabajando principalmente en memoria para simulación, snapshots y eventos.

### Resumen honesto del estado actual

- `LoginService` usa PostgreSQL;
- `GatewayService` también consulta PostgreSQL para sesiones;
- `GameServer` hoy no necesita una base de datos propia para arrancar;
- el siguiente paso natural es decidir si el runtime del mundo seguirá temporalmente en memoria o si pasaremos a persistencia de spawns/estado por mapa.

## 3. Qué necesitas instalar en tu PC

Instala esto primero:

### Opción recomendada: Windows 10/11

1. **Git**
2. **.NET 8 SDK**
3. **Docker Desktop**
4. **Visual Studio Code**
5. Extensión de C# para VS Code

## 4. Links oficiales recomendados

- Git: <https://git-scm.com/downloads>
- .NET 8 SDK: <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
- Docker Desktop: <https://www.docker.com/products/docker-desktop/>
- VS Code: <https://code.visualstudio.com/>

## 5. Verificación rápida después de instalar

Abre una terminal nueva y ejecuta:

```bash
git --version
dotnet --version
docker --version
docker compose version
```

Si esos cuatro comandos responden bien, ya tienes el entorno base.

## 6. Descargar y abrir el proyecto

```bash
git clone <TU-REPO-O-FORK>
cd GWWWlogin
```

Si usarás VS Code:

```bash
code .
```

## 7. Levantar la base de datos y dependencias

Desde la carpeta `server/`:

```bash
cd server
docker compose up -d
```

Eso levanta:

- PostgreSQL en `localhost:5432`
- Redis en `localhost:6379`

### Cómo confirmar que PostgreSQL está arriba

```bash
docker compose ps
```

Debes ver el contenedor `postgres` en estado `running` o equivalente.

## 8. Cómo se crea la base de datos

No hace falta que crees tablas manualmente para la primera prueba.

El `LoginService` ejecuta `EnsureCreated()` al iniciar, así que cuando se conecta correctamente a PostgreSQL crea la estructura inicial que necesita.

En otras palabras:

1. levantas PostgreSQL con Docker;
2. arrancas `LoginService`;
3. el servicio crea la DB schema inicial automáticamente si no existe.

## 9. Variables y configuración importantes

### LoginService y GatewayService

Ambos usan la misma cadena de conexión `AuthDb`.

Valores por defecto actuales:

- Host: `localhost`
- Port: `5432`
- Database: `gwww_auth`
- Username: `gwww`
- Password: `gwww_dev`

### GameServer

Parámetros principales:

- `GameServer__Host=0.0.0.0`
- `GameServer__Port=5100`
- `GameServer__ZoneSize=200`
- `GameServer__SimulationTickMilliseconds=2000`
- `GameServer__MaxSeedMonstersPerMap=8`
- `GameServer__StepDistance=12`
- `GameServer__RecentEventLimit=200`

### Bridge entre Gateway y GameServer

El gateway apunta al GameServer con:

- `GameServerBridge__BaseUrl=http://127.0.0.1:5100`

Si cambias el puerto del `GameServer`, también debes cambiar esta URL.

## 10. Orden correcto para arrancar todo

Arranca en este orden:

1. `docker compose up -d`
2. `LoginService`
3. `GameServer`
4. `GatewayService`

## 11. Cómo arrancarlo desde VS Code

El repo ya trae:

- `.vscode/tasks.json`
- `.vscode/launch.json`

Puedes usar:

- `compose-up`
- `build-server-sln`
- `run-loginservice`
- `run-gameserver`
- `run-gatewayservice`
- `Backend Trio`

## 12. Cómo arrancarlo manualmente por terminal

Desde la raíz del repo:

### LoginService

```bash
dotnet run --project server/src/LoginService/LoginService.csproj
```

### GameServer

```bash
dotnet run --project server/src/GameServer/GameServer.csproj
```

### GatewayService

```bash
dotnet run --project server/src/GatewayService/GatewayService.csproj
```

## 13. Puertos esperados

### LoginService

- HTTP: `5000` si así lo lanzas en local

### GameServer

- HTTP: `5100`

### GatewayService

- HTTP health: `5200` si tu launch/config local lo expone así
- TCP juego: `6001`

> Importante: el puerto del cliente legado que más te importa para entrar al juego es el **TCP del gateway**, o sea `6001`.

## 14. Checks mínimos para confirmar que quedó online

### LoginService

```bash
curl -fsS http://127.0.0.1:5000/health
```

### GameServer

```bash
curl -fsS http://127.0.0.1:5100/health
curl -fsS http://127.0.0.1:5100/world/config
curl -fsS http://127.0.0.1:5100/world/maps
curl -fsS http://127.0.0.1:5100/world/monsters?mapId=1001
```

### GatewayService

```bash
curl -fsS http://127.0.0.1:5200/health
```

Si los tres servicios responden, el backend ya está online.

## 15. Cómo preparar el cliente legado

Archivo:

`Gw Client/config.ini`

Valores actuales del repo:

```ini
[SERVER]
PORT=6001
IP=127.1.1.110
```

### Para pruebas locales

Debes asegurarte de que:

- el gateway realmente escuche en `6001`;
- el host configurado sea accesible por el cliente;
- `LoginService` anuncie el mismo host/puerto que espera el cliente.

## 16. Primer flujo funcional recomendado

Una vez arriba todo:

1. registrar cuenta;
2. hacer login;
3. crear personaje;
4. seleccionar personaje;
5. pedir gateway ticket;
6. conectar al gateway TCP;
7. probar `HELLO`, `ENTER_MAP`, `AROUND`, `POLL`, `TRAVEL`.

## 17. Qué hacer si falla la base de datos

### Caso 1: PostgreSQL no arranca

Prueba:

```bash
cd server
docker compose logs postgres
```

### Caso 2: LoginService no conecta

Revisa:

- `server/src/LoginService/appsettings.json`
- `server/src/GatewayService/appsettings.json`
- `server/.env.example`

Confirma que `AuthDb` apunte a la misma DB levantada por Docker.

### Caso 3: tablas no aparecen

Primero confirma que `LoginService` arrancó sin error. Si arrancó bien, `EnsureCreated()` debe haber intentado crear el schema.

## 18. Entonces, ¿seguimos con GameServer o no?

Sí: **el siguiente foco recomendado sigue siendo `GameServer`**, pero con una meta más concreta.

### Prioridad recomendada

1. validar arranque local completo con DB + login + gateway + game server;
2. endurecer `TRAVEL` y transiciones reales entre mapas;
3. mover spawns/estado del mundo hacia una persistencia mejor definida;
4. ampliar NPCs/monstruos reales por escena;
5. recién después pensar en herramientas administrativas o beta cerrada.

## 19. Recomendación práctica

Si tu objetivo inmediato es “tenerlo online en tu PC”, no sigas agregando features todavía.

Haz primero esta meta cerrada:

- PostgreSQL arriba;
- `LoginService` arriba;
- `GameServer` arriba;
- `GatewayService` arriba;
- `curl /health` funcionando;
- creación de cuenta y personaje funcionando;
- cliente entrando por gateway.

Cuando eso esté estable, ahí sí seguimos con el `GameServer` de forma más segura.
