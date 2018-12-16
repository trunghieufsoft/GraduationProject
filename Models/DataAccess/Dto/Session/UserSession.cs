namespace Models.DataAccess.Dto
{
    public class UserSession
    {
        public UserSession(string userid, string username, byte grantid)
        {
            UserID = userid;
            UserName = username;
            GrantID = grantid;
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public int GrantID { get; set; }
    }
}