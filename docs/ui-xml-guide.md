# UI XML and Lua guide

Esta guía resume qué archivos del cliente ayudan a modelar login, selección de personaje, creación de personaje y navegación inicial de interfaz.

## Archivos clave detectados

### Login y creación de personaje
- `Gw Client/Localization/en_us/UI/XML/LoginUI.xml`
- `Gw Client/Localization/en_us/UI/XML/LoginUI.lua`

En `LoginUI.xml` aparecen elementos útiles para inferir flujo de creación de personaje, lista, selección y fondos de facción/login.

## Pistas útiles vistas en el cliente

### Clases
El cliente usa estas etiquetas para la construcción de clase:
- `Warriorbuild`
- `Spearmanbuild` -> Champion
- `Flamenbuild` -> Priest
- `Magebuild`

### Fondos/facciones
`LoginUI.xml` contiene referencias visuales separadas para:
- `Sparta.gwo`
- `Athens.gwo`

Eso encaja bien con nuestro flujo actual por facción:
- Athens -> `Athens_Newbie`
- Sparta -> `Sparta_Newbie`

## Otros archivos útiles para guiarnos

### Mapas e interfaz de mapa
- `Gw Client/Localization/en_us/UI/XML/map.lua`
- `Gw Client/Localization/en_us/UI/XML/MiniMap.xml`
- `Gw Client/Localization/en_us/UI/XML/MiniMapProc.lua`
- `Gw Client/Localization/en_us/UI/XML/WorldMapSetUI.xml`
- `Gw Client/Localization/en_us/UI/XML/GodwardMapUI.xml`

### NPCs y diálogos
- `Gw Client/Localization/en_us/UI/XML/NpcTalk.xml`
- `Gw Client/Localization/en_us/UI/XML/NpcFun.xml`
- `Gw Client/Localization/en_us/UI/XML/NpcFunLoad.xml`
- `Gw Client/Localization/en_us/UI/XML/NpcFun/*.lua`

### Guías del cliente
- `Gw Client/Localization/en_us/UI/XML/HelpSystem.xml`
- `Gw Client/Localization/en_us/UI/XML/HelpSystemConfig.lua`
- `Gw Client/Localization/en_us/UI/XML/RookieGuide.xml`
- `Gw Client/Localization/en_us/UI/XML/RookieGuideProc.lua`

## Cómo usar esta guía en el proyecto

1. usar `LoginUI.xml` para afinar flujo de login/selección/creación;
2. usar `Message.dat` para nombres reales de clase;
3. usar `Scenes.dat` + `Map/` para escenas reales;
4. usar `NpcName.dat` + `NpcFun/*.lua` para poblar NPCs y acciones;
5. usar `MiniMap` y `map.lua` para futuros comandos de navegación y broadcast.
