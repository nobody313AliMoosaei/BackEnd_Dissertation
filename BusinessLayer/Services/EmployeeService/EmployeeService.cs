using BusinessLayer.Utilities;
using DataLayer.DataBase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public EmployeeService(Context_Project context, GeneralService.IGeneralService genericService)
        {
            _context = context;
            _genericService = genericService;
        }

        public async Task<List<BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetAllDissertationOfTeacher(long TeacherId, int PageNumber, int PageSize)
        {
            var model = new List<BusinessLayer.Models.OUTPUT.Dissertation.DissertationModelOutPut>();

            try
            {
                var ttt = await _context.Dissertations
                    .Include(o => o.Student)
                    .ThenInclude(o => o.Teachers)
                    .Where(o => o.Student.Teachers.Count > 0)
                    .ToListAsync();

                model = await _context.Dissertations
                    .Include(o => o.Student)
                    .ThenInclude(o => o.Teachers)
                    .Where(o => o.StatusDissertation < (int)DataLayer.Tools.Dissertation_Status.ConfirmationGuideMaster3
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

    }
}
