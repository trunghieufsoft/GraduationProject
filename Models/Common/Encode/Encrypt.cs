namespace Models.Common
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Encrypt
    {
        public static string Encrypt_Code(string toEncrypt, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                var hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(Constants.ENCODE_MD5));
            }
            else keyArray = Encoding.UTF8.GetBytes(Constants.ENCODE_MD5);
            var tdes = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
    }
}
