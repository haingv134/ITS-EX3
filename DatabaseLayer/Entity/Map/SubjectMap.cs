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
                    .UseIdentityColumn(1, 1)
                    .HasColumnType("INT");
            //
            builder.Property(s => s.Name)
                    .HasColumnType("NVARCHAR")
                    .IsRequired(true)
                    .HasMaxLength(50);
        }
    }
}
