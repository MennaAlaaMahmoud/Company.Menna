using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Menna.BLL.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {
         IDepartmentRepositories departmentRepositories  { get; }
         IEmployeeRepository employeeRepository { get; }
        Task<int> CompleteAsync();
    }
}
