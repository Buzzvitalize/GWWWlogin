# VS Code / GitHub runbook

## Objetivo

Dejar una forma reproducible de levantar:

- `LoginService`;
- `GatewayService`;
- `GameServer`;
- dependencias locales (`postgres`, `redis`).

## 1. Requisitos mínimos

Necesitas una máquina o entorno con:

- `.NET 8 SDK`;
- `Docker` + `docker compose`;
- `Visual Studio Code`.

Si abres el repo desde GitHub/Codespaces, asegúrate de que el entorno tenga `dotnet` disponible. En este contenedor de trabajo no está instalado, por eso aquí dejamos la configuración preparada pero no ejecutada.

## 2. Levantar dependencias

Desde `server/`:

```bash
docker compose up -d
```

Esto levanta:

- PostgreSQL en `localhost:5432`;
- Redis en `localhost:6379`.

## 3. Servicios a ejecutar

Orden recomendado:

1. `LoginService`
2. `GameServer`
3. `GatewayService`

## 4. Opción A: usar VS Code Tasks

En VS Code ya quedan preparadas estas tasks:

- `compose-up`
- `compose-down`
- `build-server-sln`
- `run-loginservice`
- `run-gatewayservice`
- `run-gameserver`

## 5. Opción B: usar VS Code Launch

En `.vscode/launch.json` quedan estas configuraciones:

- `LoginService`
- `GatewayService`
- `GameServer`
- compound `Backend Trio`

El compound sirve para arrancar los tres procesos desde VS Code.

## 6. URLs esperadas

### LoginService
- `GET /health`

### GatewayService
- `GET /health`
- TCP gateway en el puerto configurado del servicio

### GameServer
- `GET /health`
- `GET /maps`
- `GET /world/config`
- `GET /world/maps`
- `GET /world/transitions`
- `GET /world/monsters`
- `GET /world/events`

## 7. Configuración útil de GameServer

El `GameServer` ahora consume configuración propia desde `server/src/GameServer/appsettings.json` o variables de entorno:

- `GameServer__Host` y `GameServer__Port`: endpoint HTTP real que expondrá `/health`, `/maps` y `/world/*`;
- `GameServer__ZoneSize`: tamaño de zona usado para calcular `zone:*:*`;
- `GameServer__SimulationTickMilliseconds`: frecuencia del loop de simulación;
- `GameServer__MaxSeedMonstersPerMap`: límite de monstruos sembrados por mapa;
- `GameServer__StepDistance`: distancia horizontal recorrida por tick;
- `GameServer__RecentEventLimit`: tamaño del buffer de eventos consumido por el bridge.

Si cambias `GameServer__Host` o `GameServer__Port`, actualiza también `GameServerBridge__BaseUrl` en `GatewayService`, porque el gateway depende de esa URL para `POLL` y `AROUND`.

Puedes validar rápidamente la configuración efectiva con:

```bash
curl -fsS http://127.0.0.1:5100/world/config
```

## 8. Flujo manual mínimo para observar el sistema

1. levantar dependencias con Docker;
2. correr `LoginService`;
3. correr `GameServer`;
4. correr `GatewayService`;
5. registrar cuenta y login;
6. crear personaje y seleccionarlo;
7. pedir gateway ticket;
8. conectar cliente TCP al gateway;
9. consultar `/maps` para confirmar el catálogo cargado desde el cliente;
10. probar `HELLO`, `ENTER_MAP`, `TRAVEL`, `AROUND` y `POLL`.

## 9. Configuración del cliente legado

El cliente toma el host y puerto desde `Gw Client/config.ini`. Ahora el repo ya deja `IP=127.1.1.110` y `PORT=6001`; si cambias el host público del gateway, debes reflejar esa IP/host aquí.

### Local / VS Code

- deja `IP` apuntando al host accesible por el cliente en tu PC;
- ajusta `PORT` para que coincida con el puerto TCP real del gateway;
- si cambias `Gateway:PublicHost`, mantén esa misma referencia en `Gw Client/config.ini`.

### GitHub / Codespaces

- no asumas una IP fija de GitHub;
- publica el puerto TCP del gateway con port forwarding;
- usa en `Gw Client/config.ini` el host/puerto público real que te entregue Codespaces o tu túnel;
- si el backend anuncia `PublicHost`, actualízalo para que refleje ese mismo endpoint.

## 10. Checklist operativa

Consulta también `docs/client-first-run-checklist.md` para una pasada más operativa de endpoints, comandos y señales de problema, incluyendo ejemplos copy/paste para local y host remoto.

## 11. Qué sigue después de esta guía

Una vez que esto esté corrido de punta a punta, el siguiente paso recomendado es:

- documentar mejor la conexión con el cliente legado;
- observar una corrida completa de punta a punta;
- y recién después evaluar una beta cerrada.
