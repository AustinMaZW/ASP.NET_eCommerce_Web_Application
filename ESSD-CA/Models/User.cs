using ESSD_CA.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
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
        public string Password { get; set; }
        [MaxLength(36)]
        public string SessionId { get; set; }

        [MaxLength(36)]
        public string AccountType { get; set; }

        public User(string Username)
        {
            this.UserId = Guid.NewGuid().ToString();
            this.Username = Username;
            using (MD5 md5Hash = MD5.Create())
            {
                string hashPassword = MD5Hash.Md5hash(md5Hash, Username);
                this.Password = hashPassword;
            }
            this.AccountType = "User";
        }
    }

}