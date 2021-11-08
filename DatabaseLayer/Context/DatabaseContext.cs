using DatabaseLayer.Entity;
using DatabaseLayer.Entity.Identity;
using DatabaseLayer.Entity.Map;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseLayer.Context
{
    public class DatabaseContext : IdentityDbContext<AppUser> 
    {

        private IConfiguration _configuration { get; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly ILoggerFactory loggerFactory = LoggerFactory.Create(
            (ILoggingBuilder logger) =>
            {
                logger.AddFilter(DbLoggerCategory.Query.Name, LogLevel.Information);     
                logger.AddConsole();            
            }
        );

        public DbSet<ClassModel> Classes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<ClassSubject> ClassSubjects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseLoggerFactory(loggerFactory);
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("ITS-EX3"), b => b.MigrationsAssembly("WebLayer")).EnableSensitiveDataLogging();
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var model in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = model.GetTableName();
                if (tableName.StartsWith("AspNet"))
                    model.SetTableName(tableName.Substring(6));
            }
            modelBuilder.ApplyConfiguration(new ClassMap());
            modelBuilder.ApplyConfiguration(new ClassStudentMap());
            modelBuilder.ApplyConfiguration(new ClassSubjectMap());
            modelBuilder.ApplyConfiguration(new SubjectMap());
            modelBuilder.ApplyConfiguration(new StudentMap());

            
        }
    }
}
