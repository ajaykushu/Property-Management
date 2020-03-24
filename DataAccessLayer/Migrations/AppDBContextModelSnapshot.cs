﻿// <auto-generated />
using System;
using DataAccessLayer.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccessLayer.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataEntity.ApplicationRole", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            ConcurrencyStamp = "0461163f-dc9b-431d-91c9-ba60e57340e2",
                            Name = "Admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = 2L,
                            ConcurrencyStamp = "27458f36-30f5-4746-baa3-8e2414dc124f",
                            Name = "User",
                            NormalizedName = "USER"
                        });
                });

            modelBuilder.Entity("DataEntity.ApplicationUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ClockType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("varchar(2)")
                        .HasDefaultValue("12");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<int>("LanguageId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<long?>("ManagerId")
                        .HasColumnType("bigint");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("OfficeExt")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("PhotoPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SMSAltert")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Suffix")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("TimeZone")
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("LanguageId");

                    b.HasIndex("ManagerId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("PhoneNumber")
                        .IsUnique()
                        .HasFilter("[PhoneNumber] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DataEntity.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DepartmentName")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("DataEntity.Languages", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("varchar(40)");

                    b.HasKey("Id");

                    b.ToTable("Languages");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Language = "English"
                        });
                });

            modelBuilder.Entity("DataEntity.Menu", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("MenuName")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.ToTable("Menu");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            MenuName = "Add User"
                        },
                        new
                        {
                            Id = 2L,
                            MenuName = "View Users"
                        },
                        new
                        {
                            Id = 3L,
                            MenuName = "View Property"
                        },
                        new
                        {
                            Id = 4L,
                            MenuName = "Edit User"
                        },
                        new
                        {
                            Id = 5L,
                            MenuName = "Add Property"
                        },
                        new
                        {
                            Id = 6L,
                            MenuName = "Edit Property"
                        },
                        new
                        {
                            Id = 7L,
                            MenuName = "ActDct User"
                        },
                        new
                        {
                            Id = 8L,
                            MenuName = "View User Detail"
                        },
                        new
                        {
                            Id = 9L,
                            MenuName = "Delete Property"
                        },
                        new
                        {
                            Id = 10L,
                            MenuName = "Edit Feature"
                        },
                        new
                        {
                            Id = 11L,
                            MenuName = "Access Setting"
                        });
                });

            modelBuilder.Entity("DataEntity.PayScale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Billingtype")
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Code")
                        .HasColumnType("varchar(5)");

                    b.Property<decimal>("HourlyCost")
                        .HasColumnType("money");

                    b.HasKey("Id");

                    b.ToTable("PayScale");
                });

            modelBuilder.Entity("DataEntity.Property", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("varchar(10)");

                    b.Property<string>("Locality")
                        .HasColumnType("varchar(50)");

                    b.Property<string>("PinCode")
                        .IsRequired()
                        .HasColumnType("varchar(8)");

                    b.Property<string>("PropertyName")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<int>("PropertyTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("varchar(50)");

                    b.Property<string>("StreetLine2")
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyName")
                        .IsUnique();

                    b.HasIndex("PropertyTypeId");

                    b.ToTable("Properties");
                });

            modelBuilder.Entity("DataEntity.PropertyType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PropertyTypeName")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("PropertyTypeName")
                        .IsUnique()
                        .HasFilter("[PropertyTypeName] IS NOT NULL");

                    b.ToTable("PropertyType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PropertyTypeName = "Hotel"
                        });
                });

            modelBuilder.Entity("DataEntity.RoleMenuMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("MenuId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MenuId");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleMenuMaps");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MenuId = 1L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 2,
                            MenuId = 2L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 3,
                            MenuId = 3L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 4,
                            MenuId = 4L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 5,
                            MenuId = 5L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 6,
                            MenuId = 6L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 7,
                            MenuId = 7L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 8,
                            MenuId = 8L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 9,
                            MenuId = 9L,
                            RoleId = 1L
                        },
                        new
                        {
                            Id = 10,
                            MenuId = 10L,
                            RoleId = 1L
                        });
                });

            modelBuilder.Entity("DataEntity.UserProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ApplicationUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("PropertyId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("PropertyId");

                    b.ToTable("UserProperties");
                });

            modelBuilder.Entity("DataEntity.WorkerType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<int>("PayScaleId")
                        .HasColumnType("int");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasColumnType("varchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.HasIndex("PayScaleId");

                    b.ToTable("WorkerType");
                });

            modelBuilder.Entity("DataEntity.WorkerTyperUserMap", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("ApplicationUserId")
                        .HasColumnType("bigint");

                    b.Property<long>("WorkerTypeId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("WorkerTypeId");

                    b.ToTable("WorkerTyperUserMap");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.Property<long>("RoleId")
                        .HasColumnType("bigint");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("DataEntity.ApplicationUser", b =>
                {
                    b.HasOne("DataEntity.Languages", "Language")
                        .WithMany()
                        .HasForeignKey("LanguageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataEntity.ApplicationUser", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId");
                });

            modelBuilder.Entity("DataEntity.Property", b =>
                {
                    b.HasOne("DataEntity.PropertyType", "PropertyTypes")
                        .WithMany("Properties")
                        .HasForeignKey("PropertyTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataEntity.RoleMenuMap", b =>
                {
                    b.HasOne("DataEntity.Menu", "Menu")
                        .WithMany("RoleMenuMaps")
                        .HasForeignKey("MenuId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataEntity.ApplicationRole", "Role")
                        .WithMany("RoleMenuMaps")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataEntity.UserProperty", b =>
                {
                    b.HasOne("DataEntity.ApplicationUser", "ApplicationUser")
                        .WithMany("UserProperties")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataEntity.Property", "Property")
                        .WithMany("UserProperties")
                        .HasForeignKey("PropertyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataEntity.WorkerType", b =>
                {
                    b.HasOne("DataEntity.Department", "Departments")
                        .WithMany("WorkerTypes")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataEntity.PayScale", "PayScale")
                        .WithMany("WorkerTypes")
                        .HasForeignKey("PayScaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DataEntity.WorkerTyperUserMap", b =>
                {
                    b.HasOne("DataEntity.ApplicationUser", "ApplicationUser")
                        .WithMany("WorkerTyperUserMaps")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataEntity.WorkerType", "WorkerType")
                        .WithMany("WorkerTyperUserMaps")
                        .HasForeignKey("WorkerTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<long>", b =>
                {
                    b.HasOne("DataEntity.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataEntity.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
