using GWWWlogin.GatewayService.Models;

namespace GWWWlogin.GatewayService.Protocols;

public static class GatewayProtocolSerializer
{
    public const string HandshakeVersion = "v2";
    public const string WorldEventVersion = "world-v1";
    public const string AroundVersion = "around-v1";

    public static string FormatHandshake(
        Guid accountId,
        GatewayCharacterRecord character,
        float visibilityRadius,
        string worldEventVersion)
    {
        return string.Join('|',
            "HANDSHAKE_OK",
            $"version={HandshakeVersion}",
            $"accountId={accountId}",
            $"characterId={character.Id}",
            $"name={character.Name}",
            $"class={character.Class}",
            $"gender={character.Gender}",
            $"faction={character.Faction}",
            $"level={character.Level}",
            $"scene={character.SceneName}",
            $"mapId={character.MapId}",
            $"positionX={character.PositionX}",
            $"positionY={character.PositionY}",
            $"visibilityRadius={visibilityRadius}",
            $"eventSchema={worldEventVersion}");
    }

    public static string FormatWorldEvents(IEnumerable<MapBroadcastEvent> events)
    {
        var payload = string.Join('|', events.Select(FormatWorldEvent));
        return string.IsNullOrWhiteSpace(payload)
            ? $"EVENTS|version={WorldEventVersion}"
            : $"EVENTS|version={WorldEventVersion}|{payload}";
    }

    public static string FormatAround(
        IEnumerable<PlayerPresence> players,
        IEnumerable<NpcSpawnState> npcs,
        IEnumerable<MonsterSpawnState> monsters)
    {
        var segments = new List<string> { $"AROUND|version={AroundVersion}" };
        segments.AddRange(players.Select(FormatAroundPlayer));
        segments.AddRange(npcs.Select(FormatAroundNpc));
        segments.AddRange(monsters.Select(FormatAroundMonster));
        return string.Join('|', segments);
    }

    private static string FormatWorldEvent(MapBroadcastEvent mapEvent)
    {
        return string.Join(',',
            mapEvent.SequenceId,
            mapEvent.EventType,
            mapEvent.EntityKind,
            mapEvent.EntityInstanceId,
            mapEvent.CharacterName,
            mapEvent.PositionX,
            mapEvent.PositionY,
            mapEvent.SceneName);
    }

    private static string FormatAroundPlayer(PlayerPresence player)
    {
        return string.Join(',',
            "P",
            player.CharacterId,
            player.CharacterName,
            player.Faction,
            player.Level,
            player.PositionX,
            player.PositionY,
            player.SceneName);
    }

    private static string FormatAroundNpc(NpcSpawnState npc)
    {
        return string.Join(',',
            "N",
            npc.InstanceId,
            npc.NpcKey,
            npc.DisplayName,
            npc.ZoneKey,
            npc.PositionX,
            npc.PositionY,
            npc.SceneName,
            npc.LastUpdatedAtUtc.ToString("O"));
    }

    private static string FormatAroundMonster(MonsterSpawnState monster)
    {
        return string.Join(',',
            "M",
            monster.InstanceId,
            monster.MonsterKey,
            monster.DisplayName,
            monster.ZoneKey,
            monster.PositionX,
            monster.PositionY,
            monster.IsAlive ? "alive" : "down",
            monster.SceneName,
            monster.LastUpdatedAtUtc.ToString("O"));
    }
}
