using Microsoft.EntityFrameworkCore;

namespace Evento.Core.Domain
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
            .HasMany(a => a.Tickets)            // Lista biletów w wydarzeniu
            .WithOne(b => b.Relation)           // Relacja biletu do wydarzenia

            .OnDelete(DeleteBehavior.Restrict); // Usuwanie wydarzeń wraz z listą biletów
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
