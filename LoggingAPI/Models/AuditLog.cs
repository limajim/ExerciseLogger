using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace LoggingAPI.Models
{
    public abstract class AuditLog
    {
        public int Id { get; set; }
        [Required]
        public DateTime DateEntered { set; get; }
        [Required]
        public string EventLogInformation { get; set; }




        public string EditedByUserId { get; set; }
        [ForeignKey("EditedByUserId")]
        [InverseProperty("EditedByAuditLogs")]
        public virtual User EditedByUser { get; set; }

    }
}
