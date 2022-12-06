using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Meeting_App.Models.DTOs
{
    public class FolderDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? FK_ParenId { get; set; }
        public int? countFiles { get; set; }
        public int? countFolders { get; set; }
        public FolderDTO ParentFolder { get; set; }

        public string UserId { get; set; }
    }
}
