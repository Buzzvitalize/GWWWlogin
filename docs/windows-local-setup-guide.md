# Guía práctica para Windows: instalar todo y dejar el server online

## Objetivo

Esta guía está pensada para alguien que quiere hacer esto literalmente paso a paso en **Windows 10/11**, con instrucciones concretas de qué instalar, qué abrir, qué comando ejecutar y cómo confirmar que todo quedó funcionando.

## Antes de empezar

Vas a dejar funcionando esto:

1. PostgreSQL por Docker;
2. Redis por Docker;
3. `LoginService`;
4. `GameServer`;
5. `GatewayService`;
6. cliente apuntando al gateway.

## Paso 1 - Instalar Git

1. Abre este link: <https://git-scm.com/download/win>
2. Descarga el instalador de Windows.
3. Ejecuta el `.exe`.
4. Durante la instalación puedes dejar las opciones por defecto.
5. Al terminar, cierra el instalador.

### Cómo comprobar que quedó bien

1. Pulsa `Win`.
2. Escribe `PowerShell`.
3. Abre **Windows PowerShell**.
4. Ejecuta:

```powershell
git --version
```

Si responde con una versión, Git quedó instalado.

## Paso 2 - Instalar .NET 8 SDK

1. Abre: <https://dotnet.microsoft.com/en-us/download/dotnet/8.0>
2. Busca **.NET SDK 8.x**.
3. Descarga el instalador para Windows x64.
4. Ejecuta el instalador.
5. Finaliza la instalación.

### Cómo comprobarlo

En PowerShell:

```powershell
dotnet --version
```

Si muestra una versión 8.x, ya quedó listo.

## Paso 3 - Instalar Docker Desktop

1. Abre: <https://www.docker.com/products/docker-desktop/>
2. Descarga Docker Desktop para Windows.
3. Instálalo.
4. Si te pide reiniciar, reinicia.
5. Abre Docker Desktop.
6. Espera a que diga que Docker está corriendo.

### Cómo comprobarlo

En PowerShell:

```powershell
docker --version
docker compose version
```

Si ambos responden, Docker está bien.

## Paso 4 - Instalar Visual Studio Code

1. Abre: <https://code.visualstudio.com/>
2. Descarga VS Code.
3. Instálalo.
4. Abre VS Code.

## Paso 5 - Instalar la extensión de C# en VS Code

1. Dentro de VS Code, haz clic en **Extensions** en la barra izquierda.
2. Busca `C#`.
3. Instala la extensión oficial de Microsoft.
4. Espera a que termine.

## Paso 6 - Descargar el proyecto

### Si ya tienes el repo

Solo ubica la carpeta del proyecto.

### Si aún no lo tienes

En PowerShell, ve a la carpeta donde quieres guardarlo, por ejemplo:

```powershell
cd C:\Users\TU_USUARIO\Desktop
```

Luego clona:

```powershell
git clone <URL-DE-TU-REPO>
cd GWWWlogin
```

## Paso 7 - Abrir el proyecto en VS Code

Dentro de PowerShell, desde la carpeta del repo:

```powershell
code .
```

Si `code` no abre VS Code, abre VS Code manualmente y luego usa:

- **File**
- **Open Folder**
- selecciona la carpeta `GWWWlogin`

## Paso 8 - Levantar la base de datos

1. Abre una terminal dentro de VS Code.
2. Ve a `Terminal` → `New Terminal`.
3. Ejecuta:

```powershell
cd server
docker compose up -d
```

Eso levantará:

- PostgreSQL
- Redis

### Cómo comprobar que la base de datos está arriba

```powershell
docker compose ps
```

Debes ver `postgres` corriendo.

## Paso 9 - Entender qué base de datos estamos usando

Hoy el proyecto usa **PostgreSQL**.

Guarda actualmente:

- cuentas;
- personajes;
- sesiones.

El `GameServer` todavía no tiene una DB propia obligatoria para arrancar. El mundo sigue funcionando principalmente en memoria.

## Paso 10 - Cómo se crean las tablas

No necesitas crear tablas manualmente en esta primera fase.

Cuando arranca `LoginService`, ejecuta la creación inicial del schema automáticamente.

En la práctica:

1. levantas PostgreSQL;
2. corres `LoginService`;
3. si la conexión está bien, el schema se crea solo.

## Paso 11 - Arrancar los servicios desde VS Code

El orden recomendado es:

1. `LoginService`
2. `GameServer`
3. `GatewayService`

### Opción A - Usar Run and Debug

En VS Code:

1. abre la vista **Run and Debug**;
2. selecciona `LoginService` y ejecútalo;
3. luego selecciona `GameServer` y ejecútalo;
4. luego selecciona `GatewayService` y ejecútalo.

También puedes probar el compound:

- `Backend Trio`

### Opción B - Usar terminal manualmente

Desde la raíz del repo, abre tres terminales.

#### Terminal 1 - LoginService

```powershell
dotnet run --project server/src/LoginService/LoginService.csproj
```

#### Terminal 2 - GameServer

```powershell
dotnet run --project server/src/GameServer/GameServer.csproj
```

#### Terminal 3 - GatewayService

```powershell
dotnet run --project server/src/GatewayService/GatewayService.csproj
```

## Paso 12 - Verificar que todo quedó online

Abre otra terminal y prueba esto.

### LoginService

```powershell
curl http://127.0.0.1:5000/health
```

### GameServer

```powershell
curl http://127.0.0.1:5100/health
curl http://127.0.0.1:5100/world/config
curl http://127.0.0.1:5100/world/maps
```

### GatewayService

```powershell
curl http://127.0.0.1:5200/health
```

Si responden, el backend ya está arriba.

## Paso 13 - Preparar el cliente

Archivo a revisar:

`Gw Client/config.ini`

Valores actuales esperados:

```ini
[SERVER]
PORT=6001
IP=127.1.1.110
```

## Paso 14 - Qué debe coincidir sí o sí

Debes revisar estas coincidencias:

- el gateway TCP debe escuchar en `6001`;
- el cliente debe apuntar al host/puerto correctos;
- el login debe anunciar el mismo endpoint que el cliente usará;
- si cambias el puerto del `GameServer`, también debes cambiar `GameServerBridge__BaseUrl`.

## Paso 15 - Flujo mínimo de prueba

Una vez con todo arriba:

1. registra una cuenta;
2. haz login;
3. crea un personaje;
4. selecciona el personaje;
5. pide el gateway ticket;
6. conecta el cliente al gateway;
7. prueba entrada al mapa.

## Paso 16 - Si algo falla

### Si Docker no arranca

- abre Docker Desktop;
- espera a que diga que está running;
- vuelve a probar `docker compose up -d`.

### Si PostgreSQL no levanta

Desde `server/`:

```powershell
docker compose logs postgres
```

### Si LoginService falla al iniciar

Revisa:

- `server/src/LoginService/appsettings.json`
- `server/.env.example`
- que Docker siga levantado

### Si GatewayService no ve GameServer

Revisa que `GameServer` esté realmente corriendo en `5100` y que `GameServerBridge__BaseUrl` apunte a esa misma URL.

## Paso 17 - Qué hacer después de tenerlo online

Mi recomendación sigue siendo esta:

1. primero deja el stack estable en tu PC;
2. luego valida creación de cuenta, personaje y entrada;
3. después seguimos con `GameServer`;
4. el siguiente foco fuerte debería ser persistencia real de mundo/spawns y endurecer transiciones como `TRAVEL`.

## Paso 18 - Resumen ultra corto

Si quieres la versión resumida, es esta:

1. instala Git;
2. instala .NET 8 SDK;
3. instala Docker Desktop;
4. instala VS Code;
5. abre el repo;
6. corre `docker compose up -d` en `server/`;
7. corre `LoginService`;
8. corre `GameServer`;
9. corre `GatewayService`;
10. prueba los `/health`;
11. configura el cliente con `PORT=6001` e `IP=127.1.1.110`;
12. entra.
