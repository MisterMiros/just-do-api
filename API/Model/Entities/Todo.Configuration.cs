using API.Model.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Model.Entities
{
    public class TodoConfiguration : IEntityTypeConfiguration<Todo>
    {
        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.UserId).IsRequired();
            builder.Property(e => e.Title).IsRequired();
            builder.Property(e => e.DueDate).IsRequired();
            builder.Property(e => e.Priority).IsRequired();
            builder.Property(e => e.Done).IsRequired().HasDefaultValue(false);
        }
    }
}
