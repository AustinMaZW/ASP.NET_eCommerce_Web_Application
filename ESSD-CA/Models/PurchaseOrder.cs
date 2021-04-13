﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class PurchaseOrder
    {
        [Key]
        [MaxLength(36)]
        public string OrderId { get; set; }
        public DateTime ProductDate { get; set; }
        public double GrandTotal { get; set; }
        public int Quantity { get; set; }
        public virtual Customer Customer { get; set; }

    }
}