using Microsoft.EntityFrameworkCore;
using Moq;
using ParrotWingsApi.Context;
using ParrotWingsApi.Data.Models;
using ParrotWingsApi.Services.DataService;
using ParrotWingsApi.Test.Data;
using ParrotWingsApi.Services.ApiService;
using ParrotWingsApi.Data.Resource;

public class ApiServiceTests
{
    private readonly Mock<EntityContext> _context;
    private readonly Mock<IDbContextFactory<EntityContext>> _contextFactory;
    private readonly DbContextOptions<EntityContext> _contextOptions;
    private readonly IDataServiceProvider _dataServiceProvider;
    private readonly ApiService _apiService;

    public ApiServiceTests()
    {
        _contextOptions = new();
        _contextFactory = new();
        _context = new(_contextOptions);
        _dataServiceProvider = new DataServiceProvider(_contextFactory.Object);
        _apiService = new ApiService(_dataServiceProvider);
    }

    #region Get
    #region GetUserByAccountDataAsync
    [Fact]
    public async Task GetUserByAccountDataAsync_ArgumentException_LoginDataNullError_Email()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var accountData = new AccountData
        {
            Email = string.Empty,
            Password = users.First().Password
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .GetUserByAccountDataAsync(accountData));
        Assert.Equal(
            ConstStrings.LoginDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task GetUserByAccountDataAsync_ArgumentException_LoginDataNullError_Password()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var accountData = new AccountData
        {
            Email = users.First().Email,
            Password = string.Empty
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .GetUserByAccountDataAsync(accountData));
        Assert.Equal(
            ConstStrings.LoginDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task GetUserByAccountDataAsync_ArgumentException_UserEmailNotExistsError()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var accountData = new AccountData
        {
            Email = "test4@ya.ru",
            Password = users.First().Password
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .GetUserByAccountDataAsync(accountData));
        Assert.Equal(
            ConstStrings.UserEmailNotExistsError,
            exception.Message);
    }

    [Fact]
    public async Task GetUserByAccountDataAsync_ExistUser()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var accountData = new AccountData
        {
            Email = users.First().Email,
            Password = users.First().Password
        };

        var sut = await _apiService
            .GetUserByAccountDataAsync(accountData);

        Assert.Equal(users.First().Id, sut.Id);
        Assert.Equal(users.First().Name, sut.Name);
        Assert.Equal(users.First().Email, sut.Email);
        Assert.Equal(users.First().Balance, sut.Balance);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Between(2, 2, Moq.Range.Inclusive));
    }
    #endregion

    #region GetUserById
    [Fact]
    public async Task GetUserById_ArgumentException_UserIdNullError()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .GetUserById(Guid.Empty));
        Assert.Equal(
            ConstStrings.UserIdNullError,
            exception.Message);
    }

    [Fact]
    public async Task GetUserById_NullReferenceException_UserNotFound()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        NullReferenceException exception = await Assert
            .ThrowsAsync<NullReferenceException>(async () => await _apiService
            .GetUserById(Guid.NewGuid()));
        Assert.Equal(
            ConstStrings.UserNotFound,
            exception.Message);
    }

    [Fact]
    public async Task GetUserById_ExistUser()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var sut = await _apiService
            .GetUserById(users.First().Id.Value);

        Assert.Equal(users.First().Id, sut.Id);
        Assert.Equal(users.First().Name, sut.Name);
        Assert.Equal(users.First().Email, sut.Email);
        Assert.Equal(users.First().Balance, sut.Balance);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion

    #region GetUsersByFilterAsync
    [Fact]
    public async Task GetUsersByFilterAsync_NotExistUsers()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var filter = string.Empty;

        var sut = await _apiService
            .GetUsersByFilterAsync(filter);

        Assert.Equal(0, sut.Count());

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task GetUsersByFilterAsync_OneExistUsers()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var filter = "name2";

        var sut = await _apiService
            .GetUsersByFilterAsync(filter);

        Assert.Equal(1, sut.Count());
        Assert.Equal(
            true, 
            sut[0].Name.ToLower()
            .Contains(filter.ToLower()));

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetUsersByFilterAsync_ThreeExistUsers()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);
        var users = model.CreateModel(
            ModelCreator<User>.users);

        var filter = "name";

        var sut = await _apiService
            .GetUsersByFilterAsync(filter);

        Assert.Equal(3, sut.Count());
        
        foreach(var user in sut)
        {
            Assert.Equal(
                true,
                user.Name.ToLower()
                .Contains(filter.ToLower()));
        }

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion

    #region GetAllTransactionsByUserIdAsync
    [Fact]
    public async Task GetAllTransactionsByUserIdAsync_ArgumentException_UserIdNullError()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .GetAllTransactionsByUserIdAsync(Guid.Empty));
        Assert.Equal(
            ConstStrings.UserIdNullError,
            exception.Message);
    }

    [Fact]
    public async Task GetAllTransactionsByUserIdAsync_NotExistUserTransactions()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var userId = Guid.NewGuid();

        var sut = await _apiService
            .GetAllTransactionsByUserIdAsync(
            userId);

        Assert.Equal(0, sut.Count());

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllTransactionsByUserIdAsync_ExistUserTransactions()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var userId = users.First().Id.Value;

        var sut = await _apiService
            .GetAllTransactionsByUserIdAsync(
            userId);

        Assert.Equal(4, sut.Count());

        foreach (var transaction in sut)
        {
            Assert.Equal(
                true,
                transaction.SenderId == userId 
                || transaction.RecipientId == userId);
        }

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion
    #endregion

    #region Create
    #region CreateUserAsync
    [Fact]
    public async Task CreateUserAsync_ArgumentException_RegisterDataNullError_UserName()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var user = new User
        {
            Name = "",
            Email = "test4@ya.ru",
            Password = "111111"
        };
        
        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateUserAsync(user));
        Assert.Equal(
            ConstStrings.RegisterDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ArgumentException_RegisterDataNullError_UserEmail()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var user = new User
        {
            Name = "TestName4",
            Email = "",
            Password = "111111"
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateUserAsync(user));
        Assert.Equal(
            ConstStrings.RegisterDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ArgumentException_RegisterDataNullError_UserPassword()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var user = new User
        {
            Name = "TestName4",
            Email = "test4@ya.ru",
            Password = ""
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateUserAsync(user));
        Assert.Equal(
            ConstStrings.RegisterDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ArgumentException_UserNameExistsError()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var user = new User
        {
            Name = "TestName1",
            Email = "test4@ya.ru",
            Password = "111111"
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateUserAsync(user));
        Assert.Equal(
            ConstStrings.UserNameExistsError,
            exception.Message);
    }

    [Fact]
    public async Task CreateUserAsync_ArgumentException_UserEmailExistsError()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var user = new User
        {
            Name = "TestName4",
            Email = "test1@ya.ru",
            Password = "111111"
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateUserAsync(user));
        Assert.Equal(
            ConstStrings.UserEmailExistsError, 
            exception.Message);
    }

    [Fact]
    public async Task CreateUserAsync_CreatedUser()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var user = new User
        {
            Name = "TestName4",
            Email = "test4@ya.ru",
            Password = "111111"
        };

        var sut = await _apiService
            .CreateUserAsync(user);

        Assert.Equal(user, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Between(3, 3, Moq.Range.Inclusive));
        _context.Verify(x => x.AddAsync(It.IsAny<User>(), 
            It.IsAny<CancellationToken>()),
            Times.Once);
        _context.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion

    #region CreateTransactionAsync
    [Fact]
    public async Task CreateTransactionAsync_Exception_InsufficientFundsError()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var transaction = new Transaction
        {
            Amount = 1500000,
            SenderId = users.First().Id,
            RecipientId = users.Last().Id
        };

        Exception exception = await Assert
            .ThrowsAsync<Exception>(async () => await _apiService
            .CreateTransactionAsync(transaction));
        Assert.Equal(
            ConstStrings.InsufficientFundsError,
            exception.Message);
    }

    [Fact]
    public async Task CreateTransactionAsync_ArgumentException_CardNumberNullError()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var transaction = new Transaction
        {
            Amount = 500,
            RecipientId = users.Last().Id
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateTransactionAsync(transaction));
        Assert.Equal(
            ConstStrings.CardNumberNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateTransactionAsync_ArgumentException_RecipientNullError()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var transaction = new Transaction
        {
            Amount = 500,
            SenderId = users.First().Id
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateTransactionAsync(transaction));
        Assert.Equal(
            ConstStrings.RecipientNullError, 
            exception.Message);
    }

    [Fact]
    public async Task CreateTransactionAsync_CreatedTransactionBySender()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var transaction = new Transaction
        {
            Amount = 500,
            SenderId = users.First().Id,
            RecipientId = users.Last().Id,
        };

        var sut = await _apiService
            .CreateTransactionAsync(transaction);

        Assert.Equal(transaction, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
        _context.Verify(x => x.AddAsync(
            It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _context.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateTransactionAsync_CreatedTransactionByCardNumber()
    {
        var transactionModel = new ModelCreator<Transaction>(_context, _contextFactory);
        var userModel = new ModelCreator<User>(_context, _contextFactory);

        var transactions = transactionModel.CreateModel(
            ModelCreator<Transaction>.transactions);

        var users = userModel.CreateModel(
            ModelCreator<User>.users);

        var transaction = new Transaction
        {
            Amount = 500,
            TransferCardNumber = "1111111111111111",
            RecipientId = users.Last().Id,
        };

        var sut = await _apiService
            .CreateTransactionAsync(transaction);

        Assert.Equal(transaction, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
        _context.Verify(x => x.AddAsync(
            It.IsAny<Transaction>(), It.IsAny<CancellationToken>()),
            Times.Once);
        _context.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion

    #region CreateAppeal
    [Fact]
    public async Task CreateAppealAsync_ArgumentException_AppealDataNullError_Title()
    {
        var model = new ModelCreator<Appeal>(_context, _contextFactory);

        var appeals = model.CreateModel(
            ModelCreator<Appeal>.appeals);

        var appeal = new Appeal
        {
            Title = "",
            Message = "TestMessage4",
            Email = "testEmail4@ya.ru"
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateAppealAsync(appeal));
        Assert.Equal(
            ConstStrings.AppealDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateAppealAsync_ArgumentException_AppealDataNullError_Message()
    {
        var model = new ModelCreator<Appeal>(_context, _contextFactory);

        var appeals = model.CreateModel(
            ModelCreator<Appeal>.appeals);

        var appeal = new Appeal
        {
            Title = "TestTitle4",
            Message = "",
            Email = "testEmail4@ya.ru"
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateAppealAsync(appeal));
        Assert.Equal(
            ConstStrings.AppealDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateAppealAsync_ArgumentException_AppealDataNullError_Email()
    {
        var model = new ModelCreator<Appeal>(_context, _contextFactory);

        var appeals = model.CreateModel(
            ModelCreator<Appeal>.appeals);

        var appeal = new Appeal
        {
            Title = "TestTitle4",
            Message = "TestMessage4",
            Email = ""
        };

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .CreateAppealAsync(appeal));
        Assert.Equal(
            ConstStrings.AppealDataNullError,
            exception.Message);
    }

    [Fact]
    public async Task CreateAppealAsync_CreatedAppeal()
    {
        var model = new ModelCreator<Appeal>(_context, _contextFactory);

        var appeals = model.CreateModel(
            ModelCreator<Appeal>.appeals);

        var appeal = new Appeal
        {
            Title = "TestTitle4",
            Message = "TestMessage4",
            Email = "testEmail4@ya.ru"
        };

        var sut = await _apiService
            .CreateAppealAsync(appeal);

        Assert.Equal(appeal, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
        _context.Verify(x => x.AddAsync(
            It.IsAny<Appeal>(), It.IsAny<CancellationToken>()), 
            Times.Once);
        _context.Verify(x => x.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion
    #endregion

    #region Check
    #region IsUserExistsByEmailAsync
    [Fact]
    public async Task IsUserExistsByEmailAsync_ArgumentException_UserNameNullError()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .IsUserExistsByEmailAsync(string.Empty));
        Assert.Equal(
            ConstStrings.UserEmailNullError,
            exception.Message);
    }

    [Fact]
    public async Task IsUserExistsByEmailAsync_NotExistFlag()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var sut = await _apiService
            .IsUserExistsByEmailAsync("test5@ya.ru");

        Assert.Equal(false, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }

    [Fact]
    public async Task IsUserExistsByEmailAsync_ExistFlag()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var sut = await _apiService
            .IsUserExistsByEmailAsync("test1@ya.ru");

        Assert.Equal(true, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    #endregion

    #region IsUserExistsByNameAsync
    [Fact]
    public async Task IsUserExistsByNameAsync_ArgumentException_UserNameNullError()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        ArgumentException exception = await Assert
            .ThrowsAsync<ArgumentException>(async () => await _apiService
            .IsUserExistsByNameAsync(string.Empty));
        Assert.Equal(
            ConstStrings.UserNameNullError,
            exception.Message);
    }

    [Fact]
    public async Task IsUserExistsByNameAsync_NotExistFlag()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var sut = await _apiService
            .IsUserExistsByNameAsync("TestName4");

        Assert.Equal(false, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task IsUserExistsByNameAsync_ExistFlag()
    {
        var model = new ModelCreator<User>(_context, _contextFactory);

        var users = model.CreateModel(
            ModelCreator<User>.users);

        var sut = await _apiService
            .IsUserExistsByNameAsync("TestName1");

        Assert.Equal(true, sut);

        _contextFactory.Verify(x => x.CreateDbContextAsync(
            It.IsAny<CancellationToken>()),
            Times.Once);
    }
    #endregion
    #endregion
}