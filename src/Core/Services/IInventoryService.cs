namespace Core.Services;

public interface IInventoryService
{
    Task<int> GetStockLevelAsync(int materialId);
}