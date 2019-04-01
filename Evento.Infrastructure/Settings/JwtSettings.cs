namespace Evento.Infrastructure.Settings
{
    public class JwtSettings
    {
        public int ExpiryInMinutes { get; set; }
        public string SigningKey { get; set; }
        public string Site { get; set; }
    }
}
