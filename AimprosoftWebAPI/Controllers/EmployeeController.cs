using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IService<Employee> _service;

        public EmployeeController(IService<Employee> service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Employee> GetEmployees() => _service.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var employee = await _service.Get(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != employee.Id)
                return BadRequest();

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(_service.CheckExisting(employee))
                return Ok(new { Error = "Employee with this email is already exist" });

            await _service.Post(employee);

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
                employees = _service.GetByKey(searchKey);
            else
                employees = _service.GetAll().ToList();

            if (employees.Count != 0)
            {
                pagination.TotalRecords = employees.Count;
                pagination.TotalPages = (int)Math.Ceiling(employees.Count() / (double)pagination.PageSize);
            }

            return Ok(new { employees, pagination });
        }

        [HttpPost("index/{departmentId}")]
        public IActionResult PostIndexEmployees([FromRoute] int departmentId, [FromBody] Pagination pagination)
        {
            var employees = new List<Employee>();

            if (departmentId != 0)
                employees = _service.GetByRelationId(departmentId);
            else
                employees = _service.GetAll().ToList();

            pagination.TotalRecords = employees.Count;

            if (pagination.PageNumber > 1)
                employees = employees.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize).ToList();
            else
                employees = employees.Take(pagination.PageSize).ToList();

            pagination.TotalPages = (int)Math.Ceiling(employees.Count() / (double)pagination.PageSize);

            if (pagination.PageNumber > pagination.TotalPages)
                pagination.PageNumber = pagination.TotalPages.Value;

            return Ok(new { employees, pagination });
        }

        [HttpGet("count")]
        public IActionResult GetEmployeesCount() => Ok(new { Count = _service.GetCount() });
    }
}