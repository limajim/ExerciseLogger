using System;
using System.Collections.Generic;

namespace LoggerLibrary.ViewModels
{
    public class AuditLogViewModel
    {
        public DateTime  DateEntered { get; set; }

        public string EditedBy { get; set; }

        public string EditedByUserId { get; set; }

        public string LogInfomormation { get; set; }
    }
}