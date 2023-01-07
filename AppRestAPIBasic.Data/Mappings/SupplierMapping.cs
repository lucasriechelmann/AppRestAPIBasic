using AppRestAPIBasic.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AppRestAPIBasic.Data.Mappings
{
    public class SupplierMapping : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");
            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.HasOne(x => x.Address)
                .WithOne(x => x.Supplier);

            builder.HasMany(x => x.Products)
                .WithOne(x => x.Supplier)
                .HasForeignKey(x => x.SupplierId);

            builder.ToTable("Suppliers");
        }
    }
}
