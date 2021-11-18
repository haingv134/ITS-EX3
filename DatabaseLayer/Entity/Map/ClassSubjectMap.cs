using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity.Map
{
    public class ClassSubjectMap : IEntityTypeConfiguration<ClassSubject>
    {
        public void Configure(EntityTypeBuilder<ClassSubject> builder)
        {
            builder.HasKey(cs => cs.ClassSubjectId);

            // 1 Class - N ClassSubject
            builder.HasOne(cs => cs.Class)
                    .WithMany(c => c.ClassSubject)
                    .HasForeignKey(cs => cs.ClassId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("Fk_ClassSubject_Class");
            // 1 Subject - N ClassSubjecj
            builder.HasOne(cs => cs.Subject)
                    .WithMany(s => s.ClassSubject)
                    .HasForeignKey(cs => cs.SubjectId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .HasConstraintName("Fk_ClassSubject_Subject");
            // make couple ClassId - SubjectId is unique
            builder.HasIndex(ClassSubject => new { ClassSubject.SubjectId, ClassSubject.ClassId }).IsUnique(true);


            builder.Property(c => c.IsAvaiable)                    
                    .IsRequired(true)
                    .HasDefaultValue(true)
                    .HasColumnType("BOOLEAN");
        }
    }
}
