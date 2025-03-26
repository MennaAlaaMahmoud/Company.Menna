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
        private readonly IEmployeeRepository _employeeRepository;

        // ASK CLR Create Object From DepartmentRepositories
        public DepartmentController(IDepartmentRepositories departmentRepositories , IEmployeeRepository employeeRepository)
        {
            _departmentRepositories = departmentRepositories;
            _employeeRepository = employeeRepository;
        }

        [HttpGet] // Get: / Department /Index 
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Department> departments;
            if (string.IsNullOrEmpty(SearchInput))
            {
                departments = _departmentRepositories.GetAll();
            }
            else
            {
                departments = _departmentRepositories.GetByName(SearchInput);
            }

        

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var employee = _employeeRepository.GetAll();
            ViewData["employee"] = employee;
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
                    TempData["Message"] = "Department is Created !!";
                    return RedirectToAction(nameof(Index));
                   
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");// 400

           var department = _departmentRepositories.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 400, message = $"Department With Id : {id} is not found" });

            return View(viewName,department);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var employee = _employeeRepository.GetAll();
            ViewData["employee"] = employee;
            if (id is null) return BadRequest("Invalid Id");// 400

            var department = _departmentRepositories.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 400, message = $"Department With Id : {id} is not found" });
            var departmentDto = new CreateDepartmentDto()
            {
                Name = department.Name,
                CreateAt = department.CreateAt,
            };
            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Department model)
        {

            if (ModelState.IsValid)
            {
                //  if (id != department.Id) return BadRequest();
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };
                {
                    var Count = _departmentRepositories.Update(department);
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }


            return View(model);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Name = model.Name,
        //            Code = model.Code,
        //            CreateAt = model.CreateAt,

        //        };
        //            var Count = _departmentRepositories.Update(department);
        //            if (Count > 0)
        //            {
        //                return RedirectToAction(nameof(Index));
        //            }

        //    }


        //    return View(model);
        //}


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id");// 400

            //var department = _departmentRepositories.Get(id.Value);
            //if (department is null) return NotFound(new { StatusCode = 400, message = $"Department With Id : {id} is not found" });

            return Details(id,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {

            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest();
                {
                    var Count = _departmentRepositories.Delete(department);
                    if (Count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }


            return View(department);
        }




    }
}
