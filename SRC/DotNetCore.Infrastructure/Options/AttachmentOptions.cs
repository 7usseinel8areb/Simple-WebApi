using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.Infrastructure.Options
{
    public class AttachmentOptions
    {
        public string AllowedExtentions { get; set; }
        public int MaxSizeInMegaBytes { get; set; }
        public bool EnableCompression { get; set; }
    }
}
