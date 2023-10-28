using System.Text;

namespace DataLayer.Tools
{
    public enum Dissertation_Status
    {
        Register = 0,
        ConfirmationGuideMaster = 1,                    // تاییدیه استاد راهنمای اول
        ConfirmationGuideMaster2 = 2,                   // تاییدیه استاد راهنمای دوم
        ConfirmationGuideMaster3 = 3,                   // تاییدیه استاد راهنمای سوم
        ConfirmationEducationExpert = 4,                // تاییدیه کارشناس آموزش
        ConfirmationPostgraduateEducationExpert = 5,    // تاییدیه کارشناس تحصیلات تکمیلی
        ConfirmationDissertationExpert = 6,             // تاییدیه کارشناس امور پایان نامه
        ExpirDissertation = -3333                       // رد پایان نامه
    }
}