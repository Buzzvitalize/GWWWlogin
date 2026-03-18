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
- en progreso.

### Ya hicimos en este nivel
- modelo `Character`;
- relación `Account -> Characters`;
- endpoint para crear personaje;
- endpoint para listar personajes por cuenta;
- spawn inicial en mapa base.

### Qué sigue después
- selección de personaje;
- sesión persistida;
- primer gateway TCP.

## Nota para continuar

Cuando cambiemos de nivel lo vamos a dejar anotado aquí con esta frase:

> Ya terminamos este nivel; ahora sí pasamos al siguiente bloque.
