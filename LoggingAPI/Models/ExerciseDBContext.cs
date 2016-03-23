using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LoggingAPI.Models
{
    // Note it is an Identity DB Context.
    public class ExerciseDbContext : IdentityDbContext<User>
    {
        public ExerciseDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<ExerciseSession> ExerciseSessions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<UserAuditLog> UserAuditLogs { get; set; }
        public DbSet<ExerciseSessionAuditLog> ExerciseSessionAuditLog { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            Database.SetInitializer<ExerciseDbContext>(null);

            //modelBuilder.Entity<User>()
            //    .HasOptional(a => a.EditedByAuditLogs)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<User>()
            //    .HasOptional(a => a.EditedByAuditLogs)
            //    .WithOptionalDependent()
            //    .WillCascadeOnDelete(true);

            //modelBuilder.Entity<UserAuditLog>()
            //        .HasRequired(u => u.UpdatedUser)
            //        .WithMany(u => u.UpdatedAuditLogs)
            //        .HasForeignKey(u => u.UpdatedUserId)
            //        .WillCascadeOnDelete(true);

            modelBuilder.Entity<UserAuditLog>()
                        .HasRequired(u => u.EditedByUser)
                        .WithMany(u => u.EditedByAuditLogs)
                        .HasForeignKey(u => u.EditedByUserId)
                        .WillCascadeOnDelete(true);
        }


        public static ExerciseDbContext Create()
        {
            return new ExerciseDbContext();
        }
    }
}