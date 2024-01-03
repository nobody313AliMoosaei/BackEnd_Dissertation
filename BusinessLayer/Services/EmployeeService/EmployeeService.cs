using BusinessLayer.Utilities;
using DataLayer.DataBase;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.EmployeeService
{
    public class EmployeeService
    {
        private Context_Project _context;
        private GeneralService.IGeneralService _genericService;
        private UserManager<Users> _userManager;
        public EmployeeService(Context_Project context, GeneralService.IGeneralService genericService, UserManager<Users> usermanager)
        {
            _context = context;
            _genericService = genericService;
            _userManager = usermanager;
        }

        public async Task<List<BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetAllDissertationOfTeacher(long TeacherId, int PageNumber, int PageSize)
        {
            var model = new List<BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut>();

            try
            {
                model = await _context.Dissertations
                    .Include(o => o.Student)
                    .ThenInclude(o => o.Teachers)
                    .Where(o => o.Student.Teachers.Count > 0
                                && o.StatusDissertation < (int)DataLayer.Tools.Dissertation_Status.ConfirmationGuideMaster3
                                && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register
                                && o.Student.Teachers.Any(t => t.TeacherId == TeacherId))
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .Select(o => new BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut
                    {
                        TitleEnglish = o.TitleEnglish,
                        TitlePersian = o.TitlePersian,
                        Abstract = o.Abstract,
                        DateStr = o.RegisterDateTime.Value.ToShortDateString(),
                        DissertationFileAddress = o.DissertationFileAddress,
                        ProceedingsFileAddress = o.ProceedingsFileAddress,
                        DissertationId = o.DissertationId,
                        StatusDissertation = o.StatusDissertation,
                        StudentId = o.StudentId,
                        TermNumber = o.TermNumber,
                        TimeStr = o.RegisterDateTime.Value.ToShortTimeString(),
                    }).ToListAsync();

                var AllDissertationStatus = await _genericService.GetAllDissertationStatus();

                if (AllDissertationStatus != null && AllDissertationStatus.Count > 0)
                    model.ForEach(o =>
                    {
                        o.DisplayStatusDissertation = AllDissertationStatus.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                    });
            }
            catch
            {

            }
            return model;

        }

        public async Task<List<BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut>>
            GetAllDissertationsOtherEmployee(long UserId, int PageNumber, int PageSize)
        {
            var lstDissertations = new
                List<BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                var LstRoles = await _userManager.GetRolesAsync(user);
                List<int> Dis_Status = new List<int>();

                if (LstRoles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.EducationExpert.ToString().ToLower()))
                    Dis_Status.Add((int)DataLayer.Tools.Dissertation_Status.ConfirmationEducationExpert);

                if (LstRoles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.PostgraduateEducationExpert.ToString().ToLower()))
                    Dis_Status.Add((int)DataLayer.Tools.Dissertation_Status.ConfirmationPostgraduateEducationExpert);

                if (LstRoles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.DissertationExpert.ToString().ToLower()))
                    Dis_Status.Add((int)DataLayer.Tools.Dissertation_Status.ConfirmationDissertationExpert);


                lstDissertations = await _context.Dissertations
                        .Where(o => Dis_Status.Any(t => t == o.StatusDissertation))
                        .Skip((PageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .Select(o => new BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut
                        {
                            TitleEnglish = o.TitleEnglish,
                            TitlePersian = o.TitlePersian,
                            Abstract = o.Abstract,
                            DateStr = o.RegisterDateTime.Value.ToShortDateString(),
                            DissertationFileAddress = o.DissertationFileAddress,
                            ProceedingsFileAddress = o.ProceedingsFileAddress,
                            DissertationId = o.DissertationId,
                            StatusDissertation = o.StatusDissertation,
                            StudentId = o.StudentId,
                            TermNumber = o.TermNumber,
                            TimeStr = o.RegisterDateTime.Value.ToShortTimeString(),
                        }).ToListAsync();

                var AllDissertationStatus = await _genericService.GetAllDissertationStatus();

                if (AllDissertationStatus != null && AllDissertationStatus.Count > 0)
                    lstDissertations.ForEach(o =>
                    {
                        o.DisplayStatusDissertation = AllDissertationStatus.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                    });
            }
            catch
            {

            }
            return lstDissertations;
        }

    }
}
