using Microsoft.EntityFrameworkCore;

namespace Evento.Core.Domain
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Event>()
            .HasMany(a => a.Tickets)            // Lista biletów w wydarzeniu
            .WithOne(b => b.Relation)           // Relacja biletu do wydarzenia

            .OnDelete(DeleteBehavior.Restrict); // Usuwanie wydarzeń wraz z listą biletów*/

            modelBuilder.Entity<Event>()
            .HasMany(p => p.Tickets)
            .WithOne(b => b.Relation)
            .HasForeignKey(p => p.EventId)
            .OnDelete(DeleteBehavior.Restrict);
            //.HasConstraintName("ForeignKey_Post_Blog");
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
    }
}
