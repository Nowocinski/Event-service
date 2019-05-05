using System.Threading.Tasks;

namespace Evento.Infrastructure.Commends
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<T>(T command) where T : ICommand;
    }
}
