using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class CustomerMessage
    {
        [Required]
        [MaxLength(50)]
        public string Id { get; set; }
        
        /*[MaxLength(50)]
        public string GuestId { get; set; }
        
        [MaxLength(50)]
        public string UserId { get; set; }*/
        
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(12)]
        public string PhoneNumber { get; set; }
        
        [MaxLength(2000)]
        public string Message { get; set; }
    }
}
