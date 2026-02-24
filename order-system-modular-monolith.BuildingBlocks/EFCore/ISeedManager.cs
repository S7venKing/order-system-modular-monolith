namespace order_system_modular_monolith.BuildingBlocks.EFCore;

public interface ISeedManager
{
    Task ExecuteSeedAsync();
    Task ExecuteTestSeedAsync();
}