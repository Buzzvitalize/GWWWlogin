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
- `GET /world/maps`
- `GET /world/monsters`
- `GET /world/events`

## 7. Flujo manual mínimo para observar el sistema

1. levantar dependencias con Docker;
2. correr `LoginService`;
3. correr `GameServer`;
4. correr `GatewayService`;
5. registrar cuenta y login;
6. crear personaje y seleccionarlo;
7. pedir gateway ticket;
8. conectar cliente TCP al gateway;
9. consultar `/maps` para confirmar el catálogo configurado;
10. probar `HELLO`, `ENTER_MAP`, `AROUND` y `POLL`.

## 8. Configuración del cliente legado

El cliente toma el host y puerto desde `Gw Client/config.ini`. Hoy el repo trae `IP=127.1.1.110`, pero debes alinearlo con el host real donde publiques `GatewayService`.

### Local / VS Code

- deja `IP` apuntando al host accesible por el cliente en tu PC;
- ajusta `PORT` para que coincida con el puerto TCP real del gateway;
- si cambias `Gateway:PublicHost`, mantén esa misma referencia en `Gw Client/config.ini`.

### GitHub / Codespaces

- no asumas una IP fija de GitHub;
- publica el puerto TCP del gateway con port forwarding;
- usa en `Gw Client/config.ini` el host/puerto público real que te entregue Codespaces o tu túnel;
- si el backend anuncia `PublicHost`, actualízalo para que refleje ese mismo endpoint.

## 9. Checklist operativa

Consulta también `docs/client-first-run-checklist.md` para una pasada más operativa de endpoints, comandos y señales de problema, incluyendo ejemplos copy/paste para local y host remoto.

## 10. Qué sigue después de esta guía

Una vez que esto esté corrido de punta a punta, el siguiente paso recomendado es:

- documentar mejor la conexión con el cliente legado;
- observar una corrida completa de punta a punta;
- y recién después evaluar una beta cerrada.
