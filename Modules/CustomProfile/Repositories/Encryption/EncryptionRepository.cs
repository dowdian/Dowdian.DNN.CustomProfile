// <copyright file="EncryptionRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using DotNetNuke.Framework;
using Dowdian.Modules.CustomProfile.Providers;

namespace Dowdian.Modules.CustomProfile.Repositories.Encryption
{
    /// <summary>
    /// IEncryptionRepository
    /// </summary>
    public partial interface IEncryptionRepository
    {
        /// <summary>
        /// Use this to encrypt the inputText vavlue
        /// </summary>
        /// <param name="inputText">string inputText</param>
        /// <returns>string</returns>
        string Encrypt(string inputText);

        /// <summary>
        /// Use this to decrypt the inputText vavlue
        /// </summary>
        /// <param name="inputText">string inputText</param>
        /// <returns>string</returns>
        string Decrypt(string inputText);
    }

    /// <summary>
    /// EncryptionRepository
    /// </summary>
    public partial class EncryptionRepository : ServiceLocator<IEncryptionRepository, EncryptionRepository>, IEncryptionRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public string Encrypt(string inputText)
        {
            return EncryptionProvider.Encrypt(inputText);
        }

        public string Decrypt(string inputText)
        {
            return EncryptionProvider.Decrypt(inputText);
        }

        protected override Func<IEncryptionRepository> GetFactory()
        {
            return () => new EncryptionRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}