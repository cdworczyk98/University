using Microsoft.EntityFrameworkCore;
using StaffShiftService.Database.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffShiftService.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<StaffShift> StaffShifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=StaffShiftMicroservice;Trusted_Connection=True;");
        }
    }
}
