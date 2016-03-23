using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace LoggingAPI.Models
{
    public abstract class AuditLog
    {
        public int Id { get; set; }
        [Required]
        [ForeignKey("User")]
        public string EditedByUserId { get; set; }
        [Required]
        public DateTime DateEntered { set; get; }
        [Required]
        public string EventLogInformation { get; set; }

        public virtual User User { get; set; }
    }
}
