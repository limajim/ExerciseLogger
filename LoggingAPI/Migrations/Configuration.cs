using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Mime;
using LoggingAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LoggingAPI.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ExerciseDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ExerciseDbContext context)
        {
            //Add Permissions to the database
            var permissionManager = new PermissionManager(new RoleStore<Permission>(context));
            //Work Orders
            if (!permissionManager.RoleExists("IsSuperUser"))
            {
                permissionManager.Create(new Permission("IsSuperUser")
                {
                    Description = "Those assigned this permission are allowed to do anything"
                });
            }


            if (!permissionManager.RoleExists("CanEditSessions"))
            {
                permissionManager.Create(new Permission("CanEditSessions")
                {
                    Description = "Those assigned this permission are allowed to edit a Sessions"
                });
            }

            if (!permissionManager.RoleExists("CanCreateSessions"))
            {
                permissionManager.Create(new Permission("CanCreateSessions") {Description = "The User Can Create a session."});
            }

            if (!permissionManager.RoleExists("CanCreateExersizeTypes"))
            {
                permissionManager.Create(new Permission("CanCreateExersizeTypes") { Description = "The User Can Create an exercise type." });
            }

            if (!permissionManager.RoleExists("CanEditExersizeTypes"))
            {
                permissionManager.Create(new Permission("CanEditExersizeTypes") { Description = "The User Can Edit an exercise type." });
            }

            if (!permissionManager.RoleExists("CanDeleteExersizeTypes"))
            {
                permissionManager.Create(new Permission("CanDeleteExersizeTypes") { Description = "The User Can Delete an exercise type." });
            }

        }
    }
}