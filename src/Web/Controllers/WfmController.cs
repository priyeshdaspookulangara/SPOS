using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.Controllers
{
    [Authorize] // Secure the controller
    public class WfmController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WfmController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // Employee Actions
        public async Task<IActionResult> Employees()
        {
            var employees = _context.Employees.Include(e => e.Department).Include(e => e.PayGrade);
            return View(await employees.ToListAsync());
        }

        public IActionResult CreateEmployee()
        {
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "Id", "Grade");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEmployee([Bind("FirstName,LastName,Email,PhoneNumber,DateOfBirth,DateOfJoining,DepartmentId,PayGradeId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Employees));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "Id", "Grade", employee.PayGradeId);
            return View(employee);
        }

        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound();
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "Id", "Grade", employee.PayGradeId);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditEmployee(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,DateOfBirth,DateOfJoining,DepartmentId,PayGradeId")] Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Employees.Any(e => e.Id == employee.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Employees));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            ViewData["PayGradeId"] = new SelectList(_context.PayGrades, "Id", "Grade", employee.PayGradeId);
            return View(employee);
        }

        public async Task<IActionResult> DetailsEmployee(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.PayGrade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        public async Task<IActionResult> DeleteEmployee(int? id)
        {
            if (id == null) return NotFound();
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.PayGrade)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("DeleteEmployee")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Employees));
        }

        // Leave Request Actions
        public async Task<IActionResult> LeaveRequests()
        {
            var leaveRequests = _context.LeaveRequests.Include(l => l.Employee);
            return View(await leaveRequests.ToListAsync());
        }

        public IActionResult CreateLeaveRequest()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLeaveRequest([Bind("EmployeeId,StartDate,EndDate,LeaveType,Reason")] LeaveRequest leaveRequest)
        {
            if (ModelState.IsValid)
            {
                leaveRequest.DateRequested = DateTime.Now;
                leaveRequest.Status = "Pending";
                _context.Add(leaveRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(LeaveRequests));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Email", leaveRequest.EmployeeId);
            return View(leaveRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null) return NotFound();
            leaveRequest.Status = "Approved";
            _context.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LeaveRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);
            if (leaveRequest == null) return NotFound();
            leaveRequest.Status = "Rejected";
            _context.Update(leaveRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(LeaveRequests));
        }
    }
}