namespace Models.Common
{
    public static class Constants
    {
        public static string stringEmpty = "";
        public static string ENCODE_MD5 = "ENCODE_MD5";
        public static string USER_SESSION = "USER_SESSION";
        public static string CART_SESSION = "CART_SESSION";
        public static int PageSize = 10;
        public static int RecommendationsNumber = 30;

        public static string Grant = "Grant";
        public enum LoginState
        {
            UsernameAndPasswordNull,
            UsernameNull,
            PasswordNull,
            Successed,
            Failed,
            Locked
        }
        public enum GrantID
        {
            Manager = 1,
            Staff,
            User
        }
    }
}