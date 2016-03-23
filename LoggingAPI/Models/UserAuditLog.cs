using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace LoggingAPI.Models
{
    public class UserAuditLog : AuditLog
    {
        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User UpdatedUser { get; set; }

        public IEnumerable<User> Users { set; get; }

    }
}
