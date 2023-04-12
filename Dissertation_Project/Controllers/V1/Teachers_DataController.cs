using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class Teachers_DataController : ControllerBase
    {
        #region IOC

        private DataLayer.DataBase.Context_Project _context;
        private UserManager<DataLayer.Entities.Users> _userManager;
        public Teachers_DataController(DataLayer.DataBase.Context_Project context
            ,UserManager<DataLayer.Entities.Users>usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }
        #endregion

        public async Task<IActionResult> GetAllTeacher()
        {
            return Ok((await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString())).ToList());
        }
    }
}
