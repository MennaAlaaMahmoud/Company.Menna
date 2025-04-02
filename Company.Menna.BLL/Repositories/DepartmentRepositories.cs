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
    public class DepartmentRepositories : GenericRepository<Department>, IDepartmentRepositories
    {
        private readonly CompanyDbContext _context;

        public DepartmentRepositories(CompanyDbContext context) : base(context)
        {
            _context = context;
        }
     

        async Task< List<Department>> IDepartmentRepositories.GetByNameAsync(string name)
        {
            return await _context.Departments.Include(D => D.employees).Where(D => D.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
