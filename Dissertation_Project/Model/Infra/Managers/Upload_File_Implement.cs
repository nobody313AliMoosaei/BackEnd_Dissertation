using Dissertation_Project.Model.DTO.OUTPUT.UploadFile_Implement;
using System.Data;

namespace Dissertation_Project.Model.Infra.Managers
{
    public class Upload_File_Implement : Interfaces.IUpload_File
    {
        private IWebHostEnvironment _webHostEnvironment;
        public Upload_File_Implement(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<Resualt_UploadFile> UploadFileAsync(IFormFile MainFile)
        {
            try
            {
                if (MainFile == null)
                {
                    return null;
                }

                FileInfo fileinfo = new FileInfo(MainFile.FileName);
                
                if (fileinfo.Extension.ToLower() == ".pdf"
                    || fileinfo.Extension.ToLower() == ".docx"
                    || fileinfo.Extension.ToLower() == ".docm"
                    || fileinfo.Extension.ToLower() == ".jpeg"
                    || fileinfo.Extension.ToLower() == ".jpg"
                    || fileinfo.Extension.ToLower() == ".png"
                    || true) // بعدا حذف شود
                {
                    string Filename = "", FileAddress = "";
                    string DateNow = Core.Utlities.Persian_Calender.Shamsi_Calender.GetDate_Shamsi();
                    Filename = $"{Guid.NewGuid().ToString()}{fileinfo.Extension}";

                    string Folder_Env = $"{_webHostEnvironment.WebRootPath}\\AllFiles\\{DateNow}";
                    FileAddress = Path.Combine(_webHostEnvironment.WebRootPath, $"AllFiles\\{DateNow}", Filename);

                    if (!Directory.Exists(Folder_Env))
                    {
                        Directory.CreateDirectory(Folder_Env);
                    }
                    var MemoryStream = new MemoryStream();
                    await MainFile.OpenReadStream().CopyToAsync(MemoryStream);

                    using (var fa = new FileStream(FileAddress, FileMode.Create, FileAccess.ReadWrite))
                    {
                        MemoryStream.WriteTo(fa);
                    }

                    Resualt_UploadFile model = new Resualt_UploadFile()
                    {
                        FileAddress = FileAddress,
                        FileName = Filename
                    };
                    return model;

                }
                else
                {
                    return null;
                }
            }catch
            {
                return null;
            }
        }
    }
}



/*
  public async Task<IActionResult> AddBlog(Add_Blog_DTO newblog)
        {
            if (newblog == null)
            {
                ModelState.AddModelError(string.Empty, "Modelstate is null");
                return View();
            }
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "is Not Invalid");
                return View();
            }

            var UserId = int.Parse(this.User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier).Value);
            var user = await _Context.Users.SingleOrDefaultAsync(t => t.Id == UserId);
            string FileName = "", path = "";
            //File Upload
            try
            {
                foreach (var file in newblog.File)
                {
                    FileInfo FileInfo = new FileInfo(file.FileName);
                    var t = FileInfo.Extension;
                    if (t.ToLower() == ".jpg"
                        || t.ToLower() == ".png"
                        || t.ToLower() == ".jpeg")
                    {

                        FileName = $"{Guid.NewGuid()}_{DateTime.Now.Day}{t}";
                        int year = Core_layer.Utlities.Shamsi_DateTime.GetPersianYear(DateTime.Now);
                        int Month = Core_layer.Utlities.Shamsi_DateTime.GetPersianMonth(DateTime.Now);
                        int Day = Core_layer.Utlities.Shamsi_DateTime.GetPersianDay(DateTime.Now);
                        string DateNow = $"{year}/{Month}/{Day}";

                        string FolderEnv = $"{_webHostEnvironment.WebRootPath}\\AllFiles\\{DateNow}";
                        path = Path.Combine(_webHostEnvironment.WebRootPath, $"AllFiles\\{DateNow}", FileName);

                        var memoryStream = new MemoryStream();
                        await file.OpenReadStream().CopyToAsync(memoryStream);

                        if (!Directory.Exists(FolderEnv))
                        {
                            Directory.CreateDirectory(FolderEnv);
                        }
                        using (var fa = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                        {
                            memoryStream.WriteTo(fa);
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                Blog Blog = new Blog()
                {
                    Description = newblog.Description,
                    FileAddress = path,
                    FileName = FileName,
                    Insert_Date = DateTime.Now,
                    Likes = 0,
                    Title = newblog.Title,
                    User = user
                };
                await _Context.Blogs.AddAsync(Blog);
                await _Context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), "Home");
            }
            catch
            {
                return BadRequest();
            }

        }
 */
