using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepositories _departmentRepositories;

        // ASK CLR Create Object From DepartmentRepositories
        public DepartmentController(IDepartmentRepositories departmentRepositories)
        {
            _departmentRepositories = departmentRepositories;
        }

        [HttpGet] // Get: / Department /Index 
        public IActionResult Index()
        {

            var department = _departmentRepositories.GetAll();




            return View(department);
        }
    }
}
