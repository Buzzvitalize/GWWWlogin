# Client first-run checklist

## Objetivo

Tener una checklist corta y operativa para validar el backend con el cliente sin perderse.

## 1. Antes de abrir el cliente

Confirma este orden:

1. `docker compose up -d` en `server/`;
2. `LoginService` arriba;
3. `GameServer` arriba;
4. `GatewayService` arriba.

## 2. Checks HTTP mínimos

### LoginService
- `GET /health`
- esperado: respuesta `ok`

### GameServer
- `GET /health`
- `GET /maps`
- `GET /world/maps`
- `GET /world/monsters?mapId=1001`
- `GET /world/events`

Qué observar:
- que `SimulationTick` avance;
- que `/maps` y `/world/maps` devuelvan el catálogo de escenas derivado del cliente;
- que existan monstruos en `Athens_Newbie` o `Sparta_Newbie`;
- que `/world/events` devuelva updates con `SequenceId`.

### GatewayService
- `GET /health`

Qué observar:
- `worldRuntimeOwner = game-server`;
- `gameServerBaseUrl` correcto;
- versiones de contrato presentes.

## 3. Configuración del cliente legado

El cliente toma el host y puerto desde `Gw Client/config.ini`.

Configuración actual del repo:

```ini
[SERVER]
PORT=5999
IP=127.1.1.110
```

### Local en tu PC

Usa este caso si el backend y el cliente viven en la misma máquina o en una red local controlada.

1. confirma qué puerto TCP expone realmente `GatewayService`;
2. si el gateway corre en `6001`, actualiza `PORT` en `Gw Client/config.ini`;
3. deja `IP` apuntando al host accesible desde el cliente.

Ejemplo local típico:

```ini
[SERVER]
PORT=6001
IP=127.1.1.110
```

### GitHub Codespaces / host remoto

No asumas una IP fija de GitHub. En Codespaces normalmente tendrás:

1. el proceso escuchando dentro del contenedor;
2. el puerto publicado mediante port forwarding;
3. un host externo generado por Codespaces o por un túnel intermedio.

Para este caso, copia en `Gw Client/config.ini` el host público real del puerto forwardeado y el puerto que hayas expuesto.

Checklist rápida:

- el puerto del gateway debe estar marcado como público o accesible desde tu máquina;
- el `PublicHost` del backend debe reflejar ese host público;
- `Gw Client/config.ini` debe usar ese mismo host/puerto.

## 4. Comandos copy/paste para primer diagnóstico

### Dependencias

```bash
cd server
docker compose up -d
```

### Health checks HTTP

```bash
curl -fsS http://127.0.0.1:5000/health
curl -fsS http://127.0.0.1:5100/health
curl -fsS http://127.0.0.1:5100/maps
curl -fsS http://127.0.0.1:5100/world/maps
curl -fsS "http://127.0.0.1:5100/world/monsters?mapId=1001"
curl -fsS http://127.0.0.1:5100/world/events
curl -fsS http://127.0.0.1:5200/health
```

### Login + personaje + ticket

Ajusta puertos/URLs si tus launch settings usan otros valores.

```bash
curl -fsS -X POST http://127.0.0.1:5000/api/accounts/register \
  -H 'Content-Type: application/json' \
  -d '{"userName":"tester1","password":"test1234"}'

curl -fsS -X POST http://127.0.0.1:5000/api/login \
  -H 'Content-Type: application/json' \
  -d '{"userName":"tester1","password":"test1234"}'

curl -fsS -X POST http://127.0.0.1:5000/api/characters \
  -H 'Content-Type: application/json' \
  -d '{"accountId":1,"name":"AthensOne","faction":"Athens","className":"Warrior"}'

curl -fsS -X POST http://127.0.0.1:5000/api/sessions/select-character \
  -H 'Content-Type: application/json' \
  -d '{"token":"REEMPLAZAR_TOKEN","characterId":1}'

curl -fsS -X POST http://127.0.0.1:5000/api/sessions/REEMPLAZAR_TOKEN/gateway-ticket
```

### Probar TCP del gateway con netcat

```bash
printf 'HELLO REEMPLAZAR_TOKEN\n' | nc 127.0.0.1 6001
printf 'ENTER_MAP REEMPLAZAR_TOKEN\n' | nc 127.0.0.1 6001
printf 'AROUND REEMPLAZAR_TOKEN\n' | nc 127.0.0.1 6001
printf 'POLL REEMPLAZAR_TOKEN\n' | nc 127.0.0.1 6001
```

## 5. Flujo funcional mínimo

1. registrar cuenta;
2. hacer login;
3. crear personaje;
4. seleccionar personaje;
5. pedir gateway ticket;
6. conectar al TCP gateway;
7. enviar `HELLO <token>`;
8. enviar `ENTER_MAP <token>`;
9. enviar `AROUND <token>`;
10. enviar `POLL <token>`.

## 6. Qué deberías ver

### HELLO
- handshake versionado;
- ids de cuenta/personaje;
- facción, clase, escena, mapa y `visibilityRadius`.

### ENTER_MAP
- escena correcta (`Athens_Newbie` o `Sparta_Newbie`);
- counts básicos de población/NPCs/monstruos.

### AROUND
- jugadores cercanos si existen;
- NPCs del mapa;
- monstruos vivos provenientes del bridge de `GameServer`.

### POLL
- eventos locales de jugador;
- eventos de runtime del `GameServer` si los monstruos se están moviendo.

## 7. Señales de problema típicas

### Si el cliente no conecta al gateway
- revisar `Gw Client/config.ini`;
- revisar el puerto real expuesto por `GatewayService`;
- revisar si el host es local, remoto o un forward de Codespaces.

### Si `AROUND` no muestra monstruos
- revisar `GET /world/monsters` en `GameServer`;
- revisar `gameServerBaseUrl` en health del gateway.

### Si `POLL` no muestra eventos de runtime
- revisar `GET /world/events`;
- revisar que `SimulationTick` cambie en `GameServer`;
- revisar que el personaje esté dentro del rango visible.

### Si no entra al mapa
- revisar login/selección de personaje;
- revisar token y gateway ticket.

## 8. Qué anotar cuando hagas la primera corrida real

Cuando lo pruebes, conviene anotar:

- qué comandos del gateway respondieron bien;
- qué endpoint HTTP devolvió algo inesperado;
- si el cliente mostró mapa, entidades o movimientos;
- y en qué paso exacto se cortó el flujo si falló.
