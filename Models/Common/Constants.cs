namespace Models.Common
{
    public static class Constants
    {
        public static int minusThreeNumber = -3;
        public static int minusTwoNumber = -2;
        public static int minusOneNumber = -1;
        public static int zeroNumber = 0;
        public static int oneNumber = 1;
        public static int twoNumber = 2;
        public static int threeNumber = 3;
        public static int extremely = -9999;
        public static string nullValue = null;
        public static string stringEmpty = "";
        public static bool trueValue = true;
        public static bool falseValue = false;
        public static string ENCODE_MD5 = "ENCODE_MD5";
        public static string USER_SESSION = "USER_SESSION";
        public static string CART_SESSION = "CART_SESSION";
        public static int PageSize = 10;

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