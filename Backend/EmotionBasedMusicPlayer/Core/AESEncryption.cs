using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace EmotionBasedMusicPlayer.Core
{
    public static class AesEncryption
    {
        #region Constants
        private const string defaultIV = "oYvytYmGsQvFwBdkzLRVRQ==";
        private const string defaultKey = "RbP23vJA2jQUlohg33tptPLX6yvJTiCN3Sx+U/8GRbQ=";
        #endregion

        #region Methods
        public static string Encrypt(string text)
        {
            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Convert.FromBase64String(defaultKey), Convert.FromBase64String(defaultIV));
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(text);
                        encrypted = ms.ToArray();
                    }
                }
                return Convert.ToBase64String(encrypted);
            }

        }

        public static string Decrypt(string text)
        {
            byte[] textByte = Convert.FromBase64String(text);
            string decryptedText = null;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(Convert.FromBase64String(defaultKey), Convert.FromBase64String(defaultIV));

                using (MemoryStream ms = new MemoryStream(textByte))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            decryptedText = reader.ReadToEnd();
                    }
                }
            }
            return decryptedText;
        }
        #endregion
    }
}