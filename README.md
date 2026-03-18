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

1. registrar cuenta con `POST /api/accounts/register`;
2. consultar salud con `GET /health`;
3. obtener servidores con `GET /api/servers`;
4. autenticar con `POST /api/login`.

## Primer paso de estudio

Lee `docs/getting-started.md` y luego revisa `server/src/LoginService/Program.cs`.
