using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace LoggingAPI.Models
{
    public class ExerciseSessionAuditLog : AuditLog
    {

        [ForeignKey("ExerciseSession")]
        public int ExerciseSessionId { get; set; }

        public virtual ExerciseSessionAuditLog ExerciseSession { get; set; }

    }
}
