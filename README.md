# GWWWlogin clean-room server starter

Este repositorio contiene dos partes separadas:

- `Gw Client/`: cliente legado que se usa solo como referencia de configuración local.
- `server/`: nuevo servidor clean-room creado desde cero para aprender y avanzar paso a paso.

## Objetivo

Construir un backend propio, mantenible y documentado, sin copiar archivos propietarios del servidor original.

## Arquitectura inicial

1. `LoginService`: autenticación, lista de servidores y token de sesión.
2. `Shared`: contratos, modelos y utilidades comunes.
3. `docs/`: notas del protocolo, fases del proyecto y decisiones técnicas.

## Primer paso

Lee `docs/getting-started.md` y luego revisa `server/src/LoginService/Program.cs`.
