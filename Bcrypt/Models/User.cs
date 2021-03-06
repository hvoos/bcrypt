using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bcrypt.Models
{
    [Table("user")]
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PassHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
