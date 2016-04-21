using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;
using LoggerLibrary.Forms;


namespace LoggerLibrary.Interfaces
{
    public interface IForm
    {
        [Required]
        UserForm CurrentUser { set; get; }
        [Required]
        byte[] RowVersion { get; set; }
    }
}