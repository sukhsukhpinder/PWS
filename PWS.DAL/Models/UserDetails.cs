using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PWS.DAL.Models
{
    public partial class UserDetails
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("username")]
        [StringLength(150)]
        public string Username { get; set; }
        [Required]
        [Column("password")]
        [StringLength(150)]
        public string Password { get; set; }
        [Column("isActive")]
        public bool IsActive { get; set; }
    }
}
