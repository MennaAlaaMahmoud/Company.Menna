using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.BLL.Interfaces;
using Company.Menna.DAL.Data.Context;
using Company.Menna.DAL.Models;

namespace Company.Menna.BLL.Repositories
{
    public class EmployeeRepositories : IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepositories(CompanyDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }
        public Employee? Get(int id)
        {
            return _context.Employees.Find(id);
        }

        public int Add(Employee model)
        {
            _context.Employees.Add(model);
            return _context.SaveChanges();
        }

        public int Update(Employee model)
        {
            _context.Employees.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(Employee model)
        {
            _context.Employees.Remove(model);
            return _context.SaveChanges();
        }

       
      
     
    }
}
