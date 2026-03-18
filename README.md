# GWWWlogin clean-room server starter

Este repositorio contiene dos partes separadas:

- `Gw Client/`: cliente legado que se usa solo como referencia de configuración local.
- `server/`: nuevo servidor clean-room creado desde cero para aprender y avanzar paso a paso.

## Objetivo

Construir un backend propio, mantenible y documentado, sin copiar archivos propietarios del servidor original.

## Arquitectura inicial

1. `LoginService`: registro, autenticación, lista de servidores y token de sesión.
2. `Shared`: contratos y utilidades comunes.
3. `docs/`: notas del protocolo, fases del proyecto y decisiones técnicas.

## Flujo actual

### Nivel 1 - Login y cuentas
1. registrar cuenta con `POST /api/accounts/register`;
2. consultar salud con `GET /health`;
3. obtener servidores con `GET /api/servers`;
4. autenticar con `POST /api/login`.

### Nivel 2 - Personajes
5. crear personaje con `POST /api/characters`;
6. listar personajes por cuenta con `GET /api/accounts/{accountId}/characters`;
7. seleccionar personaje con `POST /api/sessions/select-character`;
8. consultar sesión con `GET /api/sessions/{token}`.

### Nivel 3 - Gateway y entrada al mapa
9. preparar ticket de gateway con `POST /api/sessions/{token}/gateway-ticket`;
10. levantar `GatewayService`;
11. probar `HELLO <token>` en el gateway TCP;
12. probar `ENTER_MAP <token>` en el gateway TCP;
13. usar `PING <token>`, `WHOAMI <token>` y `MOVE <token> <x> <y>` como comandos base;
14. usar `LEAVE <token>` y `POLL <token>` para broadcast base;
15. usar `AROUND <token>` para snapshot de entidades del mapa;
16. iniciar Athens en `Athens_Newbie` y Sparta en `Sparta_Newbie`.

## Seguimiento del progreso

Consulta `docs/progress-roadmap.md` para ver en qué nivel vamos, qué terminamos y qué sigue después.

## Referencia del cliente local

Consulta `docs/client-reference.md` para aprovechar la información ya subida en `Gw Client/Localization/en_us` y `Gw Client/Map`.

## Primer paso de estudio

Lee `docs/getting-started.md` y luego revisa `server/src/LoginService/Program.cs`.
