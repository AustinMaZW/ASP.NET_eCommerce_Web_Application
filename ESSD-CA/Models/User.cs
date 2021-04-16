using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ESSD_CA.Models
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(36)]
        public string UserId { get; set; }

        [Required]
        [MaxLength(36)]
        public string Username { get; set; }
        [Required]
        [MaxLength(36)]
        public string Password { get; set; }
        [MaxLength(36)]
        public string SessionId { get; set; }

        public User(string Username)
        {
            this.UserId = Guid.NewGuid().ToString();
            this.Username = Username;
            this.Password = Username;
        }
    }

}