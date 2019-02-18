using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AimprosoftWebAPI.Models;
using AimprosoftWebAPI.Interfaces;
using System;

namespace AimprosoftWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService<Department> _service;

        public DepartmentController(IDepartmentService<Department> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment([FromRoute] int id)
        {
            var department = await _service.Get(id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment([FromRoute] int id, [FromBody] Department department)
        {
            if (_service.CheckExisting(department))
                return Ok(new { Error = "Department with this name is already exist" });

            var result = await _service.Put(id, department);

            if (result)
                return NoContent();
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostDepartment([FromBody] Department department)
        {
            if (_service.CheckExisting(department))
                return Ok(new { Error = "Department with this name is already exist" });

            await _service.Post(department);

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            var department = await _service.Get(id);

            if (department == null)
                return NotFound();

            await _service.Delete(department);

            return Ok(department);
        }


        [HttpPost("index")]
        public IActionResult PostDepartmentIndex(Pagination pagination)
        {
            var departments = _service.GetForPaging(pagination.PageNumber, pagination.PageSize);
            pagination.TotalRecords = _service.GetCount();
            pagination.TotalPages = (int)Math.Ceiling(pagination.TotalRecords.Value / (double)pagination.PageSize);

            if (pagination.PageNumber > pagination.TotalPages)
                pagination.PageNumber = pagination.TotalPages.Value;

            return Ok(new { departments, pagination });
        }

        [HttpPost("filter/{searchKey}")]
        public IActionResult PostDepartmentFilter([FromRoute] string searchKey, [FromBody] Pagination pagination)
        {
            var departments = new List<Department>();

            if (!string.IsNullOrEmpty(searchKey))
                departments = _service.GetByKey(searchKey, pagination.PageNumber, pagination.PageSize);
            else
                departments = _service.GetForPaging(pagination.PageNumber, pagination.PageSize);

            if (departments.Count != 0)
            {
                pagination.TotalRecords = _service.GetCount();
                pagination.TotalPages = (int)Math.Ceiling(pagination.TotalRecords.Value / (double)pagination.PageSize);
            }

            return Ok(new { departments, pagination });
        }

        [HttpGet("count")]
        public IActionResult GetDepartmentsCount() => Ok(new { Count = _service.GetCount() });
    }
}