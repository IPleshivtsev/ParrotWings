using Microsoft.EntityFrameworkCore.Query;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace ParrotWingsApi.Test.Helpers;

[ExcludeFromCodeCoverage]
internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<TEntity>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        return _inner.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var returnValue = Execute(expression);
        return ConvertToThreadingTResult<TResult>(returnValue ?? throw new InvalidOperationException());
    }

    TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        var returnValue = ExecuteAsync<TResult>(expression, default);
        return ConvertToTResult<TResult>(returnValue ?? throw new InvalidOperationException());
    }

    private static TResult ConvertToTResult<TResult>(dynamic toConvert)
    {
        return (TResult)toConvert;
    }

    private static TResult ConvertToThreadingTResult<TResult>(dynamic toConvert)
    {
        return (TResult)Task.FromResult(toConvert);
    }
}