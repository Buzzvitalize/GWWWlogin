# Empezar desde cero

## 1. Qué estamos construyendo

Vamos a crear un servidor nuevo **clean-room** para practicar arquitectura de MMO antiguo sin reutilizar código propietario del servidor original.

## 2. Stack elegido

Usaremos **C# con .NET 8** como objetivo del proyecto porque ofrece:

- buena productividad;
- tipado fuerte;
- servicios de red y web maduros;
- facilidad para crear paneles administrativos y pruebas.

> Nota: en este contenedor no está instalado `dotnet`, así que el código queda preparado pero no compilado aquí.

## 3. Fases del proyecto

### Fase 1
- levantar `LoginService`;
- exponer un endpoint `health`;
- registrar cuentas en PostgreSQL;
- autenticar cuentas con contraseña hasheada;
- modelar respuesta de lista de servidores.

### Fase 2
- personajes;
- selección de personaje;
- tokens de sesión más completos;
- bans, roles y auditoría.

### Fase 2.1 (nivel actual)
- crear personaje;
- listar personajes por cuenta;
- definir punto inicial de spawn;
- persistir sesión;
- seleccionar personaje;
- dejar anotado el siguiente paso.

### Fase 3
- gateway TCP;
- handshake;
- entrada al mapa;
- comandos base;
- estado de mapa en memoria;
- ticket de entrada al mapa;
- catálogo de paquetes.

### Fase 4
- separar `GameServer`;
- mover simulación activa fuera del gateway;
- preparar conexión interna gateway <-> runtime del mundo;
- dejar el entorno listo para correr desde VS Code/GitHub cuando `dotnet` esté disponible.

## 4. Cómo pensar el sistema

Separa siempre:

- **transporte**: HTTP/TCP;
- **aplicación**: casos de uso;
- **dominio**: reglas del juego;
- **infraestructura**: DB, cache, logs.

## 5. Qué estudiar primero

Antes de crear entidades reales del mapa, revisa también `docs/client-reference.md` para apoyarte en la información del cliente ya subida al repo.


Lee estos archivos en este orden:

1. `server/src/LoginService/Program.cs`
2. `server/src/LoginService/Services/AccountService.cs`
3. `server/src/LoginService/Data/AuthDbContext.cs`
4. `server/src/LoginService/Models/Account.cs`

## 6. Próximo paso recomendado

Ahora que el contrato del gateway quedó más estable, seguimos con esta secuencia:

1. mover simulación/estado activo a `GameServer`;
2. definir el contrato interno entre `GatewayService` y `GameServer`;
3. dejar guía reproducible para abrir el repo desde VS Code/GitHub y levantar servicios locales;
4. luego recién pensar en una beta cerrada.
