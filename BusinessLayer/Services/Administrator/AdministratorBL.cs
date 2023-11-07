using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Administrator
{
    public class AdministratorBL
    {
        private DataLayer.DataBase.Context_Project _context;


        public AdministratorBL(DataLayer.DataBase.Context_Project context)
        {
            _context= context;
        }

    }
}
