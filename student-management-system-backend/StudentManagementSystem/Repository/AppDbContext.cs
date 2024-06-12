using Microsoft.EntityFrameworkCore;
using Repository.Enums;
using Repository.Modals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }

        public virtual DbSet<Subject> Subjects { get; set; }

        public virtual DbSet<Attendance> Attendance { get; set; }

        public virtual DbSet<Grades> Grades { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>().HasData(new Users
            {
                UserId = 1,
                Name = "Admin",
                Email = "admin@gmail.com",
                Password = "Admin@1990",
                DateOfBirth = new DateTime(1990, 1, 1),
                DateOfEnrollment = new DateTime(2018, 1, 1),
                Role = Roles.Admin,
                IsActive = true,
                IsPasswordReset = true
            });

            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, SubjectName = "Maths" },
                new Subject { SubjectId = 2, SubjectName = "Science" },
                new Subject { SubjectId = 3, SubjectName = "English" },
                new Subject { SubjectId = 4, SubjectName = "Social Studies" }
            );

        }
    }
}
