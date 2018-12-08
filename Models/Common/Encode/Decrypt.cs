namespace Models.Common
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public static class Decrypt
    {
        public static string Decrypt_Code(string toDecrypt, bool useHashing = true)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
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
            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
