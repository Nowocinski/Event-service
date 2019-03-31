using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evento.Core.Domain
{
    public class Ticket : Entity
    {
        [ForeignKey("EventId")]
        public Event Relation { get; protected set; }

        //-----------------------------------------

        [Required] [Column(TypeName = "uniqueidentifier")]
        public Guid EventId { get; protected set; }

        [Required] [Column(TypeName = "int")]
        public int Seating { get; protected set; }

        [Required] [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; protected set; }

        [Column(TypeName = "uniqueidentifier")]
        public Guid? UserId { get; protected set; }

        [Column(TypeName = "nvarchar(40)")]
        public string Username { get; protected set; }

        [Column(TypeName = "datetime")]
        public DateTime? PurchasedAt { get; protected set; }

        [Required] [Column(TypeName = "BIT")]
        public bool Purchased => UserId.HasValue;

        public Ticket(Guid Id /*Event @event*/, int Seating, decimal Price)
        {
            EventId = Id /* @event.Id */;
            this.Seating = Seating;
            this.Price = Price;
        }
    }
}
