using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuanLyCaThi.Models;
namespace QuanLyCaThi.Data;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ExamTime> ExamTime { get; set; } = default!;
        public DbSet<RegisteredList> RegisteredList { get; set; } = default!;
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<Subject> Subject { get; set; } = default!;
        public DbSet<SecurityCode> SecurityCode { get; set; } = default!;
    }
