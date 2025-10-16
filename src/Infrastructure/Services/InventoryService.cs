using Core.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class InventoryService : IInventoryService
{
    private readonly ApplicationDbContext _context;

    public InventoryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetStockLevelAsync(int materialId)
    {
        var lastStock = await _context.StockLedger
            .Where(s => s.MaterialId == materialId)
            .OrderByDescending(s => s.MovementDate)
            .FirstOrDefaultAsync();

        return lastStock?.NewQuantity ?? 0;
    }
}