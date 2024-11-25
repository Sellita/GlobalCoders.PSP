using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class SurchargeEntityConfiguration : IEntityTypeConfiguration<SurchargeEntity>
{
    public void Configure(EntityTypeBuilder<SurchargeEntity> builder)
    {
        builder.ToTable("Surcharges");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Value)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Property(e => e.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue("Value");

        builder.Property(e => e.CreationDateTime)
            .IsRequired();

        builder.Property(e => e.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50)
            .HasDefaultValue("Active");

        builder.Property(e => e.Minute)
            .HasMaxLength(255);

        builder.Property(e => e.Hour)
            .HasMaxLength(255);

        builder.Property(e => e.DayOfMonth)
            .HasMaxLength(255);

        builder.Property(e => e.Month)
            .HasMaxLength(255);

        builder.Property(e => e.DayOfWeek)
            .HasMaxLength(255);
        
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}