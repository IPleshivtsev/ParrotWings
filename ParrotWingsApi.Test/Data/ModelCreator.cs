using Microsoft.EntityFrameworkCore;
using Moq;
using ParrotWingsApi.Context;
using ParrotWingsApi.Data.Models;
using ParrotWingsApi.Test.Helpers;

namespace ParrotWingsApi.Test.Data;

internal class ModelCreator<T> where T : class, new()
{
    public readonly Mock<EntityContext> _context;
    public readonly Mock<IDbContextFactory<EntityContext>> _contextFactory;

    public ModelCreator(
        Mock<EntityContext> context,
        Mock<IDbContextFactory<EntityContext>> contextFactory)
    {
        _context = context;
        _contextFactory = contextFactory;
    }

    public static readonly List<User> users = new List<User>()
    {
        new User {
            Id = new Guid("2a082722-e05f-449b-a6f6-25867c20e1ea"),
            Name = "TestName1",
            Balance = 500,
            Email = "test1@ya.ru",
            Password = "111111"
        },
        new User {
            Id = new Guid("dbefc02a-5dba-4b61-a380-709bf14deec5"),
            Name = "TestName2",
            Balance = 600,
            Email = "test2@ya.ru",
            Password = "222222"
        },
        new User {
            Id = new Guid("11b5708a-efd5-453f-bd13-40c9fdd9ebbe"),
            Name = "TestName3",
            Balance = 700,
            Email = "test3@ya.ru",
            Password = "333333"
        },
    };

    public static readonly List<Transaction> transactions = new List<Transaction>()
    {
        new Transaction {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-5),
            SenderId = users[0].Id,
            SenderName = users[0].Name,
            RecipientId = users[1].Id,
            RecipientName = users[1].Name,
            Amount = 100,
            TransferCardNumber = "1111111111111111"
        },
        new Transaction {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-4),
            SenderId = users[0].Id,
            SenderName = users[0].Name,
            RecipientId = users[2].Id,
            RecipientName = users[2].Name,
            Amount = 200,
            TransferCardNumber = "1111111111111111"
        },
        new Transaction {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-3),
            SenderId = users[1].Id,
            SenderName = users[1].Name,
            RecipientId = users[2].Id,
            RecipientName = users[2].Name,
            Amount = 200,
            TransferCardNumber = "2222222222222222"
        },
        new Transaction {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-2),
            SenderId = users[1].Id,
            SenderName = users[1].Name,
            RecipientId = users[0].Id,
            RecipientName = users[0].Name,
            Amount = 300,
            TransferCardNumber = "2222222222222222"
        },
        new Transaction {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-1),
            SenderId = users[2].Id,
            SenderName = users[2].Name,
            RecipientId = users[0].Id,
            RecipientName = users[0].Name,
            Amount = 300,
            TransferCardNumber = "3333333333333333"
        },
        new Transaction {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            SenderId = users[2].Id,
            SenderName = users[2].Name,
            RecipientId = users[1].Id,
            RecipientName = users[1].Name,
            Amount = 400,
            TransferCardNumber = "3333333333333333"
        },
    };

    public static readonly List<Appeal> appeals = new List<Appeal>()
    {
        new Appeal {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-10),
            Title = "TestTitle1",
            Message = "TestMessage1",
            Email = "testEmail1@ya.ru"
        },
        new Appeal {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-8),
            Title = "TestTitle2",
            Message = "TestMessage2",
            Email = "testEmail2@ya.ru"
        },
        new Appeal {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now.AddDays(-6),
            Title = "TestTitle3",
            Message = "TestMessage3",
            Email = "testEmail3@ya.ru"
        },
    };

    public IQueryable<T> CreateModel(
        List<T> data)
    {
        var datas = data.AsQueryable();
        var mockSet = CreateMockSet<T>.MockSet(data);

        if (mockSet.Object is DbSet<User>)
            _ = _context
                .Setup(x => x.Users)
                .Returns(mockSet.Object as DbSet<User>);
        if (mockSet.Object is DbSet<Transaction>)
            _ = _context
                .Setup(x => x.Transactions)
                .Returns(mockSet.Object as DbSet<Transaction>);
        if (mockSet.Object is DbSet<Appeal>)
            _ = _context
                .Setup(x => x.Appeals)
                .Returns(mockSet.Object as DbSet<Appeal>);

        _ = _contextFactory
            .Setup(x => x.CreateDbContextAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(_context.Object));

        return datas;
    }
}