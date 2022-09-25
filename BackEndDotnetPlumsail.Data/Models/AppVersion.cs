using BackEndDotnetPlumsail.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEndDotnetPlumsail.Data.Models
{
    public class AppVersion : BaseModel
    {
        [StringLength(255)]
        public string Hash { get; set; }
        [MaxLength]
        public byte[] File { get; set; }
    }
}
