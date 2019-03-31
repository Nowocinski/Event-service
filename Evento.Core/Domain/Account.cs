using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evento.Core.Domain
{
    public class Account : Entity
    {
        [Required] [Column(TypeName = "nvarchar(30)")]
        public string Role { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(40)")]
        public string Name { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(50)")]
        public string Email { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(30)")]
        public string Password { get; protected set; }

        [Required] [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; protected set; }

        public Account(Guid Id, string Role, string Name, string Email, string Password)
        {
            this.Id = Id;
            this.Role = Role;
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
