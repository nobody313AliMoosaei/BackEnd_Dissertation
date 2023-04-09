namespace Dissertation_Project.Core.Utlities.JWT
{
    public interface IJWTBearer
    {
        public string GetUserToken(ulong Id, string UserName);
    }
}
