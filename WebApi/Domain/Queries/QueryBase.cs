using MediatR;

namespace Domain.Queries
{
    public abstract class QueryBase<T> : IRequest<T> where T : class { }
}
