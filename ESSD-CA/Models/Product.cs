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
        public string Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        [Required]
        public double UnitPrice { get; set; }
        public string DownloadLink { get; set; }
        public string ImagePath { get; set; }

    }
}
