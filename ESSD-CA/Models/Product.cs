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
        [MaxLength(50)]
        public string ProductName { get; set; }

        [MaxLength(300)]
        public string ProductDescription { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Price must be at least $0.01")]
        public double UnitPrice { get; set; }
        [MaxLength(200)]
        public string DownloadLink { get; set; }
        [MaxLength(200)]
        public string ImagePath { get; set; }
        [Required]
        [MaxLength(30)]
        public string ProductStatus { get; set; }

    }
}