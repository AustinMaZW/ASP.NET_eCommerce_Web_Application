using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class HistoryViewModel
    {
        public Product Product { get; set; }

        public PurchaseOrder Order { get; set; }

        public List<string> ActivationCdList { get; set; }
    }
}