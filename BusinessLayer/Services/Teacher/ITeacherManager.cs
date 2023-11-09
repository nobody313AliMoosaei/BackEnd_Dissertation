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
        Task<ErrorsVM> AddNewTeacher(Models.INPUT.Teacher.TeacherInModelDTO newTeacher);

        // GetAllTeacher
        Task<List<Models.OUTPUT.Teacher.TeacherOutModelDTO>> GetAllTeachers();
        // Get Teachers Of one College
        Task<List<Models.OUTPUT.Teacher.TeacherOutModelDTO>> GetTeachersCollege(long CollegeRef);
        // Update Teacher
        Task<ErrorsVM> UpdateTeacher(long TeacherID, Models.INPUT.Teacher.TeacherInModelDTO teacher);
        // Delete Teacher
        Task<ErrorsVM> DeleteTeacher(long TeacherId);
    }
}
