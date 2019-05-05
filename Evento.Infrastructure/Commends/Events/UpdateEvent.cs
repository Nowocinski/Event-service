using System;

namespace Evento.Infrastructure.Commends.Events
{
    public class UpdateEvent : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
