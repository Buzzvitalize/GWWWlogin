# Localization and map guide

Esta guía toma como base el cliente local subido al repositorio y resume cómo convertir esa información en datos útiles para el servidor.

## Clases confirmadas desde el cliente

Detectadas en `Gw Client/Localization/en_us/Text/Message.dat`:

- Warrior
- Champion
- Priest
- Mage

## Facciones iniciales recomendadas

Para el arranque del proyecto clean-room se recomienda usar:

- Athens -> `Athens_Newbie`
- Sparta -> `Sparta_Newbie`

## Mapas iniciales sugeridos

### Athens_Newbie
- escena: `Athens_Newbie`
- terreno: `Gw Client/Map/TerrAthens_Newbie`
- monstruos: `Gw Client/Localization/en_us/Monster/Athens_Newbie`
- monster address local adicional: `Gw Client/Map/Monster/Athens_Newbie/Address.ini`

### Sparta_Newbie
- escena: `Sparta_Newbie`
- terreno: `Gw Client/Map/TerrSparta_Newbie`
- monstruos: `Gw Client/Localization/en_us/Monster/Sparta_Newbie`
- monster address local adicional: `Gw Client/Map/Monster/Sparta_Newbie/Address.ini`

## Archivos clave por categoría

### Escenas
- `Gw Client/Localization/en_us/Text/Scenes.dat`

### NPCs
- `Gw Client/Localization/en_us/Text/NpcName.dat`
- `Gw Client/Localization/en_us/Text/NPC.dat`
- `Gw Client/Localization/en_us/Text/NPCDescription.dat`

### Monstruos
- `Gw Client/Localization/en_us/Monster/<Mapa>/Monster.ini`
- `Gw Client/Localization/en_us/Monster/<Mapa>/Address.ini`

### Mapas
- `Gw Client/Map/*.hmp`
- `Gw Client/Map/Terr*/Terr*.ini`
- `Gw Client/Map/Monster/<Mapa>/Address.ini`

## Cómo usar esta guía en el siguiente paso

1. elegir una escena interna real del cliente;
2. asignarla a una facción o flujo de entrada;
3. poblarla con NPCs base;
4. poblarla con monstruos base;
5. emitir broadcast de entrada, salida y movimiento usando esa escena.
