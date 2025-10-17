using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Web.Controllers
{
    [Authorize]
    public class CrmController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CrmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Customers()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer([Bind("Name,CustomerType,Email,PhoneNumber")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Customers));
            }
            return View(customer);
        }

        public async Task<IActionResult> EditCustomer(int? id)
        {
            if (id == null) return NotFound();
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCustomer(int id, [Bind("Id,Name,CustomerType,Email,PhoneNumber")] Customer customer)
        {
            if (id != customer.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Customers.Any(e => e.Id == customer.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Customers));
            }
            return View(customer);
        }

        public async Task<IActionResult> DetailsCustomer(int? id)
        {
            if (id == null) return NotFound();
            var customer = await _context.Customers
                .Include(c => c.Addresses)
                .Include(c => c.Contacts)
                .Include(c => c.Interactions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        public async Task<IActionResult> DeleteCustomer(int? id)
        {
            if (id == null) return NotFound();
            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null) return NotFound();
            return View(customer);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomerConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Customers));
        }

        // Lead Actions
        public async Task<IActionResult> Leads()
        {
            return View(await _context.Leads.ToListAsync());
        }

        public IActionResult CreateLead()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLead([Bind("FirstName,LastName,Company,Email,PhoneNumber,Status,Source")] Lead lead)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lead);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Leads));
            }
            return View(lead);
        }

        public async Task<IActionResult> EditLead(int? id)
        {
            if (id == null) return NotFound();
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return NotFound();
            return View(lead);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditLead(int id, [Bind("Id,FirstName,LastName,Company,Email,PhoneNumber,Status,Source")] Lead lead)
        {
            if (id != lead.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lead);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Leads.Any(e => e.Id == lead.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Leads));
            }
            return View(lead);
        }

        public async Task<IActionResult> DeleteLead(int? id)
        {
            if (id == null) return NotFound();
            var lead = await _context.Leads.FirstOrDefaultAsync(m => m.Id == id);
            if (lead == null) return NotFound();
            return View(lead);
        }

        [HttpPost, ActionName("DeleteLead")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLeadConfirmed(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Leads));
        }

        // Opportunity Actions
        public async Task<IActionResult> Opportunities()
        {
            return View(await _context.Opportunities.Include(o => o.Customer).ToListAsync());
        }

        public IActionResult CreateOpportunity()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOpportunity([Bind("Name,CustomerId,EstimatedValue,CloseDate,Stage")] Opportunity opportunity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opportunity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Opportunities));
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "Id", "Name", opportunity.CustomerId);
            return View(opportunity);
        }

        // Interaction History Actions
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddInteraction(int customerId, string interactionType, string channel, string notes)
        {
            var interaction = new InteractionHistory
            {
                CustomerId = customerId,
                InteractionDate = DateTime.Now,
                InteractionType = interactionType,
                Channel = channel,
                Notes = notes
            };
            _context.InteractionHistories.Add(interaction);
            await _context.SaveChangesAsync();
            return RedirectToAction("DetailsCustomer", new { id = customerId });
        }
    }
}