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
- preparar futura lógica del mundo.

## Carpetas

```text
server/
  src/
    LoginService/
      Data/
      Models/
      Services/
    Shared/
  tests/
```

## Decisiones

- Empezar por HTTP para iterar rápido.
- Persistir cuentas en PostgreSQL desde el principio.
- Mantener TCP del juego para una siguiente fase.
- Usar DTOs simples y contratos claros.
