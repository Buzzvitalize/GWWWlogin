namespace GWWWlogin.Shared.Maps;

public interface IClientMapCatalog
{
    IReadOnlyList<ClientMapDefinition> GetAll();

    ClientMapDefinition? GetById(int mapId);

    ClientMapDefinition? GetStartingMap(string faction);
}
