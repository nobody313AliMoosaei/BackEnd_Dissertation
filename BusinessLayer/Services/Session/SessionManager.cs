using BusinessLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Session
{
    public class SessionManager : ISessionManager
    {
        private IHttpContextAccessor _contextAccessor;

        public SessionManager(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string? GetSession(string Key)
        {
            try
            {
                return _contextAccessor.HttpContext?.Session.GetString(Key);
            }catch
            {
                return null;
            }
        }

        public bool SetSession(string Key, string Value)
        {
            try
            {
                _contextAccessor.HttpContext?.Session.SetString(Key, Value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
