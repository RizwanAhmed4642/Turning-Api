using System;
using System.Collections.Generic;

namespace Meeting_App.Data.Database.Tables
{
    public partial class ViewFilesFolders
    {
        public int FolderId { get; set; }
        public string FolderDescripion { get; set; }
        public string FolderName { get; set; }
        public bool? FolderEnableFlag { get; set; }
        public DateTime? FolderCreatedAt { get; set; }
        public string FolderCreatedBy { get; set; }
        public string FolderCreaterUserName { get; set; }
        public int? FileId { get; set; }
        public string FileDescripion { get; set; }
        public string FileName { get; set; }
        public bool? FileEnableFlag { get; set; }
        public DateTime? FileCreatedAt { get; set; }
        public string FileCreatedBy { get; set; }
        public string FileCreaterUserName { get; set; }
        public string FilePath { get; set; }
        public string FileSize { get; set; }
        public string FileExtension { get; set; }
    }
}
