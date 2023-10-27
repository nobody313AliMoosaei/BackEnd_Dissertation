using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Session
{
    public interface ISessionManager
    {
        string GetSession(string Key);
        bool SetSession(string Key,string Value);
    }
}
