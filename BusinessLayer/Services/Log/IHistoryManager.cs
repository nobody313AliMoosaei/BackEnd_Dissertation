using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Log
{
    public interface IHistoryManager
    {
        Task InsertHistory(DateTime DateTime, string IP, string Url, string Level, string Client, string Message);
        Task SendEmail(string Email, string Title, string Dsr);
    }
}
