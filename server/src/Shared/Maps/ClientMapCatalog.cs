using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GWWWlogin.Shared.Maps;

public sealed class ClientMapCatalog : IClientMapCatalog
{
    private static readonly Regex SectionRegex = new(@"^\[(.+)\]$", RegexOptions.Compiled);
    private static readonly Regex CoordinateKeyRegex = new(@"^Coordinate(?<index>\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex AddressKeyRegex = new(@"^AddressName(?<index>\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    private static readonly Regex BomRegex = new("^\uFEFF", RegexOptions.Compiled);

    private readonly IReadOnlyList<ClientMapDefinition> _maps;

    public ClientMapCatalog()
    {
        _maps = BuildCatalog();
    }

    public IReadOnlyList<ClientMapDefinition> GetAll() => _maps;

    public ClientMapDefinition? GetById(int mapId) => _maps.SingleOrDefault(x => x.MapId == mapId);

    public ClientMapDefinition? GetStartingMap(string faction)
    {
        var normalizedFaction = faction.Trim().ToUpperInvariant();
        var preferredScene = normalizedFaction switch
        {
            "ATHENS" => "Athens_Newbie",
            "SPARTA" => "Sparta_Newbie",
            _ => null
        };

        return preferredScene is null
            ? null
            : _maps.SingleOrDefault(x => string.Equals(x.SceneName, preferredScene, StringComparison.OrdinalIgnoreCase));
    }

    private static IReadOnlyList<ClientMapDefinition> BuildCatalog()
    {
        var repoRoot = ResolveRepositoryRoot();
        var mapRoot = Path.Combine(repoRoot, "Gw Client", "Map");
        var monsterRoot = Path.Combine(repoRoot, "Gw Client", "Localization", "en_us", "Monster");
        var textRoot = Path.Combine(repoRoot, "Gw Client", "Localization", "en_us", "Text");
        var settingsRoot = Path.Combine(repoRoot, "Gw Client", "Localization", "en_us", "Settings", "Sys");

        if (!Directory.Exists(mapRoot))
        {
            return BuildFallbackCatalog();
        }

        var scenes = Directory.GetDirectories(mapRoot, "Terr*")
            .Select(Path.GetFileName)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x!["Terr".Length..])
            .OrderBy(x => x, StringComparer.OrdinalIgnoreCase)
            .ToList();

        var orderedScenes = new List<string>();
        if (scenes.Remove("Athens_Newbie")) orderedScenes.Add("Athens_Newbie");
        if (scenes.Remove("Sparta_Newbie")) orderedScenes.Add("Sparta_Newbie");
        orderedScenes.AddRange(scenes);

        var npcNames = LoadNpcNames(Path.Combine(textRoot, "NpcName.dat"));
        var npcsByScene = LoadNpcPositions(Path.Combine(settingsRoot, "Quest.xml"), npcNames, orderedScenes);

        var results = new List<ClientMapDefinition>();
        for (var index = 0; index < orderedScenes.Count; index++)
        {
            var sceneName = orderedScenes[index];
            var mapId = 1001 + index;
            var terrainFolder = Path.Combine("Gw Client", "Map", $"Terr{sceneName}");
            var monsterFolderPath = Path.Combine(monsterRoot, sceneName);
            var monsterFolder = Directory.Exists(monsterFolderPath)
                ? Path.Combine("Gw Client", "Localization", "en_us", "Monster", sceneName)
                : string.Empty;

            var addresses = Directory.Exists(monsterFolderPath)
                ? LoadAddresses(Path.Combine(monsterFolderPath, "Address.ini"))
                : [];
            var monsters = Directory.Exists(monsterFolderPath)
                ? LoadMonsters(Path.Combine(monsterFolderPath, "Monster.ini"))
                : [];
            var npcs = npcsByScene.TryGetValue(sceneName, out var sceneNpcs)
                ? sceneNpcs
                : [];

            var defaultSpawn = ResolveDefaultSpawn(sceneName, addresses);
            results.Add(new ClientMapDefinition(
                mapId,
                sceneName,
                terrainFolder,
                monsterFolder,
                defaultSpawn.PositionX,
                defaultSpawn.PositionY,
                addresses,
                npcs,
                monsters));
        }

        return results.Count > 0 ? results : BuildFallbackCatalog();
    }

    private static IReadOnlyList<ClientMapDefinition> BuildFallbackCatalog()
    {
        return
        [
            new ClientMapDefinition(1001, "Athens_Newbie", "Gw Client/Map/TerrAthens_Newbie", "Gw Client/Localization/en_us/Monster/Athens_Newbie", 102f, -217f, [], [], []),
            new ClientMapDefinition(1002, "Sparta_Newbie", "Gw Client/Map/TerrSparta_Newbie", "Gw Client/Localization/en_us/Monster/Sparta_Newbie", 102f, -217f, [], [], [])
        ];
    }

    private static Dictionary<string, IReadOnlyList<ClientNpcTemplate>> LoadNpcPositions(
        string questXmlPath,
        IReadOnlyDictionary<string, string> npcNames,
        IReadOnlyList<string> sceneNames)
    {
        if (!File.Exists(questXmlPath))
        {
            return new Dictionary<string, IReadOnlyList<ClientNpcTemplate>>(StringComparer.OrdinalIgnoreCase);
        }

        var sceneLookup = sceneNames.OrderByDescending(x => x.Length).ToList();
        var buckets = new Dictionary<string, Dictionary<string, ClientNpcTemplate>>(StringComparer.OrdinalIgnoreCase);

        var document = XDocument.Load(questXmlPath);
        foreach (var element in document.Root?.Elements() ?? Enumerable.Empty<XElement>())
        {
            RegisterNpc(element.Attribute("GiverName")?.Value, element.Attribute("Giver")?.Value);
            RegisterNpc(element.Attribute("ResponderName")?.Value, element.Attribute("Responder")?.Value);
        }

        return buckets.ToDictionary(
            x => x.Key,
            x => (IReadOnlyList<ClientNpcTemplate>)x.Value.Values.OrderBy(npc => npc.NpcKey, StringComparer.OrdinalIgnoreCase).ToList(),
            StringComparer.OrdinalIgnoreCase);

        void RegisterNpc(string? npcKey, string? rawCoordinate)
        {
            if (string.IsNullOrWhiteSpace(npcKey) || string.IsNullOrWhiteSpace(rawCoordinate))
            {
                return;
            }

            var sceneName = ResolveSceneNameFromNpcKey(npcKey, sceneLookup);
            if (sceneName is null)
            {
                return;
            }

            if (!TryParseQuestCoordinate(rawCoordinate, out var positionX, out var positionY))
            {
                return;
            }

            var displayName = npcNames.TryGetValue(npcKey, out var npcName) && !string.IsNullOrWhiteSpace(npcName)
                ? npcName
                : npcKey;

            if (!buckets.TryGetValue(sceneName, out var byKey))
            {
                byKey = new Dictionary<string, ClientNpcTemplate>(StringComparer.OrdinalIgnoreCase);
                buckets[sceneName] = byKey;
            }

            byKey.TryAdd(npcKey, new ClientNpcTemplate(npcKey, displayName, positionX, positionY));
        }
    }

    private static string? ResolveSceneNameFromNpcKey(string npcKey, IReadOnlyList<string> sceneNames)
    {
        return sceneNames.FirstOrDefault(scene =>
            npcKey.StartsWith(scene + "_", StringComparison.OrdinalIgnoreCase) ||
            npcKey.StartsWith(scene, StringComparison.OrdinalIgnoreCase));
    }

    private static bool TryParseQuestCoordinate(string rawCoordinate, out float positionX, out float positionY)
    {
        var parts = rawCoordinate.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2)
        {
            positionX = 0;
            positionY = 0;
            return false;
        }

        return TryParseFloat(parts[0], out positionX) && TryParseFloat(parts[1], out positionY);
    }

    private static IReadOnlyDictionary<string, string> LoadNpcNames(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        return ReadText(filePath)
            .Replace("\r", string.Empty, StringComparison.Ordinal)
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Select(line => line.Split('\t', 2, StringSplitOptions.TrimEntries))
            .Where(parts => parts.Length == 2)
            .ToDictionary(parts => parts[0], parts => parts[1], StringComparer.OrdinalIgnoreCase);
    }

    private static (float PositionX, float PositionY) ResolveDefaultSpawn(string sceneName, IReadOnlyList<ClientMapAddress> addresses)
    {
        var sceneToken = sceneName.Replace("_", " ", StringComparison.OrdinalIgnoreCase);
        var preferredAddress = addresses.FirstOrDefault(address =>
            string.Equals(address.Name, sceneName, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(address.Name, sceneToken, StringComparison.OrdinalIgnoreCase) ||
            (sceneName.StartsWith("Athens", StringComparison.OrdinalIgnoreCase) && address.Name.Contains("Athens", StringComparison.OrdinalIgnoreCase)) ||
            (sceneName.StartsWith("Sparta", StringComparison.OrdinalIgnoreCase) && address.Name.Contains("Sparta", StringComparison.OrdinalIgnoreCase)));

        if (preferredAddress is not null)
        {
            return (preferredAddress.PositionX, preferredAddress.PositionY);
        }

        var firstAddress = addresses.FirstOrDefault();
        return firstAddress is not null
            ? (firstAddress.PositionX, firstAddress.PositionY)
            : (128f, 128f);
    }

    private static IReadOnlyList<ClientMapAddress> LoadAddresses(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        var data = ParseIni(ReadText(filePath));
        if (!data.TryGetValue("0", out var primarySection) || primarySection.Count == 0)
        {
            primarySection = data.Values.FirstOrDefault(section => section.Count > 0) ?? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        var names = new Dictionary<int, string>();
        var coordinates = new Dictionary<int, (float X, float Y)>();

        foreach (var pair in primarySection)
        {
            var addressMatch = AddressKeyRegex.Match(pair.Key);
            if (addressMatch.Success && int.TryParse(addressMatch.Groups["index"].Value, out var addressIndex))
            {
                names[addressIndex] = pair.Value.Trim();
                continue;
            }

            var coordinateMatch = CoordinateKeyRegex.Match(pair.Key);
            if (coordinateMatch.Success && int.TryParse(coordinateMatch.Groups["index"].Value, out var coordinateIndex))
            {
                var parts = pair.Value.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && TryParseFloat(parts[0], out var x) && TryParseFloat(parts[1], out var y))
                {
                    coordinates[coordinateIndex] = (x, y);
                }
            }
        }

        return names
            .OrderBy(x => x.Key)
            .Where(x => coordinates.ContainsKey(x.Key))
            .Select(x => new ClientMapAddress(
                string.IsNullOrWhiteSpace(x.Value) ? $"Point {x.Key}" : x.Value,
                coordinates[x.Key].X,
                coordinates[x.Key].Y))
            .ToList();
    }

    private static IReadOnlyList<ClientMonsterTemplate> LoadMonsters(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return [];
        }

        var data = ParseIni(ReadText(filePath));
        return data
            .Where(section => !string.Equals(section.Key, "default", StringComparison.OrdinalIgnoreCase))
            .Select(section => new
            {
                MonsterKey = section.Key,
                Name = section.Value.TryGetValue("Name", out var displayName) ? displayName.Trim() : section.Key,
                MonsterFlag = section.Value.TryGetValue("Monster", out var monsterFlag) ? monsterFlag.Trim() : "0"
            })
            .Where(x => x.MonsterFlag == "1")
            .Select(x => new ClientMonsterTemplate(x.MonsterKey, string.IsNullOrWhiteSpace(x.Name) ? x.MonsterKey : x.Name))
            .DistinctBy(x => x.MonsterKey)
            .Take(8)
            .ToList();
    }

    private static Dictionary<string, Dictionary<string, string>> ParseIni(string content)
    {
        var result = new Dictionary<string, Dictionary<string, string>>(StringComparer.OrdinalIgnoreCase);
        var currentSection = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var currentSectionName = "default";
        result[currentSectionName] = currentSection;

        foreach (var rawLine in content.Replace("\r", string.Empty, StringComparison.Ordinal).Split('\n'))
        {
            var line = BomRegex.Replace(rawLine, string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith(';'))
            {
                continue;
            }

            var sectionMatch = SectionRegex.Match(line);
            if (sectionMatch.Success)
            {
                currentSectionName = sectionMatch.Groups[1].Value.Trim();
                currentSection = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                result[currentSectionName] = currentSection;
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim().TrimEnd(';');
            currentSection[key] = value;
        }

        return result;
    }

    private static bool TryParseFloat(string rawValue, out float value)
    {
        return float.TryParse(rawValue.Trim().TrimEnd('f', 'F'), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private static string ReadText(string filePath)
    {
        var bytes = File.ReadAllBytes(filePath);
        if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
        {
            return Encoding.Unicode.GetString(bytes);
        }

        return File.ReadAllText(filePath);
    }

    private static string ResolveRepositoryRoot()
    {
        var current = AppContext.BaseDirectory;
        var directory = new DirectoryInfo(current);
        while (directory is not null)
        {
            if (Directory.Exists(Path.Combine(directory.FullName, "Gw Client")))
            {
                return directory.FullName;
            }

            directory = directory.Parent;
        }

        return Directory.GetCurrentDirectory();
    }
}
