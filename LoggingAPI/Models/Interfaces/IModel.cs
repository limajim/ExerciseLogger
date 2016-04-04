using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace LoggingAPI.Models.Interfaces
{
    /// <summary>
    /// Use generic type to define the type of form that will be created.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModel<T>
    {
        T ToForm();
    }
}