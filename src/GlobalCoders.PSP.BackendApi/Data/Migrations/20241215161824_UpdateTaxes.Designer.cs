﻿// <auto-generated />
using System;
using GlobalCoders.PSP.BackendApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GlobalCoders.PSP.BackendApi.Data.Migrations
{
    [DbContext(typeof(BackendContext))]
    [Migration("20241215161824_UpdateTaxes")]
    partial class UpdateTaxes
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DayMounth")
                        .HasColumnType("integer");

                    b.Property<int>("DayWeek")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<int>("Hour")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<int>("Minute")
                        .HasColumnType("integer");

                    b.Property<int>("Mounth")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("IsActive");

                    b.HasIndex("IsDeleted");

                    b.HasIndex("MerchantId");

                    b.HasIndex("Name");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionEntity", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AppRoleId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("EmployeeId")
                        .HasColumnType("uuid");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("AppRoleId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionTemplateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<bool>("Enable")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("Enable");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.InventoryManagement.Entities.InventoryTransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.HasIndex("ProductId", "MerchantId");

                    b.ToTable("InventoryTransactions");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric");

                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal>("Tips")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("MerchantId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderPaymentsEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderPayments");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderProductEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Discount")
                        .HasColumnType("numeric");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderProductTaxEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid?>("OrderProductId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("OrderProductId");

                    b.ToTable("OrderProductTaxEntity");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<TimeSpan>("BatchOutTime")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("ClosingHour")
                        .HasColumnType("interval");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("LegalName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("MainPhoneNr")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<TimeSpan>("OpeningHour")
                        .HasColumnType("interval");

                    b.Property<string>("SecondaryPhoneNr")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.HasKey("Id");

                    b.ToTable("Merchant");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<int>("ProductState")
                        .HasColumnType("integer");

                    b.Property<Guid>("ProductTypeId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductTypeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.ToTable("ProductType");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities.SurchargeEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DayOfMonth")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("DayOfWeek")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Hour")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Minute")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Month")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.ToTable("Surcharge");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.TaxManagement.Entities.TaxEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MerchantId")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid?>("ProductTypeId")
                        .HasColumnType("uuid");

                    b.Property<int>("Status")
                        .HasMaxLength(255)
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<decimal>("Value")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("MerchantId");

                    b.HasIndex("ProductTypeId");

                    b.ToTable("Tax");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionTemplateEntity", "AppRole")
                        .WithMany("UserRoles")
                        .HasForeignKey("AppRoleId");

                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", "Employee")
                        .WithMany("UserPermissions")
                        .HasForeignKey("EmployeeId");

                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionTemplateEntity", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppRole");

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.InventoryManagement.Entities.InventoryTransactionEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Merchant");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderPaymentsEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderEntity", null)
                        .WithMany("OrderPayments")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderProductEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderEntity", null)
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId");

                    b.HasOne("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductEntity", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderProductTaxEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderProductEntity", null)
                        .WithMany("OrderProductTaxes")
                        .HasForeignKey("OrderProductId");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductTypeEntity", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Merchant");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities.SurchargeEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.TaxManagement.Entities.TaxEntity", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities.MerchantEntity", "Merchant")
                        .WithMany()
                        .HasForeignKey("MerchantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GlobalCoders.PSP.BackendApi.ProductsManagment.Entities.ProductTypeEntity", "ProductType")
                        .WithMany()
                        .HasForeignKey("ProductTypeId");

                    b.Navigation("Merchant");

                    b.Navigation("ProductType");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionTemplateEntity", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.EmployeeEntity", b =>
                {
                    b.Navigation("UserPermissions");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities.PermisionTemplateEntity", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderEntity", b =>
                {
                    b.Navigation("OrderPayments");

                    b.Navigation("OrderProducts");
                });

            modelBuilder.Entity("GlobalCoders.PSP.BackendApi.OrdersManagement.Entities.OrderProductEntity", b =>
                {
                    b.Navigation("OrderProductTaxes");
                });
#pragma warning restore 612, 618
        }
    }
}
