namespace Evento.Infrastructure.Commends.Users
{
    public class Login : ICommand
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
