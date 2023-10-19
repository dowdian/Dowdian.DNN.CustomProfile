// <copyright file="DnnFileSystemRepository.cs" company="Dowdian SRL">
// Copyright (c) Dowdian SRL. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using DotNetNuke.Framework;
using DotNetNuke.Services.FileSystem;

namespace Dowdian.Modules.CustomProfile.Repositories.Dnn
{
    /// <summary>
    /// IDnnFileRepository
    /// </summary>
    public partial interface IDnnFileSystemRepository
    {
        /// <summary>
        /// Use this to save a file to the file system
        /// </summary>
        /// <param name="folder">IFolderInfo folder</param>
        /// <param name="fileName">string fileName</param>
        /// <param name="fileContent">Stream fileContent</param>
        /// <param name="overwrite">bool overwrite</param>
        /// <param name="checkPermissions">bool checkPermissions</param>
        /// <param name="contentType">string contentType</param>
        /// <returns>IFileInfo</returns>
        IFileInfo AddFile(IFolderInfo folder, string fileName, Stream fileContent, bool overwrite, bool checkPermissions, string contentType);

        /// <summary>
        /// Use this to save a file to the file system
        /// </summary>
        /// <param name="folder">IFolderInfo folder</param>
        /// <param name="fileName">string fileName</param>
        /// <param name="fileContent">Stream fileContent</param>
        /// <returns>IFileInfo</returns>
        IFileInfo AddFile(IFolderInfo folder, string fileName, Stream fileContent);

        /// <summary>
        /// Use this to check if a file exists in the file system
        /// </summary>
        /// <param name="folder">IFolderInfo folder</param>
        /// <param name="fileName">string fileName</param>
        /// <returns>bool</returns>
        bool FileExists(IFolderInfo folder, string fileName);

        /// <summary>
        /// Use this to get a specific file
        /// </summary>
        /// <param name="fileId">int fileId</param>
        /// <returns>IFileInfo</returns>
        IFileInfo GetFile(int fileId);

        /// <summary>
        /// Use this to get a specific file
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="relativePath">string relativePath</param>
        /// <returns>IFileInfo</returns>
        IFileInfo GetFile(int portalId, string relativePath);

        /// <summary>
        /// Use this to copy some specific image files from the default Images/ directory to a newly created portal
        /// </summary>
        /// <param name="portalId">int portalId</param>
        void CopyImageFiles(int portalId);

        /// <summary>
        /// Use this to get the content of a specific file
        /// </summary>
        /// <param name="file">IFileInfo file</param>
        /// <returns>Stream</returns>
        Stream GetFileContent(IFileInfo file);

        /// <summary>
        /// Use this to send a file to the client as a response
        /// </summary>
        /// <param name="file">IFileInfo file</param>
        /// <param name="contentDisposition">ContentDisposition contentDisposition</param>
        void WriteFileToResponse(IFileInfo file, ContentDisposition contentDisposition);

        /// <summary>
        /// Use this to check if a folder exists in the file system
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="folderPath">string folderPath</param>
        /// <returns>bool</returns>
        bool FolderExists(int portalId, string folderPath);

        /// <summary>
        /// Use this to create a new folder in the file system
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="folderPath">string folderPath</param>
        /// <returns>IFolderInfo</returns>
        IFolderInfo AddFolder(int portalId, string folderPath);

        /// <summary>
        /// Use this to get a folder from the file system
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <param name="folderPath">string folderPath</param>
        /// <returns>IFolderInfo</returns>
        IFolderInfo GetFolder(int portalId, string folderPath);

        /// <summary>
        /// Synchronize the file system with the database for the given portal
        /// </summary>
        /// <param name="portalId">int portalId</param>
        /// <returns>int</returns>
        int Synchronize(int portalId);

        /// <summary>
        /// Use this to get all the files within a folder from the file system
        /// </summary>
        /// <param name="folder">IFolderInfo folder</param>
        /// <returns>IEnumerable of IFileInfo</returns>
        IEnumerable<IFileInfo> GetFiles(IFolderInfo folder);
    }

    /// <summary>
    /// DnnFileRepository
    /// </summary>
    public partial class DnnFileSystemRepository : ServiceLocator<IDnnFileSystemRepository, DnnFileSystemRepository>, IDnnFileSystemRepository
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public IFileInfo AddFile(IFolderInfo folder, string fileName, Stream fileContent, bool overwrite, bool checkPermissions, string contentType)
        {
            return FileManager.Instance.AddFile(folder, fileName, fileContent, overwrite, checkPermissions, contentType);
        }

        public IFileInfo AddFile(IFolderInfo folder, string fileName, Stream fileContent)
        {
            return FileManager.Instance.AddFile(folder, fileName, fileContent);
        }

        public bool FileExists(IFolderInfo folder, string fileName)
        {
            return FileManager.Instance.FileExists(folder, fileName);
        }

        public IFileInfo GetFile(int fileId)
        {
            return FileManager.Instance.GetFile(fileId);
        }

        public IFileInfo GetFile(int fileId, string relativePath)
        {
            return FileManager.Instance.GetFile(fileId, relativePath);
        }

        public void CopyImageFiles(int portalId)
        {
            var file = this.GetFile(-1, "Images/logotype_color_transparent.svg");
            var fileStream = this.GetFileContent(file);
            var folder = this.GetFolder(portalId, "Images/");
            this.AddFile(folder, file.FileName, fileStream);
        }

        public Stream GetFileContent(IFileInfo file)
        {
            return FileManager.Instance.GetFileContent(file);
        }

        public void WriteFileToResponse(IFileInfo file, ContentDisposition contentDisposition)
        {
            FileManager.Instance.WriteFileToResponse(file, contentDisposition);
        }

        public bool FolderExists(int portalId, string folderPath)
        {
            return FolderManager.Instance.FolderExists(portalId, folderPath);
        }

        public IFolderInfo AddFolder(int portalId, string folderPath)
        {
            return FolderManager.Instance.AddFolder(portalId, folderPath);
        }

        public IFolderInfo GetFolder(int portalId, string folderPath)
        {
            return FolderManager.Instance.GetFolder(portalId, folderPath);
        }

        public int Synchronize(int portalId)
        {
            return FolderManager.Instance.Synchronize(portalId);
        }

        public IEnumerable<IFileInfo> GetFiles(IFolderInfo folder)
        {
            return FolderManager.Instance.GetFiles(folder);
        }

        protected override Func<IDnnFileSystemRepository> GetFactory()
        {
            return () => new DnnFileSystemRepository();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}