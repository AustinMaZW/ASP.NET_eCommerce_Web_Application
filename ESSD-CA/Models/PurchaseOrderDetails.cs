using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ESSD_CA.Db;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class PurchaseOrderDetails
    {
        [Key]
        [MaxLength(36)]
        public string ActivationCode { get; set; }


        [MaxLength(36)]
        public string OrderId { get; set; }
        //[ForeignKey("OrderId")]
        //public virtual PurchaseOrder Order { get; set; }


        [MaxLength(50)]
        public string ProductId { get; set; }
        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }

        public PurchaseOrderDetails()
        {

        }
    }
}