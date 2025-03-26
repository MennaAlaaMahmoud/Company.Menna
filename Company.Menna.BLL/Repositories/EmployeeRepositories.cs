using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.BLL.Interfaces;
using Company.Menna.DAL.Data.Context;
using Company.Menna.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Menna.BLL.Repositories
{
    public class EmployeeRepositories : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepositories(CompanyDbContext context) : base(context) // ASK CLR Create Object From CompanyDbContext
        {
            _context = context;
        }

        public List<Employee> GetByName(string name)
        {
            return _context.Employees.Include(E => E.Department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
