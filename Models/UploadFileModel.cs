using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EDProject1.Models
{
    public class UploadFileModel
    {
        public IFormFile File { get; set; }
        public bool IsHeader { get; set; }
    }
}
