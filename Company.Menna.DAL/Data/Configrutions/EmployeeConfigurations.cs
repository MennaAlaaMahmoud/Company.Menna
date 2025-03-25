using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Menna.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Menna.DAL.Data.Configrutions
{
    public class EmployeeConfigurations : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> E)
        {
            E.Property(E => E.Salary).HasColumnType("decimal(18,2)");

            E.HasOne(E => E.Department)
             .WithMany(D => D.employees)
             .HasForeignKey(E => E.DepartmentId)
             .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
