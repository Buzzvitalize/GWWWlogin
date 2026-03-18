namespace GWWWlogin.GatewayService.Definitions;

public interface IMapDefinitionService
{
    MapDefinition? GetById(int id);

    IReadOnlyList<MapDefinition> GetAll();
}
