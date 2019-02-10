using System.Collections.Generic;
using System.Linq;
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
        private readonly IService<Department> _service;

        public DepartmentController(IService<Department> service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<Department> GetDepartments() => _service.GetAll();

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = await _service.Get(id);

            if (department == null)
                return NotFound();

            return Ok(department);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDepartment([FromRoute] int id, [FromBody] Department department)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != department.Id)
                return BadRequest();

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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_service.CheckExisting(department))
                return Ok(new { Error = "Department with this name is already exist" });

            await _service.Post(department);

            return CreatedAtAction("GetDepartment", new { id = department.Id }, department);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var department = await _service.Get(id);

            if (department == null)
                return NotFound();

            await _service.Delete(department);

            return Ok(department);
        }


        [HttpPost("index")]
        public IActionResult PostDepartmentIndex(Pagination pagination)
        {
            var departments = _service.GetAll().ToList();
            pagination.TotalRecords = departments.Count;

            if (pagination.PageNumber > 1)
                departments = departments.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize).ToList();
            else
                departments = departments.Take(pagination.PageSize).ToList();

            pagination.TotalPages = (int)Math.Ceiling(departments.Count() / (double)pagination.PageSize);

            if (pagination.PageNumber > pagination.TotalPages)
                pagination.PageNumber = pagination.TotalPages.Value;

            return Ok(new { departments, pagination });
        }

        [HttpPost("filter/{searchKey}")]
        public IActionResult PostDepartmentFilter([FromRoute] string searchKey, [FromBody] Pagination pagination)
        {
            var departments = new List<Department>();

            if (!string.IsNullOrEmpty(searchKey))
                departments = _service.GetByKey(searchKey);
            else
                departments = _service.GetAll().ToList();

            if (departments.Count != 0)
            {
                pagination.TotalRecords = departments.Count;
                pagination.TotalPages = (int)Math.Ceiling(departments.Count() / (double)pagination.PageSize);
            }

            return Ok(new { departments, pagination });
        }

        [HttpGet("count")]
        public IActionResult GetDepartmentsCount() => Ok(new { Count = _service.GetCount() });
    }
}