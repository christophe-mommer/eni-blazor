using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorAppShared.Models;
using BlazorServerAPI.Data;

namespace BlazorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public EmployeesController(ApiDbContext context)
        {
            _context = context;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Address.Country)
                .ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(Guid id)
        {
            var employee = await _context.Employees
                .Include(e => e.Job)
                .Include(e => e.Address.Country)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(Guid id, Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            await MapDbObjects(employee);
            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee)
        {
            await MapDbObjects(employee);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(Guid id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        private async Task MapDbObjects(Employee employee)
        {
            if (employee.Address?.Country != null)
            {
                var dbCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == employee.Address.Country.Id);
                if (dbCountry != null)
                {
                    employee.Address.Country = dbCountry;
                }
            }
            if (employee.Job != null)
            {
                var dbJob = await _context.Jobs.FirstOrDefaultAsync(j => j.Id == employee.Job.Id);
                if (dbJob != null)
                {
                    employee.Job = dbJob;
                }
            }
        }
    }
}
