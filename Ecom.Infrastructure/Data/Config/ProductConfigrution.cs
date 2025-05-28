using Ecom.Core.Entites.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class ProductConfigrution : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Description).IsRequired();
            builder.Property(p => p.NewPrice).IsRequired().HasColumnType("decimal(18,2)");
            //seeding Data
            builder.HasData(
                new Product
                {
                    Id = 1,
                    Name = "Test Product",
                    Description = "This is a test product",
                    NewPrice = 99.99m,
                    CategoryId = 1 // Assuming this category exists
                });


        }
    }
    
}
