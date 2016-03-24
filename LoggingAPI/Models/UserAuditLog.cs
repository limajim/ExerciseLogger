using System.ComponentModel.DataAnnotations.Schema;

namespace LoggingAPI.Models
{
    public class UserAuditLog : AuditLog
    {
        public string UserIdUpdated { get; set; }
        [ForeignKey("UserIdUpdated")]
        [InverseProperty("UpdatedAuditLogs")]
        public virtual User UpdatedUser { get; set; }

    }
}