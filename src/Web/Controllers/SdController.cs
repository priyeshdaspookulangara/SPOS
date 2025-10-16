using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Core.Services;

namespace Web.Controllers
{
    [Authorize] // Secure the controller
    public class SdController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IInventoryService _inventoryService;

        public SdController(ApplicationDbContext context, IInventoryService inventoryService)
        {
            _context = context;
            _inventoryService = inventoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Sales Order Actions
        public async Task<IActionResult> SalesOrders()
        {
            var salesOrders = _context.SalesOrders.Include(s => s.Customer);
            return View(await salesOrders.ToListAsync());
        }

        public IActionResult CreateSalesOrder()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSalesOrder(SalesOrder salesOrder, int MaterialId, int Quantity)
        {
            if (ModelState.IsValid)
            {
                var stockLevel = await _inventoryService.GetStockLevelAsync(MaterialId);
                if (stockLevel < Quantity)
                {
                    ModelState.AddModelError("", "Not enough stock available.");
                    ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", salesOrder.CustomerId);
                    ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Description", MaterialId);
                    return View(salesOrder);
                }

                var material = await _context.Materials.FindAsync(MaterialId);
                var salesOrderItem = new SalesOrderItem
                {
                    MaterialId = MaterialId,
                    Quantity = Quantity,
                    UnitPrice = material.ListPrice
                };
                salesOrder.Items.Add(salesOrderItem);
                salesOrder.TotalAmount = salesOrderItem.Quantity * salesOrderItem.UnitPrice;
                salesOrder.OrderDate = DateTime.Now;

                _context.Add(salesOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(SalesOrders));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", salesOrder.CustomerId);
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Description", MaterialId);
            return View(salesOrder);
        }

        // Delivery Actions
        public async Task<IActionResult> Deliveries()
        {
            var deliveries = _context.Deliveries.Include(d => d.SalesOrder);
            return View(await deliveries.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDelivery(int id)
        {
            var salesOrder = await _context.SalesOrders.Include(s => s.Items).ThenInclude(i => i.Material).FirstOrDefaultAsync(s => s.Id == id);
            if (salesOrder == null) return NotFound();

            var delivery = new Delivery
            {
                SalesOrderId = salesOrder.Id,
                DeliveryDate = DateTime.Now,
                Status = "Picking"
            };
            _context.Add(delivery);

            foreach (var item in salesOrder.Items)
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
                    MovementType = "Goods Issue SO",
                    MovementDate = DateTime.Now,
                };
                _context.StockLedger.Add(newStockLedgerEntry);
            }

            salesOrder.Status = "Delivered";
            _context.Update(salesOrder);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Deliveries));
        }

        // Billing Actions
        public async Task<IActionResult> BillingDocuments()
        {
            var billingDocuments = _context.BillingDocuments.Include(b => b.Delivery);
            return View(await billingDocuments.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBillingDocument(int id)
        {
            var delivery = await _context.Deliveries.Include(d => d.SalesOrder).ThenInclude(s => s.Items).FirstOrDefaultAsync(d => d.Id == id);
            if (delivery == null) return NotFound();

            var totalAmount = delivery.SalesOrder.TotalAmount;
            var billingDocument = new BillingDocument
            {
                DeliveryId = delivery.Id,
                BillingDate = DateTime.Now,
                TotalAmount = totalAmount
            };
            _context.Add(billingDocument);

            // Financial Posting
            var revenueAccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountNumber == "400000"); // Assuming 400000 is Revenue
            var arAccount = await _context.ChartOfAccounts.FirstOrDefaultAsync(a => a.AccountNumber == "120000"); // Assuming 120000 is Accounts Receivable

            if (revenueAccount != null && arAccount != null)
            {
                // Credit Revenue
                _context.GlEntries.Add(new GlEntry { AccountId = revenueAccount.Id, Amount = -totalAmount, PostingDate = DateTime.Now, DocumentType = "RV", Description = $"Revenue for invoice {billingDocument.Id}" });
                // Debit Accounts Receivable
                _context.GlEntries.Add(new GlEntry { AccountId = arAccount.Id, Amount = totalAmount, PostingDate = DateTime.Now, DocumentType = "RV", Description = $"A/R for invoice {billingDocument.Id}" });
            }

            delivery.SalesOrder.Status = "Invoiced";
            _context.Update(delivery.SalesOrder);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BillingDocuments));
        }
    }
}