using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class Product
    {
        [Required]
        [MaxLength(50)]
        public string Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string ProductName { get; set; }
        [MaxLength(200)]
        public string ProductDescription { get; set; }
        [Required]
        [MaxLength(10)]
        public double UnitPrice { get; set; }
        [MaxLength(128)]
        public string DownloadLink { get; set; }
        [MaxLength(128)]
        public string ImagePath { get; set; }

    }
}
