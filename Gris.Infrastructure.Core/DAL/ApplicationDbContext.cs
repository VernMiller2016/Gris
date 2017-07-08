using Gris.Domain.Core.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gris.Infrastructure.Core.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Database.CommandTimeout = 600;
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException("modelBuilder");
            }

            // Keep this:
            modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

            // EF won't let us swap out IdentityUserRole for ApplicationUserRole here:
            modelBuilder.Entity<IdentityUser>().HasMany<IdentityUserRole>((IdentityUser u) => u.Roles);
            modelBuilder.Entity<IdentityUserRole>().HasKey((IdentityUserRole r) =>
                new { UserId = r.UserId, RoleId = r.RoleId }).ToTable("AspNetUserRoles");

            // Leave this alone:
            EntityTypeConfiguration<IdentityUserLogin> entityTypeConfiguration =
                modelBuilder.Entity<IdentityUserLogin>().HasKey((IdentityUserLogin l) =>
                    new
                    {
                        UserId = l.UserId,
                        LoginProvider = l.LoginProvider,
                        ProviderKey
                            = l.ProviderKey
                    }).ToTable("AspNetUserLogins");


            EntityTypeConfiguration<IdentityUserClaim> table1 =
                modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

            // Add this, so that IdentityRole can share a table with ApplicationRole:
            modelBuilder.Entity<IdentityRole>().ToTable("AspNetRoles");

            modelBuilder.Entity<ServerSalaryReportEntity>().ToTable("salaryReport");

            //modelBuilder.Entity<User>().
            // HasMany(u => u.Roles).
            // WithMany(r => r.Users).
            // Map(
            //  m =>
            //  {
            //      m.MapLeftKey("UserId");
            //      m.MapRightKey("RoleId");
            //      m.ToTable("UserRoles");
            //  });
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Program> Programs { get; set; }

        public System.Data.Entity.DbSet<PaySource> PaySources { get; set; }

        public System.Data.Entity.DbSet<PlaceOfService> PlaceOfServices { get; set; }

        public System.Data.Entity.DbSet<Server> Servers { get; set; }

        public System.Data.Entity.DbSet<Category> Categories { get; set; }

        public System.Data.Entity.DbSet<Service> Services { get; set; }

        public System.Data.Entity.DbSet<ServerTimeEntry> ServerTimeEntries { get; set; }

        public System.Data.Entity.DbSet<ServerAvailableHour> ServerAvailableHours { get; set; }

        public System.Data.Entity.DbSet<ServerSalaryReportEntity> ServerSalaryMonthlyReport { get; set; }
    }
}
