using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity.Map
{
    public class ClassStudentMap : IEntityTypeConfiguration<ClassStudent>
    {
        // N - N
        public void Configure(EntityTypeBuilder<ClassStudent> builder)
        {
            builder.HasKey(cs => cs.ClassStudentId);

            builder.Property(cs => cs.ClassStudentId)
                    .UseIdentityColumn(1, 1) // IDENTITY(1,1)                    
                    .HasColumnType("INT");


            // 1 ClassStudent - N Class
            builder.HasOne(cs => cs.Class)
                    .WithMany(c => c.ClassStudent)
                    .HasForeignKey(cs => cs.ClassId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_ClassStudent_Class");                       
            // 1 ClassStudent - N Student
            builder.HasOne(cs => cs.Student)
                    .WithMany(s => s.ClassStudent)
                    .HasForeignKey(cs => cs.StudentId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("Fk_ClassStudent_Student");
            // Make couple StudentId - ClassId is unique
            builder.HasIndex(ClassStudent => new { ClassStudent.ClassId, ClassStudent.StudentId }).IsUnique(true);
        }
    }
}
