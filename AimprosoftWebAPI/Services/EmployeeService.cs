﻿using AimprosoftWebAPI.Interfaces;
using AimprosoftWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AimprosoftWebAPI.Services
{
    public class EmployeeService : IService<Employee>
    {
        private readonly DepartmentContext _context;

        public EmployeeService(DepartmentContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll() => _context.Employees;

        public async Task<Employee> Get(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            return employee;
        }

        public async Task<bool> Put(int Id, Employee department)
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

        public async Task Post(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Employee employee)
        {
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        public List<Employee> GetByKey(string searchKey) => _context.Employees.Where(x => x.FirstName.ToLower().Contains(searchKey)
                                            || x.LastName.ToLower().Contains(searchKey)
                                            || x.Email.ToLower().Contains(searchKey)
                                            || x.Salary.ToString().Contains(searchKey)
                                            || x.JobStartDate.Year.ToString().Contains(searchKey)
                                            || x.JobStartDate.Month.ToString().Contains(searchKey)
                                            || x.JobStartDate.Day.ToString().Contains(searchKey)).ToList();

        public List<Employee> GetByRelationId(int id) => _context.Employees.Where(x => x.DepartmentId == id).ToList();

        public bool CheckExisting(Employee employee) => _context.Employees.Any(e => e.Email == employee.Email);

        public int GetCount() => _context.Employees.Count();
    }
}
