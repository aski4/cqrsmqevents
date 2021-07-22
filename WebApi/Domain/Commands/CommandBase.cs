using MediatR;

namespace Domain.Commands
{
    public abstract class CommandBase<T> : IRequest<T> where T : class { }
}
