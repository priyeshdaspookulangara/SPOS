using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize] // Secure the controller
    public class MasterDataController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MasterDataController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Vendor Actions
        public async Task<IActionResult> Vendors()
        {
            return View(await _context.Vendors.ToListAsync());
        }

        public IActionResult CreateVendor() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVendor([Bind("Name,Address,City,PostalCode,Country,PhoneNumber,Email")] Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vendor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Vendors));
            }
            return View(vendor);
        }

        public async Task<IActionResult> EditVendor(int? id)
        {
            if (id == null) return NotFound();
            var vendor = await _context.Vendors.FindAsync(id);
            if (vendor == null) return NotFound();
            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVendor(int id, [Bind("Id,Name,Address,City,PostalCode,Country,PhoneNumber,Email")] Vendor vendor)
        {
            if (id != vendor.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vendor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vendors.Any(e => e.Id == vendor.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Vendors));
            }
            return View(vendor);
        }

        public async Task<IActionResult> DetailsVendor(int? id)
        {
            if (id == null) return NotFound();
            var vendor = await _context.Vendors.FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null) return NotFound();
            return View(vendor);
        }

        public async Task<IActionResult> DeleteVendor(int? id)
        {
            if (id == null) return NotFound();
            var vendor = await _context.Vendors.FirstOrDefaultAsync(m => m.Id == id);
            if (vendor == null) return NotFound();
            return View(vendor);
        }

        [HttpPost, ActionName("DeleteVendor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVendorConfirmed(int id)
        {
            var vendor = await _context.Vendors.FindAsync(id);
            _context.Vendors.Remove(vendor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Vendors));
        }

        // Material Actions
        public async Task<IActionResult> Materials()
        {
            return View(await _context.Materials.ToListAsync());
        }

        public IActionResult CreateMaterial() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMaterial([Bind("Description,UnitOfMeasure,StandardCost,ListPrice")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Materials));
            }
            return View(material);
        }

        public async Task<IActionResult> EditMaterial(int? id)
        {
            if (id == null) return NotFound();
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return NotFound();
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMaterial(int id, [Bind("Id,Description,UnitOfMeasure,StandardCost,ListPrice")] Material material)
        {
            if (id != material.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Materials.Any(e => e.Id == material.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Materials));
            }
            return View(material);
        }

        public async Task<IActionResult> DetailsMaterial(int? id)
        {
            if (id == null) return NotFound();
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == id);
            if (material == null) return NotFound();
            return View(material);
        }

        public async Task<IActionResult> DeleteMaterial(int? id)
        {
            if (id == null) return NotFound();
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == id);
            if (material == null) return NotFound();
            return View(material);
        }

        [HttpPost, ActionName("DeleteMaterial")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMaterialConfirmed(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Materials));
        }

        // Chart of Accounts Actions
        public async Task<IActionResult> ChartOfAccounts()
        {
            return View(await _context.ChartOfAccounts.ToListAsync());
        }

        public IActionResult CreateChartOfAccounts() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChartOfAccounts([Bind("AccountNumber,Description,AccountType")] ChartOfAccounts account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ChartOfAccounts));
            }
            return View(account);
        }

        public async Task<IActionResult> EditChartOfAccounts(int? id)
        {
            if (id == null) return NotFound();
            var account = await _context.ChartOfAccounts.FindAsync(id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditChartOfAccounts(int id, [Bind("Id,AccountNumber,Description,AccountType")] ChartOfAccounts account)
        {
            if (id != account.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChartOfAccounts.Any(e => e.Id == account.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(ChartOfAccounts));
            }
            return View(account);
        }

        public async Task<IActionResult> DetailsChartOfAccounts(int? id)
        {
            if (id == null) return NotFound();
            var account = await _context.ChartOfAccounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null) return NotFound();
            return View(account);
        }

        public async Task<IActionResult> DeleteChartOfAccounts(int? id)
        {
            if (id == null) return NotFound();
            var account = await _context.ChartOfAccounts.FirstOrDefaultAsync(m => m.Id == id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost, ActionName("DeleteChartOfAccounts")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteChartOfAccountsConfirmed(int id)
        {
            var account = await _context.ChartOfAccounts.FindAsync(id);
            _context.ChartOfAccounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ChartOfAccounts));
        }
    }
}