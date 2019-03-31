using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Evento.Core.Domain
{
    public class Event : Entity
    {
        private ISet<Ticket> _tickets = new HashSet<Ticket>();

        [Required] [Column(TypeName = "nvarchar(40)")]
        public string Name { get; protected set; }

        [Required] [Column(TypeName = "nvarchar(100)")]
        public string Description { get; protected set; }

        [Required] [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; protected set; }

        [Required] [Column(TypeName = "datetime")]
        public DateTime StartDate { get; protected set; }

        [Required] [Column(TypeName = "datetime")]
        public DateTime EndDate { get; protected set; }

        [Required] [Column(TypeName = "datetime")]
        public DateTime UpdateAt { get; protected set; }

        public IEnumerable<Ticket> Tickets => _tickets;
        public IEnumerable<Ticket> PurchasedTickets => Tickets.Where(x => x.Purchased);
        public IEnumerable<Ticket> AvailableTickets => Tickets.Where(x => !x.Purchased);

        public void SetName(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new Exception($"Event with id: '{Id}' can not have an empty name.");

            this.Name = Name;
            UpdateAt = DateTime.UtcNow;
        }

        public void SetDescription(string Description)
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new Exception($"Event with id: '{Id}' can not have an empty decription.");

            this.Description = Description;
            UpdateAt = DateTime.UtcNow;
        }

        public void AddTickets(int amount, decimal price)
        {
            int seating = _tickets.Count + 1;
            for(int i=0; i<amount; i++)
            {
                _tickets.Add(new Ticket(Id, seating, price));
                seating++;
            }
        }

        public Event(Guid Id, string Name, string Description, DateTime StartDate, DateTime EndDate)
        {
            this.Id = Id;
            SetName(Name);
            SetDescription(Description);
            this.StartDate = StartDate;
            this.EndDate = EndDate;
            CreatedAt = DateTime.UtcNow;
            UpdateAt = DateTime.UtcNow;
        }

        //-------------------------------------------

        public void LoadTickets(List<Ticket> tickets)
        {
            foreach(Ticket t in tickets)
                _tickets.Add(t);
        }
    }
}
