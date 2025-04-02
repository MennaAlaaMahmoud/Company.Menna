using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.DAL.Models;

namespace Company.Menna.BLL.Interfaces
{
    public interface IDepartmentRepositories : IGenericRepository<Department>
    {
        Task<List<Department>> GetByNameAsync(string name);
    }
}
