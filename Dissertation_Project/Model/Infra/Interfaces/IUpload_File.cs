namespace Dissertation_Project.Model.Infra.Interfaces
{
    public interface IUpload_File
    {
        public Task<Model.DTO.OUTPUT.UploadFile_Implement.Resualt_UploadFile>UploadFileAsync(IFormFile MainFile);
    }
}
