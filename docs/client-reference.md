# Client reference notes

Esta guía resume rutas útiles del cliente local ya subido al repositorio para seguir el proyecto con más fidelidad.

## Clases detectadas

En `Gw Client/Localization/en_us/Text/Message.dat` aparecen estas clases base del cliente:

- Warrior
- Champion
- Priest
- Mage

## Archivos importantes para adaptar el servidor

### Escenas y nombres de mapas
- `Gw Client/Localization/en_us/Text/Scenes.dat`
- `Gw Client/Map/*.hmp`
- `Gw Client/Map/Terr*/Terr*.ini`

### NPCs y nombres visibles
- `Gw Client/Localization/en_us/Text/NpcName.dat`
- `Gw Client/Localization/en_us/Text/NPC.dat`
- `Gw Client/Localization/en_us/Text/NPCDescription.dat`

### Monstruos por mapa
- `Gw Client/Localization/en_us/Monster/<Mapa>/Monster.ini`
- `Gw Client/Localization/en_us/Monster/<Mapa>/Address.ini`

### Skills, mensajes y quests
- `Gw Client/Localization/en_us/Text/Skill.dat`
- `Gw Client/Localization/en_us/Text/SkillInfo.dat`
- `Gw Client/Localization/en_us/Text/PassiveSkillInfo.dat`
- `Gw Client/Localization/en_us/Text/Message.dat`
- `Gw Client/Localization/en_us/Text/Quest.dat`
- `Gw Client/Localization/en_us/Text/QuestMonster.dat`

## Mapas detectados en el cliente

Ejemplos visibles al revisar `Scenes.dat` y `Map/`:

- Athens
- Athens_Newbie
- Sparta
- Sparta_Newbie
- Marathon_All
- Parnitha_1
- Parnitha_2
- Nemea_1
- Nemea_2
- Olympia_All
- Thebes_All
- Atlantis_Entrance
- Tartarus

## Recomendación para el siguiente bloque

Cuando pasemos a entidades reales del mapa, tomar primero un mapa simple del cliente, por ejemplo:

1. `Athens`
2. `Athens_Newbie`
3. `Sparta_Newbie`
4. `Nemea_1`

Con eso podremos mapear:
- nombre interno de escena;
- NPCs base;
- monstruos base;
- punto de entrada;
- comandos iniciales del mapa.
