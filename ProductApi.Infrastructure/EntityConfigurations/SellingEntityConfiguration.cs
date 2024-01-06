using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductApi.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.EntityConfigurations
{
    public class SellingEntityConfiguration : IEntityTypeConfiguration<Selling>
    {
        public void Configure(EntityTypeBuilder<Selling> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.ProductId).IsRequired();
            builder.Property(x => x.Price).IsRequired();
            builder.Property(x => x.Quantity).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UpdatedAt).IsRequired();
        }
    }
}
