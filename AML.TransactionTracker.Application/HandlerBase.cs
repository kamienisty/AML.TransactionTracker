using AML.TransactionTracker.Core.Repositories;

namespace AML.TransactionTracker.Application
{
    public abstract class HandlerBase<T> where T : IRepository
    {
        protected readonly T _repository;

        protected HandlerBase(T repository)
        {
            _repository = repository;
        }
    }
}
