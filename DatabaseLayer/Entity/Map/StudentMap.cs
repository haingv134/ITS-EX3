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
                            .UseIdentityColumn(1, 1);
            //
            builder.Property(s => s.Name)
                    .HasMaxLength(50)
                    .IsRequired(true)
                    .HasColumnType("VARCHAR");
            //
            builder.Property(s => s.Gender)
                    .IsRequired(true)
                    .HasDefaultValue(true)
                    .HasColumnType("BOOLEAN");
            //
            builder.Property(s => s.Birthday)
                    .IsRequired(true)
                    .HasDefaultValue(DateTime.Now)
                    .HasColumnType("DATE");
            
        }
    }
}
