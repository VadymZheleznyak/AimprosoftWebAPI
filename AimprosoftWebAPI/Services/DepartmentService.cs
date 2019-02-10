using AimprosoftWebAPI.Interfaces;
using AimprosoftWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Services
{
    public class DepartmentService : IService<Department>
    {
        private readonly DepartmentContext _context;

        public DepartmentService(DepartmentContext context)
        {
            _context = context;
        }

        public IEnumerable<Department> GetAll() => _context.Departments;

        public async Task<Department> Get(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            return department;
        }

        public async Task<bool> Put(int Id, Department department)
        {
            _context.Entry(department).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
            return true;
        }

        public async Task Post(Department department)
        {
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Department department)
        {
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        public List<Department> GetByKey(string searchKey) => _context.Departments.Where(x => x.Name.ToLower().Contains(searchKey)
                                                                                    || x.KindOfWork.ToLower().Contains(searchKey)
                                                                                    || x.City.ToLower().Contains(searchKey)).ToList();

        public List<Department> GetByRelationId(int id) => new List<Department>();

        public bool CheckExisting(Department department) => _context.Departments.Any(e => e.Name == department.Name);

        public int GetCount() => _context.Departments.Count();
    }
}
