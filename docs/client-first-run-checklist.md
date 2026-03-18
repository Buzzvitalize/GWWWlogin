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
- `GET /world/maps`
- `GET /world/monsters?mapId=1001`
- `GET /world/events`

Qué observar:
- que `SimulationTick` avance;
- que existan monstruos en `Athens_Newbie` o `Sparta_Newbie`;
- que `/world/events` devuelva updates con `SequenceId`.

### GatewayService
- `GET /health`

Qué observar:
- `worldRuntimeOwner = game-server`;
- `gameServerBaseUrl` correcto;
- versiones de contrato presentes.

## 3. Flujo funcional mínimo

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

## 4. Qué deberías ver

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

## 5. Señales de problema típicas

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

## 6. Qué anotar cuando hagas la primera corrida real

Cuando lo pruebes, conviene anotar:

- qué comandos del gateway respondieron bien;
- qué endpoint HTTP devolvió algo inesperado;
- si el cliente mostró mapa, entidades o movimientos;
- y en qué paso exacto se cortó el flujo si falló.
