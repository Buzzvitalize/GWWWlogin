# Arquitectura inicial

## Servicios

### LoginService
Responsable de:
- registrar cuentas;
- autenticar usuarios;
- emitir sesión inicial;
- devolver lista de servidores disponibles.

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
