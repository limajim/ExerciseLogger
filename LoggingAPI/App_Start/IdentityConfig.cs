using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using LoggingAPI.Models;

namespace LoggingAPI
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    // Since we are extending IdentityUser, we need to make a manager to use the Extended class.
    public class JJUserManager : UserManager<User>
    {
        public JJUserManager(IUserStore<User> store)
            : base(store)
        {
        }

        public User FindByUserName(string userName)
        {
            return
                (this.Users.SingleOrDefault(
                    usr => usr.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase)));
        }

        public static JJUserManager Create(IdentityFactoryOptions<JJUserManager> options, IOwinContext context)
        {
            var manager = new JJUserManager(new UserStore<User>(context.Get<ExerciseDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Since we are extending Roles, we need to make a manager to use the Extended class.
    public class PermissionManager : RoleManager<Permission>
    {
        public PermissionManager(IRoleStore<Permission, string> roleStore)
            : base(roleStore)
        {
        }

        public static PermissionManager Create(
            IdentityFactoryOptions<PermissionManager> options, IOwinContext context)
        {
            return new PermissionManager(
                new RoleStore<Permission>(context.Get<ExerciseDbContext>()));
        }
    }


    public class ApplicationDbInitializer
   : DropCreateDatabaseIfModelChanges<ExerciseDbContext>
    {
        protected override void Seed(ExerciseDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(ExerciseDbContext db)
        {
            var userManager =
                HttpContext.Current.GetOwinContext()
                    .GetUserManager<JJUserManager>();
            var permissionManager =
                HttpContext.Current.GetOwinContext().Get<PermissionManager>();
            const string name = "jvandick";
            const string email = "jvandick@cscos.com";
            const string password = "Admin@123456";
            const string permissionName = "IsSuperUser";

            //Create permission if it does not exist
            var permission = permissionManager.FindByName(permissionName);
            if (permission == null)
            {
                permission = new Permission(permissionName);
                var roleresult = permissionManager.Create(permission);
            }

            var user = userManager.FindByName(name);
            if (user == null)
            {
                user = new User { UserName = name, Email = email,FirstName = "Jim",LastName = "Van Dick"};
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            // Add user admin to Role Admin if not already added
            var permissionsForUser = userManager.GetRoles(user.Id);
            if (!permissionsForUser.Contains(permission.Name))
            {
                var result = userManager.AddToRole(user.Id, permission.Name);
            }
        }
    }
}
