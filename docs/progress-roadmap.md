# Progress roadmap

## Nivel 1 - Login y cuentas

### Estado
- terminado.

### Ya hicimos
- registro de cuentas;
- login con contraseña hasheada;
- persistencia en PostgreSQL;
- health check del servicio.

### Qué sigue después
- crear personajes.

## Nivel 2 - Personajes básicos

### Estado
- terminado.

### Ya hicimos en este nivel
- modelo `Character`;
- relación `Account -> Characters`;
- endpoint para crear personaje;
- endpoint para listar personajes por cuenta;
- spawn inicial en mapa base;
- sesión persistida en base de datos;
- selección de personaje por sesión.

### Qué sigue después
- primer gateway TCP;
- entrada al mapa;
- comandos base del juego.

## Nota para continuar

Cuando cambiemos de nivel lo vamos a dejar anotado aquí con esta frase:

> Ya terminamos este nivel; ahora sí pasamos al siguiente bloque.

## Reglas iniciales de progreso

- nivel máximo de referencia actual: `140`;
- el personaje inicia en nivel `1`;
- el siguiente bloque será gateway + entrada a mapa.

## Nivel 3 - Gateway y entrada al mapa

### Estado
- en progreso.

### Ya hicimos en este nivel
- proyecto `GatewayService`;
- health check HTTP para gateway;
- listener TCP inicial;
- ticket de gateway desde `LoginService`.

### Qué sigue después
- handshake inicial del cliente;
- entrada al mapa;
- comandos base del juego;
- maps reales.
