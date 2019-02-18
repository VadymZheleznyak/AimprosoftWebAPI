using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AimprosoftWebAPI.Models;
using AimprosoftWebAPI.Interfaces;

namespace AimprosoftWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService<Employee> _service;

        public EmployeeController(IEmployeeService<Employee> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            var employee = await _service.Get(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (_service.CheckExisting(employee))
                return Ok(new { Error = "Employee with this email is already exist" });

            var result = await _service.Put(id, employee);

            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromBody] Employee employee)
        {
            if (_service.CheckExisting(employee))
                return Ok(new { Error = "Employee with this email is already exist" });

            await _service.Post(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var employee = await _service.Get(id);

            if (employee == null)
                return NotFound();

            await _service.Delete(employee);

            return Ok(employee);
        }

        [HttpPost("filter/{searchKey}")]
        public IActionResult PostEmployeeFilter([FromRoute] string searchKey, [FromBody] Pagination pagination)
        {
            var employees = new List<Employee>();

            if (!string.IsNullOrEmpty(searchKey))
                employees = _service.GetByKey(searchKey, pagination.PageNumber, pagination.PageSize);
            else
                employees = _service.GetForPaging(pagination.PageNumber, pagination.PageSize);

            if (employees.Count != 0)
            {
                pagination.TotalRecords = _service.GetCount();
                pagination.TotalPages = (int)Math.Ceiling(pagination.TotalRecords.Value / (double)pagination.PageSize);
            }

            return Ok(new { employees, pagination });
        }

        [HttpPost("index/{departmentId}")]
        public IActionResult PostIndexEmployees([FromRoute] int departmentId, [FromBody] Pagination pagination)
        {
            var employees = new List<Employee>();

            if (departmentId != 0)
                employees = _service.GetByRelationId(departmentId, pagination.PageNumber, pagination.PageSize);
            else
                employees = _service.GetForPaging(pagination.PageNumber, pagination.PageSize);

            pagination.TotalRecords = _service.GetCount();
            pagination.TotalPages = (int)Math.Ceiling(pagination.TotalRecords.Value / (double)pagination.PageSize);

            if (pagination.PageNumber > pagination.TotalPages)
                pagination.PageNumber = pagination.TotalPages.Value;

            return Ok(new { employees, pagination });
        }

        [HttpGet("count")]
        public IActionResult GetEmployeesCount() => Ok(new { Count = _service.GetCount() });
    }
}