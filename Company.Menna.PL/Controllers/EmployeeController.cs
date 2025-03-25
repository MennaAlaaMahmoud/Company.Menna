using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepositories _departmentRepositories;

        public EmployeeController(IEmployeeRepository employeeRepository , IDepartmentRepositories departmentRepositories )
        {
            _employeeRepository = employeeRepository;
            _departmentRepositories = departmentRepositories;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employee = _employeeRepository.GetAll();
            // Dictionary  : 3 Property 
            // 1. ViewData : Transfer Extra Information Form Controller (Action) To View 
            //ViewData["Message"] = "Hello From ViewData";

            // 2. ViewBag  : Transfer Extra Information Form Controller (Action) To View    
           // ViewBag.Message =  new { Message = "Hello From ViewBag" };



            return View(employee);
        }

        [HttpGet]
        public IActionResult Create()
        {
         var departments = _departmentRepositories.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDelete = model.IsDelete,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    DepartmentId = model.DepartmentId
                };
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction(nameof(Index));
                }

            } return View(model);
        }

        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = _employeeRepository.Get(id.Value);
            if(employee is null) return NotFound(new {StatusCode =404, message =$"Employee With Id : {id} is not found" });
            return View(viewName , employee);
        }

        [HttpGet]
        public IActionResult Edit (int? id)
        {
            var departments = _departmentRepositories.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");// 400

            var employee = _employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 400, message = $"Employee With Id : {id} is not found" });
            var employeeDto = new CreateEmployeeDto()
            {
               
                Name = employee.Name,
                Address = employee.Address,
                Age = employee.Age,
                CreateAt = employee.CreateAt,
                HiringDate = employee.HiringDate,
                Email = employee.Email,
                IsActive = employee.IsActive,
                IsDelete = employee.IsDelete,
                Phone = employee.Phone,
                Salary = employee.Salary,

            };
            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                // if (id != employee.Id) return BadRequest();
                var employee = new Employee()
                {
                    Id = id,
                    Name = model.Name,
                    Address = model.Address,
                    Age = model.Age,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    Email = model.Email,
                    IsActive = model.IsActive,
                    IsDelete = model.IsDelete,
                    Phone = model.Phone,
                    Salary = model.Salary,

                };
                var count = _employeeRepository.Update(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id ,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id , Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest();
                var count = _employeeRepository.Delete(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);
        }
    
    }
}
