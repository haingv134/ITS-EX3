using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLayer.Entity.Map
{
    public class SubjectMap : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            // PK
            builder.HasKey(s => s.SubjectId);
            builder.Property(s => s.SubjectId)
                    .HasColumnType("UUID")
                    .HasDefaultValueSql("uuid_generate_v4()")
                    .IsRequired(true);

            builder.Property(s => s.SubjectCode)
                    .HasColumnType("VARCHAR")
                    .IsRequired(true)
                    .HasMaxLength(10);
            builder.Property(s => s.StartTime)
                        .HasDefaultValue(DateTime.Now)
                    .HasColumnType("DATE");
            builder.Property(s => s.EndTime)
                    .HasColumnType("DATE");
            builder.Property(s => s.Name)
                    .HasColumnType("VARCHAR")
                    .IsRequired(true)
                    .HasMaxLength(50);
            builder.Property(c => c.IsAvaiable)
            .IsRequired(true)
            .HasDefaultValue(true)
            .HasColumnType("BOOLEAN");
        }
    }
}
