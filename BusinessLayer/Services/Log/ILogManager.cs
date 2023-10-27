using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Log
{
    public interface ILogManager
    {
        Task<bool> InsertLogInDatabase(BusinessLayer.Models.INPUT.Log.InsertLogDTO InsertLog);
    }
}
