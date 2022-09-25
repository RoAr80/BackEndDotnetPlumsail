using BackEndDotnetPlumsail.Data.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BackEndDotnetPlumsail.Data.Models
{
    public class Ration: BaseModel
    {
        [MaxLength]
        public string Json { get; set; }
        [MaxLength]
        public string Keywords { get; set; }
        public int? AppVersionId { get; set; }
        [ForeignKey("AppVersionId")]
        public virtual AppVersion AppVersion { get; set; }

    }
}
