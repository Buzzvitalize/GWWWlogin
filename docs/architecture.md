# Arquitectura inicial

## Servicios

### LoginService
Responsable de:
- registrar cuentas;
- autenticar usuarios;
- crear personajes iniciales;
- listar personajes por cuenta;
- emitir sesión inicial persistida;
- seleccionar personaje antes del gateway;
- devolver lista de servidores disponibles;
- emitir ticket para entrada al gateway.

### GatewayService
Responsable de:
- aceptar conexiones TCP iniciales;
- ejecutar handshake inicial por sesión;
- validar entrada al mapa usando la sesión persistida;
- negociar contrato/versiones del protocolo;
- reenviar hacia el runtime del mundo alojado en `GameServer`.

### GameServer
Responsable de:
- alojar simulación activa por mapa y zona;
- avanzar ticks de mundo;
- mantener runtime del mundo separado del gateway;
- preparar integración futura con entidades vivas, IA y mapas reales;
- exponer snapshots/eventos internos para que el gateway deje de alojar la simulación.

## Carpetas

```text
server/
  src/
    LoginService/
      Data/
      Models/
      Services/
    GatewayService/
    GameServer/
    Shared/
  tests/
```

## Decisiones

- Empezar por HTTP para iterar rápido.
- Persistir cuentas en PostgreSQL desde el principio.
- Mantener TCP del juego para una siguiente fase.
- Usar DTOs simples y contratos claros.
- Separar gateway y simulación cuando el contrato del mundo ya sea estable.

## Persistencia actual

- `LoginService` persiste cuentas, personajes y sesiones en PostgreSQL.
- `GatewayService` reutiliza esa misma base para validar y consultar sesiones.
- `GameServer` todavía corre principalmente en memoria; aún no tiene persistencia dedicada para mundo/spawns/IA.
- `Redis` aparece preparado en `docker-compose.yml` como dependencia futura, pero todavía no es una pieza obligatoria del flujo principal.
