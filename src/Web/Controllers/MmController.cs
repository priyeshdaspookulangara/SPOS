using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize] // Secure the controller
    public class MmController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Purchase Requisition Actions
        public async Task<IActionResult> PurchaseRequisitions()
        {
            var purchaseRequisitions = _context.PurchaseRequisitions.Include(p => p.Material);
            return View(await purchaseRequisitions.ToListAsync());
        }

        public IActionResult CreatePurchaseRequisition()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchaseRequisition([Bind("MaterialId,Quantity")] PurchaseRequisition purchaseRequisition)
        {
            if (ModelState.IsValid)
            {
                purchaseRequisition.DateRequested = DateTime.Now;
                purchaseRequisition.Status = "Pending";
                _context.Add(purchaseRequisition);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PurchaseRequisitions));
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "Id", "Description", purchaseRequisition.MaterialId);
            return View(purchaseRequisition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePurchaseRequisition(int id)
        {
            var purchaseRequisition = await _context.PurchaseRequisitions.FindAsync(id);
            if (purchaseRequisition == null) return NotFound();
            purchaseRequisition.Status = "Approved";
            _context.Update(purchaseRequisition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PurchaseRequisitions));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPurchaseRequisition(int id)
        {
            var purchaseRequisition = await _context.PurchaseRequisitions.FindAsync(id);
            if (purchaseRequisition == null) return NotFound();
            purchaseRequisition.Status = "Rejected";
            _context.Update(purchaseRequisition);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PurchaseRequisitions));
        }

        // Purchase Order Actions
        public async Task<IActionResult> PurchaseOrders()
        {
            var purchaseOrders = _context.PurchaseOrders.Include(p => p.Material).Include(p => p.Vendor);
            return View(await purchaseOrders.ToListAsync());
        }

        public async Task<IActionResult> CreatePurchaseOrderFromRequisition(int id)
        {
            var purchaseRequisition = await _context.PurchaseRequisitions.Include(p => p.Material).FirstOrDefaultAsync(p => p.Id == id);
            if (purchaseRequisition == null) return NotFound();

            var purchaseOrder = new PurchaseOrder
            {
                MaterialId = purchaseRequisition.MaterialId,
                Quantity = purchaseRequisition.Quantity,
                UnitPrice = purchaseRequisition.Material.StandardCost,
                OrderDate = DateTime.Now
            };
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Name");
            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePurchaseOrderFromRequisition(int id, [Bind("VendorId,MaterialId,Quantity,UnitPrice")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                purchaseOrder.OrderDate = DateTime.Now;
                _context.Add(purchaseOrder);

                var purchaseRequisition = await _context.PurchaseRequisitions.FindAsync(id);
                if (purchaseRequisition != null)
                {
                    purchaseRequisition.Status = "Ordered";
                    _context.Update(purchaseRequisition);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(PurchaseOrders));
            }
            ViewData["VendorId"] = new SelectList(_context.Vendors, "Id", "Name", purchaseOrder.VendorId);
            return View(purchaseOrder);
        }

        // Goods Receipt
        public async Task<IActionResult> GoodsReceipt(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.Include(p => p.Material).FirstOrDefaultAsync(p => p.Id == id);
            if (purchaseOrder == null) return NotFound();
            return View(purchaseOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GoodsReceipt(int id, int quantity)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            if (purchaseOrder == null) return NotFound();

            var lastStock = await _context.StockLedger
                .Where(s => s.MaterialId == purchaseOrder.MaterialId)
                .OrderByDescending(s => s.MovementDate)
                .FirstOrDefaultAsync();

            var newStockLedgerEntry = new StockLedger
            {
                MaterialId = purchaseOrder.MaterialId,
                QuantityChange = quantity,
                NewQuantity = (lastStock?.NewQuantity ?? 0) + quantity,
                MovementType = "Goods Receipt PO",
                MovementDate = DateTime.Now,
                PurchaseOrderId = purchaseOrder.Id
            };

            _context.StockLedger.Add(newStockLedgerEntry);

            purchaseOrder.Status = "Closed";
            _context.Update(purchaseOrder);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(PurchaseOrders));
        }
    }
}