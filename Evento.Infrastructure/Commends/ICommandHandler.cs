using System.Threading.Tasks;

namespace Evento.Infrastructure.Commends
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task HandleAsync(T command);
    }
}
