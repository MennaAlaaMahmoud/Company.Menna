using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
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

        [HttpGet]
        public IActionResult Create()
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid)
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };
               var count = _departmentRepositories.Add(department);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View();
        }

    }
}
