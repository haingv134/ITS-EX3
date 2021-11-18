using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseLayer.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseLayer.Entity.Map
{
    public class ClassMap : IEntityTypeConfiguration<ClassModel>
    {
        public void Configure(EntityTypeBuilder<ClassModel> builder)
        {
            // PK
            builder.HasKey(c => c.ClassId);

            builder.Property(c => c.ClassId)
                    .HasColumnType("UUID")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .IsRequired(true);
            builder.Property(c => c.Name)
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(50)
                    .IsRequired(true);
            builder.Property(c => c.MaxStudent)
                    .IsRequired(true)
                    .HasColumnType("INT")
                    .HasDefaultValue(24);
            builder.Property(c => c.IsAvaiable)                    
                    .IsRequired(true)
                    .HasDefaultValue(true)
                    .HasColumnType("BOOLEAN");

        }
    }
}
