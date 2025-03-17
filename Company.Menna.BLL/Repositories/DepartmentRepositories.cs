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
    public class DepartmentRepositories : GenericRepository<Department>, IDepartmentRepositories
    {
        public DepartmentRepositories(CompanyDbContext context) : base(context)
        {
        }
    }
}
