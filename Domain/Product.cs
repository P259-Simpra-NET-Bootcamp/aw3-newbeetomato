using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SimApi.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimApi.Data;


[Table("Product", Schema = "dbo")]
public class Product : BaseModel
{
    public int CategoryId { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tag { get; set; }
    public bool IsValid { get; set; }


    public virtual Category Category { get; set; }

}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Property(x => x.Id).IsRequired(true).UseIdentityColumn();
        builder.Property(x => x.CreatedAt).IsRequired(false);
        builder.Property(x => x.CreatedBy).IsRequired(false).HasMaxLength(30);
        builder.Property(x => x.UpdatedAt).IsRequired(false);
        builder.Property(x => x.UpdatedBy).IsRequired(false).HasMaxLength(30);

        builder.Property(x => x.Name).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Url).IsRequired(true).HasMaxLength(30);
        builder.Property(x => x.Tag).IsRequired(true).HasMaxLength(100);
        builder.Property(x => x.CategoryId).IsRequired(true);
        builder.Property(x => x.IsValid).IsRequired(true).HasDefaultValue(true);


        builder.HasIndex(x => x.Name).IsUnique(true);

        builder.HasOne(p => p.Category) // Her bir Product'ın bir Category'si olmasını sağlar.
            .WithMany(c => c.Products) // Her bir Category'nin birden çok Product'a sahip olmasını sağlar.
            .HasForeignKey(p => p.CategoryId) // Product sınıfındaki CategoryId alanını foreign key olarak belirler.
            .OnDelete(DeleteBehavior.Cascade); // Eğer bir Category silinirse, ilgili Product'lar da silinir.

    }
}