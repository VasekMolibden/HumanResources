using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HumanResources.Models;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Data
{
    public class HumanResourcesContext : DbContext
    {
        public HumanResourcesContext(DbContextOptions<HumanResourcesContext> options) : base(options)
        {
        }

        public DbSet<Position> Positions { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Position>().ToTable("Position");
            modelBuilder.Entity<Department>().ToTable("Department");
            modelBuilder.Entity<Employee>().ToTable("Employee");
            modelBuilder.Entity<Department>()
                .HasOne(d => d.Employee)
                .WithMany(e => e.Departments)
                .HasForeignKey(d => d.HeadID);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.Employee)
                .WithMany(e => e.Positions);
        }
    }
}
