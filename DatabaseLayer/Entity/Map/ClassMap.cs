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

            // Setup property for ClassId
            builder.Property(c => c.ClassId)
                    .UseIdentityColumn(1, 1)
                    .HasColumnType("INT");
            // Setup property for Class Name
            builder.Property(c => c.Name)
                    .HasColumnType("VARCHAR")
                    .HasMaxLength(50)
                    .IsRequired(true);
            
        }
    }
}
