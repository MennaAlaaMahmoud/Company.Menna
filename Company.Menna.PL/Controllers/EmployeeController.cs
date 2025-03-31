using AutoMapper;
using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepositories _departmentRepositories;

        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository ,
            //IDepartmentRepositories departmentRepositories,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepositories = departmentRepositories;

            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees =_unitOfWork.employeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.employeeRepository.GetByName(SearchInput);
            }

            // Dictionary   : 3 Property
            // 1. ViewData  : Transfer Extra Information From Controller (Action) To View

            //ViewData["Message"] = "Hello From ViewDat,a";

            // 2. ViewBag   : Transfer Extra Information From Controller (Action) To View

            //ViewBag.Massage = "Hello From ViewBag";

            //ViewBag.Message = new { Message = "Hello From ViewBag" };

            // 3. TempData  :


            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
         var departments = _unitOfWork.departmentRepositories.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
               var employee = _mapper.Map<Employee>(model);
                  _unitOfWork.employeeRepository.Add(employee);
                var count = _unitOfWork.Complete();
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
            var employee = _unitOfWork.employeeRepository.Get(id.Value);
            if(employee is null) return NotFound(new {StatusCode =404, message =$"Employee With Id : {id} is not found" });
           

            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit (int? id)
        {
            var departments = _unitOfWork.departmentRepositories.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");// 400

            var employee = _unitOfWork.employeeRepository.Get(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 400, message = $"Employee With Id : {id} is not found" });
           var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(dto);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id,CreateEmployeeDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.employeeRepository.Update(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewName,model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return Details(id ,"Delete");
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id , CreateEmployeeDto model )
        {
            if (ModelState.IsValid)
            {
               var employee = _mapper.Map<Employee>(model);
                _unitOfWork.employeeRepository.Delete(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    
    }
}
