using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class FileDTO
    {
        public int Int { get; set; }
        public int PkCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public string Extension { get; set; }
        public string FileSize { get; set; }
        public int Fk_Folder { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public List<IFormFile>  FileAttachments { get; set; }
        public string FileAttachmentName { get; set; }
        public DateTime? UpdateAt { get; set; }
        public bool? EnableFlag { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string DeletedBy { get; set; }

    }

  
}
