using Microsoft.AspNetCore.Mvc;
using RealEstateWebAPI.Common.ErrorHandeling;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateWebAPI.BLL.Services
{
    public class ExceptionHandling
    {
        protected async Task<T> HandleAsync<T>(Func<Task<T>> action)
        {
            try
            {
                return await action();
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        protected async Task HandleAsync(Func<Task> action)
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                throw ex ;
            }
        }
    }
}

