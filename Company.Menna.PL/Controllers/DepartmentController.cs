using AutoMapper;
using Company.Menna.BLL.Interfaces;
using Company.Menna.BLL.Repositories;
using Company.Menna.DAL.Models;
using Company.Menna.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Menna.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepositories _departmentRepositories;
        //private readonly IEmployeeRepository _employeeRepository;
        private readonly IMapper  _mapper;

        // ASK CLR Create Object From DepartmentRepositories
        public DepartmentController(
            IUnitOfWork unitOfWork,   
            IMapper mapper
            )
        {
            //_departmentRepositories = departmentRepositories;
            //_employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] // Get: / Department /Index 
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Department> departments;
            if (string.IsNullOrEmpty(SearchInput))
            {
                departments = _unitOfWork.departmentRepositories.GetAll();
            }
            else
            {
                departments = _unitOfWork.departmentRepositories.GetByName(SearchInput);
            }

        

            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var employee = _unitOfWork.employeeRepository.GetAll();
            ViewData["employee"] = employee;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                _unitOfWork.departmentRepositories.Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
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

           var department = _unitOfWork.departmentRepositories.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 400, message = $"Department With Id : {id} is not found" });

            return View(viewName,department);

        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.employeeRepository.GetAll();
            ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");// 400

            var department = _unitOfWork.departmentRepositories.Get(id.Value);
            if (department is null) return NotFound(new { StatusCode = 400, message = $"Employee With Id : {id} is not found" });
            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(dto);
        }

        [HttpPost]
       // [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model , string viewName = "Edit")
        {

            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                _unitOfWork.departmentRepositories.Update(department);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(viewName, model);
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
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDto model)
        {

            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                _unitOfWork.departmentRepositories.Delete(department);
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
