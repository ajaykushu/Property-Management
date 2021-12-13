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
        public DbSet<Inspection> Inspections { set; get; }
        public DbSet<CheckList> CheckLists { set; get; }
        public DbSet<Languages> Languages { set; get; }
        public DbSet<Location> Locations { set; get; }
        public DbSet<Menu> Menu { set; get; }
        public DbSet<Property> Properties { set; get; }
        public DbSet<Reply> Replies { set; get; }
        public DbSet<RoleMenuMap> RoleMenuMaps { set; get; }
        public DbSet<Status> Statuses { set; get; }
        public DbSet<UserProperty> UserProperties { set; get; }
        public DbSet<WorkOrder> WorkOrders { set; get; }
        public DbSet<InspectionQueue> InspectionQueues { set; get; }
        public DbSet<CheckListQueue> CheckListQueues { get; set; }
        public DbSet<RecurringWO> RecurringWOs { set; get; }
        public DbSet<Effort> Efforts { get; set; }

        public DbSet<Notification> Notifications { set; get; }
        public DbSet<History> Histories { get; set; }

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
            builder.Entity<ApplicationUser>().Property(x => x.IsEffortVisible).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().HasIndex(x => x.PhoneNumber).IsUnique();
            builder.Entity<Property>().HasIndex(x => x.PropertyName).IsUnique();
            builder.Entity<PropertyType>().HasIndex(x => x.PropertyTypeName).IsUnique();
            builder.Entity<ApplicationUser>().Property(x => x.SMSAltert).HasDefaultValue(false);
            builder.Entity<ApplicationUser>().Property(x => x.ClockType).HasDefaultValue(12);
            
            builder.Entity<ApplicationUser>().Property(x => x.LanguageId).HasDefaultValue(1);
            builder.Entity<ApplicationUser>().Property(x => x.IsActive).HasDefaultValue(true);
            builder.Entity<WorkOrder>().Property(x => x.IsActive).HasDefaultValue(true);
            builder.Entity<UserProperty>().Property(x => x.IsPrimary).HasDefaultValue(false);

            builder.Entity<Property>().Property(x => x.IsActive).HasDefaultValue(true);
            builder.Entity<WorkOrder>().Property(x => x.Priority).HasDefaultValue(0);
            builder.Entity<WorkOrder>().Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("Concat('WO', NEXT VALUE FOR workordersequence)");
            builder.Entity<RecurringWO>().Property(x => x.Id).ValueGeneratedOnAdd().HasDefaultValueSql("Concat('RWO', NEXT VALUE FOR workordersequence)");
            DataSeeder(builder);
            //deleting cascade
            builder.Entity<Issue>().HasOne<Item>(x => x.Item).WithMany(d => d.Issues).HasForeignKey(x => x.ItemId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<SubLocation>().HasOne<Location>(x => x.Location).WithMany(d => d.SubLocations).HasForeignKey(x => x.LocationId).IsRequired(true).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Item>().Property(x => x.Active).HasDefaultValue(true);
            builder.Entity<Location>().Property(x => x.Active).HasDefaultValue(true);

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
                 .HasData(new Issue() { Id = 1, IssueName = "Power Problem",ItemId=1 }, new Issue() { Id = 2, IssueName = "Item Not Available",ItemId=2 });
            builder.Entity<Status>().HasData(new Status() { Id = 1, StatusCode = "ADCM", StatusDescription = "Add Comment Only" }, new Status() { Id = 2, StatusCode = "BINE", StatusDescription = "Bid Needed" }, new Status() { Id = 3, StatusCode = "BIRE", StatusDescription = "Bid Recieved" },
                new Status() { Id = 4, StatusCode = "BIRQ", StatusDescription = "Bid Requested" },
                new Status() { Id = 5, StatusCode = "COMP", StatusDescription = "Complete" },
                new Status() { Id = 6, StatusCode = "COBQ", StatusDescription = "Complete But Bad Quality" },
                new Status() { Id = 7, StatusCode = "CONI", StatusDescription = "Complete, Need Inspection" },
                new Status() { Id = 8, StatusCode = "DAIL", StatusDescription = "Daily" },
                new Status() { Id = 9, StatusCode = "FISC", StatusDescription = "Finalize Scope" },
                new Status() { Id = 10, StatusCode = "HOLD", StatusDescription = "Hold" },
                new Status() { Id = 11, StatusCode = "NEWO", StatusDescription = "New Work Order" },
                new Status() { Id = 12, StatusCode = "ORMA", StatusDescription = "Order Materials" },
                new Status() { Id = 13, StatusCode = "PEND", StatusDescription = "Pending" },
                new Status() { Id = 14, StatusCode = "REAS", StatusDescription = "Ready To Assign" },
                new Status() { Id = 15, StatusCode = "WOAS", StatusDescription = "Work Assigned" },
                new Status() { Id = 16, StatusCode = "WOPR", StatusDescription = "Work In Progress" },
                new Status() { Id = 17, StatusCode = "WOOR", StatusDescription = "Work Ordered" }
                );

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
                    Id = 13,
                    MenuName = "Create_WO"
                }, new Menu()
                {
                    Id = 12,
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
                },
                 new Menu()
                 {
                     Id = 19,
                     MenuName = "Recurring_WO"
                 },
                  new Menu()
                  {
                      Id = 20,
                      MenuName = "Completed_WO"
                  },
                   new Menu()
                   {
                       Id = 21,
                       MenuName = "All_WO"
                   }, new Menu()
                   {
                       Id = 22,
                       MenuName = "Effort"
                   }, new Menu()
                   {
                Id = 24,
                       MenuName = "Inspections"
                   }
                   , new Menu()
                   {
                       Id = 23,
                       MenuName = "Dashboard"
                   }
                   ); 


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
                new RoleMenuMap { Id = 18, MenuId = 18, RoleId = 1 },
                new RoleMenuMap { Id = 19, MenuId = 19, RoleId = 1 },
                new RoleMenuMap { Id = 20, MenuId = 20, RoleId = 1 },
                new RoleMenuMap { Id = 21, MenuId = 21, RoleId = 1 },
                new RoleMenuMap { Id = 22, MenuId = 22, RoleId = 1 }
                );
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
            .Entries()
           .Where(e => e.Entity is Log && (
                e.State == EntityState.Added
                || e.State == EntityState.Modified));
            var user = _httpContextAccessor.HttpContext?.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);

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
