using AimprosoftWebAPI.Interfaces;
using AimprosoftWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Services
{
    public class DepartmentService : IDepartmentService<Department>
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

        public List<Department> GetByKey(string searchKey, int pageNumber, int pageSize) => _context.Departments.Where(x => x.Name.ToLower()
                                                                                    .Contains(searchKey) || x.KindOfWork.ToLower()
                                                                                    .Contains(searchKey) || x.City.ToLower().Contains(searchKey))
                                                                                    .Skip((pageNumber - 1) *pageSize).Take(pageSize).ToList();

        public bool CheckExisting(Department department) => _context.Departments.Any(d => d.Name == department.Name && d.Id != department.Id);

        public List<Department> GetForPaging(int pageNumber, int pageSize) => _context.Departments.Skip((pageNumber - 1) *
                                                                                        pageSize).Take(pageSize).ToList();

        public int GetCount() => _context.Departments.Count();
    }
}
