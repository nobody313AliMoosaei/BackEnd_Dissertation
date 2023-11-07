using BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Teacher
{
    public interface ITeacherManager
    {
        Task<ErrorsVM> AddNewTeacher();

        // GetAllTeacher

        // Get Teachers Of one College

        // Update Teacher

        // Delete Teacher
        Task<ErrorsVM> DeleteTeacher(long TeacherId);
    }
}
