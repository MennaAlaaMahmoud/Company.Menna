using AutoMapper;
using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Company.Menna.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Company.Menna.PL.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string? SearchInput)
        {
           IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees =await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.employeeRepository.GetByNameAsync(SearchInput);
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
        public async Task<IActionResult> Search(string? SearchInpust)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInpust))
            {
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.employeeRepository.GetByNameAsync(SearchInpust);
            }
            return PartialView("EmployeePartialView/EmployeeTablePartialView", employees);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
         var departments = await _unitOfWork.departmentRepositories.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                  model.ImageName =  DocumentSettings.UploadFile(model.Image, "Images");
                }
                 var employee = _mapper.Map<Employee>(model);
                 await _unitOfWork.employeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                { 
                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction(nameof(Index));
                }

            } return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");
            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);
            if(employee is null) return NotFound(new {StatusCode =404, message =$"Employee With Id : {id} is not found" });
           

            return View(employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int? id)
        {
            var departments = await _unitOfWork.departmentRepositories.GetAllAsync();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");// 400

            var employee = await _unitOfWork.employeeRepository.GetAsync(id.Value);
            if (employee is null) return NotFound(new { StatusCode = 400, message = $"Employee With Id : {id} is not found" });
           var dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(dto);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async  Task<IActionResult> Edit([FromRoute] int id,CreateEmployeeDto model, string viewName = "Edit")
        {
            if (ModelState.IsValid)
            {
                if (model.Image is not null  && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                }

                if (model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }
                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.employeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewName,model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id ,"Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int? id, Employee employee)
        {
            //if (ModelState.IsValid)
            //{
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var departmentDelete = await _unitOfWork.employeeRepository.GetAsync(id.Value);
            _unitOfWork.employeeRepository.Delete(departmentDelete);

            var Count = await _unitOfWork.CompleteAsync();

            if (Count > 0)
            {
                if(departmentDelete.ImageName is not null)
                {
                    DocumentSettings.DeleteFile(departmentDelete.ImageName, "Images");
                }
                return RedirectToAction("Index");
            }
            //}
            return View(employee);
        }

    }
}
