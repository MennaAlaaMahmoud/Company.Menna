using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Data.Context;

namespace Company.Menna.BLL
{
    public class Unitofwork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepositories departmentRepositories { get; }

        public IEmployeeRepository employeeRepository { get; }

        public Unitofwork(CompanyDbContext context)
        {
           
            _context = context;
            departmentRepositories = new DepartmentRepositories(_context);
            employeeRepository = new EmployeeRepositories(_context);



        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
