﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.DAL.Models;

namespace Company.Menna.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        //IEnumerable<Employee> GetAll();
        //Employee? Get(int id);
        //int Add (Employee model);
        //int Update (Employee model);
        //int Delete (Employee model);
    }
}
