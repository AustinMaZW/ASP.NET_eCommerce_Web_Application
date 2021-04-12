using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class PurchaseOrderDetails
    {
        public string ActivationCode { get; set; }
        public virtual PurchaseOrder Order { get; set; }
        public virtual Product Product { get; set; }

    }
}
