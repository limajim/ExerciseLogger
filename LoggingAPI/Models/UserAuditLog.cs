using System.ComponentModel.DataAnnotations.Schema;

namespace LoggingAPI.Models
{
    public class UserAuditLog : AuditLog
    {
        [ForeignKey("UpdatedUser"), Column(Order = 1)]
        public string UpdatedUserId { get; set; }

        public virtual User UpdatedUser { get; set; }
    }
}