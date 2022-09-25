using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BackEndDotnetPlumsail.Data.Common
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
