﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.BLL.Interfaces;
using Company.Menna.DAL.Data.Context;
using Company.Menna.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Menna.BLL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;

        public GenericRepository(CompanyDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task< T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.Employees.Include(E => E.Department).FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return _context.Set<T>().Find(id);
        }

        public async Task AddAsync(T model)
        {
            await _context.AddAsync(model);
          
        }

        public void Update(T model)
        {
            _context.Set<T>().Update(model);
          
        }
        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
           
        }
    }
}
