using DataEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataAccessLayer.Repository
{
    public class AppDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public DbSet<Property> Properties { set; get; }
        public DbSet<RoleMenuMap> RoleMenuMaps { set; get; }
        public DbSet<Menu> Menu { set; get; }

        public DbSet<Languages> Languages { set; get; }
        public DbSet<UserProperty> UserProperties { set; get; }
        public static readonly ILoggerFactory MyLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public AppDBContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLoggerFactory(MyLoggerFactory);
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //ignoring some tables
            builder.Ignore<IdentityUserToken<long>>();
            builder.Ignore<IdentityUserLogin<long>>();
            builder.Ignore<IdentityUserClaim<long>>();
            builder.Ignore<IdentityRoleClaim<long>>();
            //configuration
            //disabling on delete cascade
            //var entityfk = builder.Model.GetEntityTypes().SelectMany(t=>t.GetForeignKeys()).Where(fk=>!fk.IsOwnership && fk.DeleteBehavior==DeleteBehavior.Cascade);
            //foreach(var fk in entityfk)
            //    fk.DeleteBehavior = DeleteBehavior.Restrict;
            builder.Entity<ApplicationUser>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber).IsUnique();
            builder.Entity<Property>().HasIndex(x => x.PropertyName).IsUnique();
            builder.Entity<PropertyType>().HasIndex(x => x.PropertyTypeName).IsUnique();
            builder.Entity<ApplicationUser>().Property(x => x.SMSAltert).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().Property(x => x.ClockType).HasDefaultValue(12);
            builder.Entity<ApplicationUser>().Property(x => x.LanguageId).HasDefaultValue(1);
            builder.Entity<ApplicationUser>().Property(x => x.IsActive).HasDefaultValue(true);
            builder.Entity<UserProperty>().Property(x => x.isPrimary).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().HasOne(s => s.Manager)
            .WithMany().HasForeignKey(x => x.ManagerId);


            builder.Entity<ApplicationRole>().HasData(
             new ApplicationRole()
             {
                 Id = 1,
                 Name = "Admin",
                 NormalizedName = "ADMIN"
             },
              new ApplicationRole()
              {
                  Id = 2,
                  Name = "User",
                  NormalizedName = "USER"
              });
            builder.Entity<Languages>().HasData(
                new Languages()
                {
                    Id = 1,
                    Language = "English"
                });

            builder.Entity<PropertyType>()
                   .HasData(new PropertyType() { Id = 1, PropertyTypeName = "Hotel" });
            builder.Entity<Menu>().HasData(
                new Menu()
                {
                    Id = 1,
                    MenuName = "Add User"
                },
                new Menu()
                {
                    Id = 2,
                    MenuName = "View Users"
                }, new Menu()
                {
                    Id = 3,
                    MenuName = "View Property"
                }
            , new Menu()
            {
                Id = 4,
                MenuName = "Edit User"
            }
            , new Menu()
            {
                Id = 5,
                MenuName = "Add Property"
            }
            , new Menu()
            {
                Id = 6,
                MenuName = "Edit Property"
            }
            , new Menu()
            {
                Id = 7,
                MenuName = "ActDct User"
            }, new Menu()
            {
                Id = 8,
                MenuName = "View User Detail"
            },
             new Menu()
             {
                 Id = 9,
                 MenuName = "Delete Property"
             }, new Menu()
             {
                 Id = 10,
                 MenuName = "Edit Feature"
             }, new Menu()
             {
                 Id = 11,
                 MenuName = "Access Setting"
             });

            builder.Entity<RoleMenuMap>().HasData(
                new RoleMenuMap { Id = 1, MenuId = 1, RoleId = 1 },
                new RoleMenuMap { Id = 2, MenuId = 2, RoleId = 1 },
                new RoleMenuMap { Id = 3, MenuId = 3, RoleId = 1 },
                new RoleMenuMap { Id = 4, MenuId = 4, RoleId = 1 },
                new RoleMenuMap { Id = 5, MenuId = 5, RoleId = 1 },
                new RoleMenuMap { Id = 6, MenuId = 6, RoleId = 1 },
                new RoleMenuMap { Id = 7, MenuId = 7, RoleId = 1 },
                new RoleMenuMap { Id = 8, MenuId = 8, RoleId = 1 },
                new RoleMenuMap { Id = 9, MenuId = 9, RoleId = 1 },
                new RoleMenuMap { Id = 10, MenuId = 10, RoleId = 1 }
                );


        }
    }

}
