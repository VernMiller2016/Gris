namespace Gris.Infrastructure.Core.Migrations
{
    using Domain.Core.Models;
    using System;
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

        }
    }
}
