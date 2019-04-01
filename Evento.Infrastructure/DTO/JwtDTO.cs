namespace Evento.Infrastructure.DTO
{
    public class JwtDTO
    {
        public string Token { get; set; }
        public long Expires { get; set; } // Czas wyczerpania się tokena
    }
}
