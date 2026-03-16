# Login de GodsWar Online (Blueprint-first)

Este documento baja a Blueprint la implementación de un login moderno sin tocar binarios legacy.

## 1) Hallazgos del cliente legacy

- El cliente tiene defaults de red en `config.ini`: `IP=67.208.220.189`, `PORT=5999`, `Region=11`, `Locals=en_us`.
- El archivo `Localization/en_us/Text/LoginInfo.dat` contiene textos para pantalla de login (billboard, facciones y clases), aunque el contenido actual está mayormente en chino.
- Se generó `docs/login_context_en_us.json` para exponer esos textos en un formato consumible desde UI/servicios.

## 2) Arquitectura propuesta (100% Blueprint en frontend)

**Objetivo:** que el flujo de autenticación del launcher/menu sea operable desde Blueprints en Unreal.

### Componentes

1. **WBP_LoginScreen** (Widget principal)
   - Inputs: `Email/Usuario`, `Password`.
   - Botones: `Login`, `Crear cuenta`, `Olvidé contraseña`, `Salir`.
   - Estado visual: `Idle`, `Loading`, `Error`, `Success`.

2. **BP_LoginManager** (Actor o GameInstance Subsystem)
   - Variables:
     - `ApiBaseUrl` (String)
     - `SessionToken` (String)
     - `RefreshToken` (String)
     - `PlayerProfile` (Struct)
   - Eventos Blueprint:
     - `LoginRequested(User, Pass)`
     - `OnLoginSuccess(Profile)`
     - `OnLoginError(Code, Message)`

3. **Structs Blueprint**
   - `ST_LoginRequest` => `username`, `password`, `region`, `locale`, `client_version`.
   - `ST_LoginResponse` => `access_token`, `refresh_token`, `expires_in`, `player_id`, `nickname`, `faction`.

### Flujo Blueprint (nodos)

1. `OnClicked(LoginButton)`
2. `Branch` validando inputs no vacíos.
3. `Set UI State = Loading`
4. `Construct Json Object` (o VaRest si ya lo usan) con `ST_LoginRequest`.
5. `HTTP POST /auth/login`
6. `OnProcessRequestComplete`
   - `Branch(StatusCode == 200)`
   - **true**: parsear JSON -> guardar tokens -> `OnLoginSuccess` -> `Open Level (CharacterSelect)`
   - **false**: mapear error -> `OnLoginError` -> `Set UI State = Error`

## 3) Contrato recomendado de API (para backend)

### POST `/auth/login`

Request:

```json
{
  "username": "demo",
  "password": "secret",
  "region": 11,
  "locale": "en_us",
  "client_version": "gw_setup_2.48.001_usa"
}
```

Response 200:

```json
{
  "access_token": "jwt-or-session-token",
  "refresh_token": "refresh-token",
  "expires_in": 3600,
  "player_id": 1001,
  "nickname": "Ares",
  "faction": "Athens"
}
```

## 4) Checklist de implementación en Unreal

- [ ] Crear `WBP_LoginScreen` con bindings de estado.
- [ ] Crear `BP_LoginManager` en `GameInstance` para persistencia entre mapas.
- [ ] Implementar serialización/deserialización JSON en Blueprint.
- [ ] Implementar cola/retry (1 intento) para timeout de red.
- [ ] Guardar `RefreshToken` en `SaveGame` cifrado (si aplica).
- [ ] Añadir telemetría mínima: login success/fail y latencia.

## 5) Siguiente paso sugerido

Cuando compartas el repo Unreal (o carpeta `Content/`), te puedo devolver:

1. Estructura exacta de Blueprints (nombres de variables y pins).
2. Eventos y funciones por cada Blueprint.
3. Diagrama paso a paso de nodos para copiar/pegar manualmente.
