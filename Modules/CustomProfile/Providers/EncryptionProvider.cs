// <copyright file="EncryptionProvider.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using DotNetNuke.Common.Utilities;

namespace Dowdian.Modules.CustomProfile.Providers
{
    /// <summary>
    /// EncryptionProvider
    /// </summary>
    public static class EncryptionProvider
    {
        private static readonly string EncryptionKey = Config.GetDecryptionkey();

        // The salt value used to strengthen the encryption.
        private static readonly byte[] Salt = Encoding.ASCII.GetBytes(EncryptionKey);
        private static readonly byte[] Key;
        private static readonly byte[] Iv;

        static EncryptionProvider()
        {
            var keyGenerator = new Rfc2898DeriveBytes(EncryptionKey, Salt);
            Key = keyGenerator.GetBytes(32);
            Iv = keyGenerator.GetBytes(16);
        }

        /// <summary>
        /// Encrypts any string using the Rijndael algorithm.
        /// A Base64 encrypted string.
        /// </summary>
        /// <param name="inputText">string inputText</param>
        /// <returns>string</returns>
        public static string Encrypt(string inputText)
        {
            if (inputText == null)
            {
                return null;
            }

            // Create a new RijndaelManaged cipher for the symmetric algorithm from the key and iv
            var rijndaelCipher = new RijndaelManaged { Key = Key, IV = Iv };

            var plainText = Encoding.Unicode.GetBytes(inputText);

            using (var encryptor = rijndaelCipher.CreateEncryptor())
            {
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainText, 0, plainText.Length);
                        cryptoStream.FlushFinalBlock();
                        var endValue = Convert.ToBase64String(memoryStream.ToArray());
                        return endValue;
                    }
                }
            }
        }

        /// <summary>
        /// Decrypts a previously encrypted string.
        /// </summary>
        /// <param name="inputText">The encrypted string to decrypt.</param>
        /// <returns>string</returns>
        public static string Decrypt(string inputText)
        {
            if (inputText == null)
            {
                return null;
            }

            var rijndaelCipher = new RijndaelManaged();
            var encryptedData = Convert.FromBase64String(inputText);

            using (var decryptor = rijndaelCipher.CreateDecryptor(Key, Iv))
            {
                using (var memoryStream = new MemoryStream(encryptedData))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var plainText = new byte[encryptedData.Length];
                        var decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                        var endValue = Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                        return endValue;
                    }
                }
            }
        }
    }
}