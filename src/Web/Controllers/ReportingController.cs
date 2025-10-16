using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize]
    public class ReportingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportingController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> InventoryStockReport()
        {
            var stockLevels = await _context.Materials
                .Select(m => new {
                    Material = m,
                    Quantity = _context.StockLedger
                                .Where(s => s.MaterialId == m.Id)
                                .OrderByDescending(s => s.MovementDate)
                                .Select(s => s.NewQuantity)
                                .FirstOrDefault()
                })
                .ToListAsync();

            return View(stockLevels);
        }

        public async Task<IActionResult> TrialBalance()
        {
            var trialBalance = await _context.GlEntries
                .GroupBy(g => g.Account)
                .Select(g => new {
                    Account = g.Key,
                    Balance = g.Sum(e => e.Amount)
                })
                .ToListAsync();

            return View(trialBalance);
        }

        public async Task<IActionResult> ZReport()
        {
            var today = DateTime.Today;
            var zReport = await _context.PosTransactions
                .Where(t => t.TransactionDate.Date == today)
                .Include(t => t.PaymentMethod)
                .GroupBy(t => t.PaymentMethod.Name)
                .Select(g => new {
                    PaymentMethod = g.Key,
                    Total = g.Sum(t => t.TotalAmount)
                })
                .ToListAsync();

            return View(zReport);
        }
    }
}