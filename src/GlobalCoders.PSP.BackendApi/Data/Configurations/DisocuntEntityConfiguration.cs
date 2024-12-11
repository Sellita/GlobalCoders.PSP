using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class DiscountEntityConfiguration : IEntityTypeConfiguration<DiscountEntity>
{
    public void Configure(EntityTypeBuilder<DiscountEntity> builder)
    {
        // Table Mapping
        builder.ToTable("Discount");

        // Primary Key
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        // Code Property
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(255); // Assuming DefaultStringLimitation is 255

        // Description Property
        builder.Property(p => p.Description)
            .HasMaxLength(255); // Optional

        // Percentage Property
        builder.Property(p => p.Percentage)
            .IsRequired();

        // Usage Limit and Count Properties
        builder.Property(p => p.UsageLimit)
            .IsRequired();

        builder.Property(p => p.UsageCount)
            .IsRequired();

        // Start and End Dates
        builder.Property(p => p.StartDate)
            .IsRequired();

        builder.Property(p => p.EndDate)
            .IsRequired();

        // Created Date
        builder.Property(p => p.CreatedDateTime)
            .IsRequired();

        // Status Property
        builder.Property(p => p.Status)
            .IsRequired()
            .HasMaxLength(50)
            .HasDefaultValue("Active");
    }
}