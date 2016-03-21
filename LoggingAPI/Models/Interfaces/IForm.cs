using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace LoggingAPI.Models.Interfaces
{
    public interface IForm
    {
        [Required]
        string CurrentUserId { set; get; }
        [Required]
        byte[] RowVersion { get; set; }
    }
}