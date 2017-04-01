namespace Gris.Infrastructure.Core.Migrations
{
    using Domain.Core.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Gris.Infrastructure.Core.DAL.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Gris.Infrastructure.Core.DAL.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.Categories.AddOrUpdate(
              c => c.Id,
              new Category { Id = 1,Name= "Combined-admin" },
              new Category { Id = 2, Name = "MH-admin" },
              new Category { Id =3, Name = "MED-admin" },
              new Category { Id = 4, Name = "CD-Admin" }
            );

            var UserStore = new UserStore<ApplicationUser>(context);
            var UserManager = new UserManager<ApplicationUser>(UserStore);

            string[] systemRoles = new string[] { "Admin", "Clerk" };
            List<string> names = new List<string>();
            names.Add("Admin");
            names.Add("Clerk");

            for (int i = 0; i < systemRoles.Count(); i++)
            {
                string sysRoelName = systemRoles[i];
                IdentityRole sysRole = context.Roles.Where(m => m.Name == sysRoelName).FirstOrDefault();
                if (sysRole == null)
                {
                    sysRole = new IdentityRole(sysRoelName);
                    context.Roles.AddOrUpdate(sysRole);
                    context.SaveChanges();
                    sysRole = context.Roles.Where(m => m.Name == sysRoelName).FirstOrDefault();

                    string userName = names[i];
                    int deptId = i + 1;
                    if (deptId > 5)
                        deptId += 1;
                    ApplicationUser defaultUsers = new ApplicationUser { UserName = userName, Email = userName + "@gris.com", FullName = "Test User", PhoneNumber = "054863478", LockoutEnabled = true };
                    var result = UserManager.Create(defaultUsers, "123456");
                    if (result.Succeeded)
                    {
                        var roelResult = UserManager.AddToRoles(defaultUsers.Id, sysRole.Name);
                    }

                }
            }

        }
    }
}
