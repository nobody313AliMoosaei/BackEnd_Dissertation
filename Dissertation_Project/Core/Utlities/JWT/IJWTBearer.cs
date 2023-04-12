namespace Dissertation_Project.Core.Utlities.JWT
{
    public interface IJWTBearer
    {
        public string GetUserToken(ulong Id, string UserName);
        public string GetUserToken(ulong Id, string UserName, string Role);
        public string GetUserToken(ulong Id , string UserName, IList<string> Roles);
    }
}
