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
- ticket de entrada al mapa;
- catálogo de paquetes.

## 4. Cómo pensar el sistema

Separa siempre:

- **transporte**: HTTP/TCP;
- **aplicación**: casos de uso;
- **dominio**: reglas del juego;
- **infraestructura**: DB, cache, logs.

## 5. Qué estudiar primero

Lee estos archivos en este orden:

1. `server/src/LoginService/Program.cs`
2. `server/src/LoginService/Services/AccountService.cs`
3. `server/src/LoginService/Data/AuthDbContext.cs`
4. `server/src/LoginService/Models/Account.cs`

## 6. Próximo paso recomendado

Cuando terminemos este nivel, seguimos con una de estas tareas:

1. ampliar handshake inicial del gateway;
2. agregar migrations formales de EF Core;
3. agregar comandos base del juego;
4. empezar estado real del mapa.
