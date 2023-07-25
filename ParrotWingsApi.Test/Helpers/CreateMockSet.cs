using Microsoft.EntityFrameworkCore;
using Moq;

namespace ParrotWingsApi.Test.Helpers;

public class CreateMockSet<T> where T : class, new()
{
    public static Mock<DbSet<T>> MockSet(List<T> data)
    {
        var mockSet = new Mock<DbSet<T>>();
        var datas = data.AsQueryable();
        mockSet.As<IAsyncEnumerable<T>>()
            .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
            .Returns(new TestAsyncEnumerator<T>(
                datas.GetEnumerator()));

        mockSet.As<IQueryable<T>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<T>(
                datas.Provider));

        mockSet.As<IQueryable<T>>().Setup(m => m.Expression)
            .Returns(datas.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType)
            .Returns(datas.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
            .Returns(() => datas.GetEnumerator());
        return mockSet;
    }
}