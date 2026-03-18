# Arquitectura inicial

## Servicios

### LoginService
Responsable de:
- autenticar usuarios;
- emitir sesión inicial;
- devolver lista de servidores disponibles.

## Carpetas

```text
server/
  src/
    LoginService/
    Shared/
  tests/
```

## Decisiones

- Empezar por HTTP para iterar rápido.
- Mantener TCP del juego para una siguiente fase.
- Usar DTOs simples y contratos claros.
