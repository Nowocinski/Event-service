using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evento.Core.Domain
{
    public abstract class Entity
    {
        [Key] [Column(TypeName = "uniqueidentifier")]
        public Guid Id { get; set; }
    }
}
