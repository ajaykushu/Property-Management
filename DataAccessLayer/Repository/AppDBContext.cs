using DataEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class AppDBContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public static readonly ILoggerFactory MyLoggerFactory
        = LoggerFactory.Create(builder => { builder.AddConsole(); });

        private readonly IHttpContextAccessor _httpContextAccessor;
        public AppDBContext(DbContextOptions options) : base(options)
        {
            _httpContextAccessor = new HttpContextAccessor();
        }
        
        public DbSet<SubLocation> Areas { set; get; }
        public DbSet<Comment> Comments { set; get; }
        public DbSet<Department> Departments { set; get; }
        public DbSet<Issue> Issues { set; get; }
        public DbSet<Item> Items { set; get; }
        public DbSet<Languages> Languages { set; get; }
        public DbSet<Location> Locations { set; get; }
        public DbSet<Menu> Menu { set; get; }
        public DbSet<Property> Properties { set; get; }
        public DbSet<Reply> Replies { set; get; }
        public DbSet<RoleMenuMap> RoleMenuMaps { set; get; }
        public DbSet<Stage> Stages { set; get; }
        public DbSet<UserProperty> UserProperties { set; get; }
        public DbSet<WorkOrder> WorkOrders { set; get; }
        public DbSet<UserNotification> UserNotifications { set; get; }
        public DbSet<Notification> Notifications { set; get; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseLoggerFactory(MyLoggerFactory);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //ignoring some tables
            builder.Ignore<IdentityUserToken<long>>();
            builder.Ignore<IdentityUserLogin<long>>();
            builder.Ignore<IdentityUserClaim<long>>();
            builder.Ignore<IdentityRoleClaim<long>>();

            builder.Entity<ApplicationUser>().HasIndex(x => x.Email).IsUnique();
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber).IsUnique();
            builder.Entity<Property>().HasIndex(x => x.PropertyName).IsUnique();
            builder.Entity<PropertyType>().HasIndex(x => x.PropertyTypeName).IsUnique();
            builder.Entity<ApplicationUser>().Property(x => x.SMSAltert).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().Property(x => x.ClockType).HasDefaultValue(12);
            builder.Entity<ApplicationUser>().Property(x => x.LanguageId).HasDefaultValue(1);
            builder.Entity<ApplicationUser>().Property(x => x.IsActive).HasDefaultValue(true);
            builder.Entity<UserProperty>().Property(x => x.IsPrimary).HasDefaultValue(false);
            builder.Entity<Property>().Property(x => x.IsActive).HasDefaultValue(true);
            builder.Entity<WorkOrder>().Property(x => x.Priority).HasDefaultValue(0);
            DataSeeder(builder);
        }

        private static void DataSeeder(ModelBuilder builder)
        {
            builder.Entity<Department>().HasData(
                            new Department()
                            {
                                Id = 1,
                                DepartmentName = "Administration",
                            },
                            new Department()
                            {
                                Id = 2,
                                DepartmentName = "Management",
                            },
                            new Department()
                            {
                                Id = 3,
                                DepartmentName = "Engineering",
                            }
                            );

            builder.Entity<ApplicationRole>().HasData(
             new ApplicationRole()
             {
                 Id = 1,
                 Name = "Master Admin",
                 NormalizedName = "MASTER ADMIN"
             },

              new ApplicationRole()
              {
                  Id = 2,
                  Name = "Admin",
                  NormalizedName = "ADMIN"
              },
            new ApplicationRole()
            {
                Id = 3,
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
                   .HasData(new PropertyType() { Id = 1, PropertyTypeName = "Hotel" }, new PropertyType() { Id = 2, PropertyTypeName = "PG" });
            builder.Entity<Item>()
                 .HasData(new Item() { Id = 1, ItemName = "Tv" }, new Item() { Id = 2, ItemName = "AC" });
            builder.Entity<Issue>()
                 .HasData(new Issue() { Id = 1, IssueName = "Power Problem" }, new Issue() { Id = 2, IssueName = "Item Not Available" });
            builder.Entity<Stage>()
                .HasData(new Stage() { Id = 1, StageCode = "OPEN", StageDescription = "Work Order Open State" }, new Stage() { Id = 2, StageCode = "BID ACCEPTED", StageDescription = "Bid Sucessfull" }, new Stage() { Id = 3, StageCode = "IN PROGRESS", StageDescription = "Work Order in Progress" }, new Stage() { Id = 4, StageCode = "COMPLETED", StageDescription = "Work Order Completed" });

            builder.Entity<Menu>().HasData(
                new Menu()
                {
                    Id = 1,
                    MenuName = "Add_User"
                },
                new Menu()
                {
                    Id = 2,
                    MenuName = "View_Users"
                }, new Menu()
                {
                    Id = 3,
                    MenuName = "View_Property"
                }
            , new Menu()
            {
                Id = 4,
                MenuName = "Edit_User"
            }
            , new Menu()
            {
                Id = 5,
                MenuName = "Add_Property"
            }
            , new Menu()
            {
                Id = 6,
                MenuName = "Edit_Property"
            }
            , new Menu()
            {
                Id = 7,
                MenuName = "ActDct_User"
            }, new Menu()
            {
                Id = 8,
                MenuName = "View_User_Detail"
            },
             new Menu()
             {
                 Id = 9,
                 MenuName = "Act_Deact_Property"
             }, new Menu()
             {
                 Id = 10,
                 MenuName = "Edit_Feature"
             }, new Menu()
             {
                 Id = 11,
                 MenuName = "Access_Setting"
             }, new Menu()
             {
                 Id = 12,
                 MenuName = "Create_WO"
             }, new Menu()
             {
                 Id = 13,
                 MenuName = "Get_WO"
             }, new Menu()
             {
                 Id = 14,
                 MenuName = "GetWO_Detail"
             }, new Menu()
             {
                 Id = 15,
                 MenuName = "Edit_WO"
             }, new Menu()
             {
                 Id = 16,
                 MenuName = "Post_Comment"
             }
             , new Menu()
             {
                 Id = 17,
                 MenuName = "Assign_To_User"
             }
              , new Menu()
              {
                  Id = 18,
                  MenuName = "WO_Operation"
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
                new RoleMenuMap { Id = 10, MenuId = 10, RoleId = 1 },
                new RoleMenuMap { Id = 11, MenuId = 11, RoleId = 1 },
                new RoleMenuMap { Id = 12, MenuId = 12, RoleId = 1 },
                new RoleMenuMap { Id = 13, MenuId = 13, RoleId = 1 },
                new RoleMenuMap { Id = 14, MenuId = 14, RoleId = 1 },
                new RoleMenuMap { Id = 15, MenuId = 15, RoleId = 1 },
                new RoleMenuMap { Id = 16, MenuId = 16, RoleId = 1 },
                new RoleMenuMap { Id = 17, MenuId = 17, RoleId = 1 },
                new RoleMenuMap { Id = 18, MenuId = 18, RoleId = 1 }
                );
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
            .Entries()
           .Where(e => e.Entity is Log && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));
            var user = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((Log)entityEntry.Entity).CreatedTime = DateTime.Now;
                    ((Log)entityEntry.Entity).UpdatedTime = DateTime.Now;
                    ((Log)entityEntry.Entity).CreatedByUserName = user != null ? user.Value : "System Admin";
                    ((Log)entityEntry.Entity).UpdatedByUserName = user?.Value;
                }
                if (entityEntry.State == EntityState.Modified)
                {
                    ((Log)entityEntry.Entity).UpdatedTime = DateTime.Now;
                    if (user != null)
                        ((Log)entityEntry.Entity).UpdatedByUserName = user != null ? user.Value : "System Admin";
                }
            }

            return (await base.SaveChangesAsync(true, cancellationToken));
        }
    }

}