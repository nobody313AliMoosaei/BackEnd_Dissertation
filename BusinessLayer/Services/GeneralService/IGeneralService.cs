using BusinessLayer.Models;
using BusinessLayer.Models.OUTPUT.Teacher;
using BusinessLayer.Services.UploadFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.GeneralService
{
    public interface IGeneralService
    {
        // Display Dissertation status
        Task<string?> DisplayDissertationstatus(int code);
        Task<List<DataLayer.Entities.Baslookup>?> GetAllDissertationStatus();
        // Display CollegeUni
        Task<string?> DisplayCollegeUni(int code);
        Task<List<DataLayer.Entities.Baslookup>?> GetAllCollegeUni();
        // Add Role To User
        Task<ErrorsVM> AddRoleToUser(DataLayer.Entities.Users? user, string RoleUser);
        
        // LogOut
        Task<ErrorsVM> LogOut();
        
        // Change Status Dissertation
        Task<ErrorsVM> ChangeDissertationStatus(long DissertationID, DataLayer.Tools.Dissertation_Status DissertationStatus);
        
        // Upload File
        Task<ResultUploadFile> UploadFile(IFormFile MainFile);
        
        // Get Teachers
        Task<List<TeacherOutModelDTO>> GetAllTeacher();
        Task<List<TeacherOutModelDTO>> GetCollegeTeacher(long CollegeId);
        
        // send Comment
        Task<ErrorsVM> SendComment(long UserId, long DissertationId, long CommentId = 0);

        // Add Teacher To User 


    }
}
