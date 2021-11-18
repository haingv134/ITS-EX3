using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DatabaseLayer.Enum;

namespace DatabaseLayer.Entity.Map
{
    public class StudentMap : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // PK
            builder.HasKey(s => s.StudentId);
            builder.Property(s => s.StudentId)
                .HasColumnType("UUID")
                .HasDefaultValueSql("uuid_generate_v4()")
                .IsRequired(true);
            builder.Property(s => s.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnType("VARCHAR");
            builder.Property(s => s.StudentCode)
                .HasMaxLength(50)
                .IsRequired(true)
                .HasColumnType("VARCHAR");
            builder.Property(s => s.Gender)
                .IsRequired(true)
                .HasDefaultValue(true)
                .HasColumnType("BOOLEAN");
            builder.Property(s => s.YearOfEnroll)
                .IsRequired(true)
                .HasDefaultValue(DateTime.Now.Year)
                .HasColumnType("INT");
            builder.Property(s => s.ExtraInfor)
                .IsRequired(false)
                .HasMaxLength(500)
                .HasColumnType("VARCHAR");
            builder.Property(s => s.Birthday)
                .IsRequired(true)
                .HasDefaultValue(DateTime.Now)
                .HasColumnType("DATE");
            builder.Property(c => c.IsAvaiable)
                .IsRequired(true)
                .HasDefaultValue(true)
                .HasColumnType("BOOLEAN");
        }
    }
}
