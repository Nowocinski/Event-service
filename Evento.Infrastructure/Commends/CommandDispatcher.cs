using System.Threading.Tasks;
using System;
using Autofac;

namespace Evento.Infrastructure.Commends
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _contexts;
        public CommandDispatcher(IComponentContext contexts)
        {
            _contexts = contexts;
        }
        public async Task DispatchAsync<T>(T command) where T : ICommand
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command), $"Comand {typeof(T).Name} can not be null.");
            }

            var handler = _contexts.Resolve<ICommandHandler<T>>();
            await handler.HandleAsync(command);
        }
    }
}
