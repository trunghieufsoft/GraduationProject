namespace Models.DataAccess.Dto
{
    public class UserSession
    {
        public UserSession(string username, byte grantid)
        {
            UserName = username;
            GrantID = grantid;
        }

        public string UserName { get; set; }
        public int GrantID { get; set; }
    }
}