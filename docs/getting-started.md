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
- exponer un endpoint de login falso para pruebas;
- modelar respuesta de lista de servidores.

### Fase 2
- cuentas reales en PostgreSQL;
- hash de contraseñas;
- tokens de sesión;
- personajes.

### Fase 3
- gateway TCP;
- handshake;
- heartbeat;
- catálogo de paquetes.

## 4. Cómo pensar el sistema

Separa siempre:

- **transporte**: HTTP/TCP;
- **aplicación**: casos de uso;
- **dominio**: reglas del juego;
- **infraestructura**: DB, cache, logs.

## 5. Próximo paso recomendado

Cuando quieras, seguimos con una de estas tareas:

1. agregar PostgreSQL;
2. diseñar cuentas y personajes;
3. crear gateway TCP;
4. definir paquete de login del juego.
