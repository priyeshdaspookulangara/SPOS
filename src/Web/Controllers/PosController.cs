using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Services;
using System.Text.Json;

namespace Web.Controllers
{
    [Authorize] // Secure the controller
    public class PosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IInventoryService _inventoryService;

        public PosController(ApplicationDbContext context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Description");
            ViewData["PaymentMethodId"] = new SelectList(_context.PaymentMethods, "Id", "Name");
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMaterialPrice(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return NotFound();
            return Json(new { price = material.ListPrice });
        }

        [HttpGet]
        public async Task<IActionResult> CheckStock(int id)
        {
            var stock = await _inventoryService.GetStockLevelAsync(id);
            return Json(new { stock });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompleteSale([FromBody] PosTransaction transaction, [FromQuery] int customerId)
        {
            if (ModelState.IsValid)
            {
                transaction.TransactionDate = DateTime.Now;
                transaction.TotalAmount = transaction.Items.Sum(i => i.Quantity * i.UnitPrice);
                _context.PosTransactions.Add(transaction);

                var revenueAccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountNumber == "400000");
                var paymentMethod = await _context.PaymentMethods.FindAsync(transaction.PaymentMethodId);
                var paymentAccount = await _context.ChartOfAccounts.FindAsync(paymentMethod.GlAccountId);

                if (revenueAccount != null && paymentAccount != null)
                {
                    _context.GlEntries.Add(new GlEntry { AccountId = revenueAccount.Id, Amount = -transaction.TotalAmount, PostingDate = DateTime.Now, DocumentType = "DR", Description = $"POS Sale {transaction.Id}" });
                    _context.GlEntries.Add(new GlEntry { AccountId = paymentAccount.Id, Amount = transaction.TotalAmount, PostingDate = DateTime.Now, DocumentType = "DR", Description = $"POS Sale {transaction.Id}" });
                }

                var interaction = new InteractionHistory
                {
                    CustomerId = customerId,
                    InteractionDate = DateTime.Now,
                    InteractionType = "POS Transaction",
                    Channel = "In-Store",
                    Notes = $"POS Sale #{transaction.Id} for {transaction.TotalAmount:C}",
                    PosTransactionId = transaction.Id
                };
                _context.InteractionHistories.Add(interaction);

                foreach (var item in transaction.Items)
                {
                    var lastStock = await _context.StockLedger
                        .Where(s => s.MaterialId == item.MaterialId)
                        .OrderByDescending(s => s.MovementDate)
                        .FirstOrDefaultAsync();

                    var newStockLedgerEntry = new StockLedger
                    {
                        MaterialId = item.MaterialId,
                        QuantityChange = -item.Quantity,
                        NewQuantity = (lastStock?.NewQuantity ?? 0) - item.Quantity,
                        MovementType = "Goods Issue POS",
                        MovementDate = DateTime.Now
                    };
                    _context.StockLedger.Add(newStockLedgerEntry);
                }

                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest(ModelState);
        }
    }
}