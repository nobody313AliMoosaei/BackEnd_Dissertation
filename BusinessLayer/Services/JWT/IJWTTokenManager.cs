using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.JWT
{
    public interface IJWTTokenManager
    {
        public string GetUserToken(long Id, string UserName);
        public string GetUserToken(long Id, string UserName, string Role);
        public string GetUserToken(long Id, string UserName, IList<string> Roles);
    }
}
