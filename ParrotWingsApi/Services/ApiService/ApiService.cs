using ParrotWingsApi.Context;
using ParrotWingsApi.Data.Models;
using ParrotWingsApi.Data.Resource;
using ParrotWingsApi.Services.DataService;
using System.Reflection;

namespace ParrotWingsApi.Services.ApiService;

public class ApiService : IApiService
{
    private IDataServiceProvider _dataServiceProvider;

    public ApiService(
        IDataServiceProvider dataServiceProvider)
    {
        _dataServiceProvider = dataServiceProvider;
    }

    #region Get
    /// <summary>
    /// Асинхронное получение пользователя по учетным данным
    /// </summary>
    /// <param name="accountData">Модель учетных данных</param>
    /// <returns>Task с моделью полученного пользователя</returns>
    public async Task<User> GetUserByAccountDataAsync(
        AccountData accountData)
    {
        if (accountData.Email == string.Empty
                || accountData.Password == string.Empty)
        {
            throw new ArgumentException(
                ConstStrings.LoginDataNullError);
        }

        if (!await _dataServiceProvider
            .IsUserExistsByEmailAsync(accountData.Email))
        {
            throw new ArgumentException(
                ConstStrings.UserEmailNotExistsError);
        }

        User user = await _dataServiceProvider
            .GetUserByAccountDataAsync(accountData);

        if (user == null)
        {
            throw new NullReferenceException(
                ConstStrings.LoginDataInvalid);
        }
        return user;
    }

    /// <summary>
    /// Асинхронное получение пользователя по идентификатору пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Task с моделью полученного пользователя</returns>
    public async Task<User> GetUserById(
        Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException(
                ConstStrings.UserIdNullError);
        }

        User user = await _dataServiceProvider
            .GetUserById(id);

        if (user == null)
        {
            throw new NullReferenceException(
                ConstStrings.UserNotFound);
        }
        return user;
    }

    /// <summary>
    /// Асинхронное получение списка пользователей по фильтру
    /// </summary>
    /// <param name="filter">Фильтр по имени пользователя</param>
    /// <returns>Task со списком пользователей</returns>
    public async Task<List<User>> GetUsersByFilterAsync(
        string filter)
    {
        List<User> userList = new List<User>();

        if (filter != string.Empty)
        {
            userList = await _dataServiceProvider
                .GetUsersByFilterAsync(filter);
        }
        return userList;
    }

    /// <summary>
    /// Асинхронное получение всех транзакций пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Task со списком транзакций пользователя</returns>
    public async Task<List<Transaction>> GetAllTransactionsByUserIdAsync(
        Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                ConstStrings.UserIdNullError);
        }
        return await _dataServiceProvider
            .GetAllTransactionsByUserIdAsync(userId);
    }
    #endregion

    #region Create
    /// <summary>
    /// Асинхронное создание нового пользователя
    /// </summary>
    /// <param name="newUser">Модель нового пользователя</param>
    /// <returns>Task с моделью созданного пользователя</returns>
    public async Task<User> CreateUserAsync(
        User newUser)
    {
        if (newUser.Name == string.Empty
            || newUser.Email == string.Empty
            || newUser.Password == string.Empty)
        {
            throw new ArgumentException(
                ConstStrings.RegisterDataNullError);
        }

        if (await _dataServiceProvider
            .IsUserExistsByNameAsync(newUser.Name))
        {
            throw new ArgumentException(
                ConstStrings.UserNameExistsError);
        }

        if (await _dataServiceProvider
            .IsUserExistsByEmailAsync(newUser.Email))
        {
            throw new ArgumentException(
                ConstStrings.UserEmailExistsError);
        }

        newUser.Balance = 500;
        await _dataServiceProvider.CreateUserAsync(newUser);
        return newUser;
    }

    /// <summary>
    /// Асинхронное создание новой транзакции
    /// </summary>
    /// <param name="newTransaction">Модель новой транзакции</param>
    /// <returns>Task с моделью созданной транзакции</returns>
    public async Task<Transaction> CreateTransactionAsync(
        Transaction newTransaction)
    {
        if (newTransaction.SenderId == null
            && newTransaction.TransferCardNumber == string.Empty)
        {
            throw new ArgumentException(
                ConstStrings.CardNumberNullError);
        }

        if (newTransaction.RecipientId == null)
        {
            throw new ArgumentException(
                ConstStrings.RecipientNullError);
        }

        await _dataServiceProvider
            .CreateTransactionAsync(newTransaction);
        return newTransaction;
    }

    /// <summary>
    /// Асинхронное создание нового обращения
    /// </summary>
    /// <param name="newAppeal">Модель нового обращения</param>
    /// <returns>Task с моделью созданного обращения</returns>
    public async Task<Appeal> CreateAppealAsync(
        Appeal newAppeal)
    {
        if (newAppeal.Title == string.Empty
            || newAppeal.Message == string.Empty
            || newAppeal.Email == string.Empty)
        {
            throw new ArgumentException(
                ConstStrings.AppealDataNullError);
        }

        await _dataServiceProvider
            .CreateAppealAsync(newAppeal);
        return newAppeal;
    }
    #endregion

    #region Check
    /// <summary>
    /// Асинхронная проверка на существование пользователя с указанной электронной почтой
    /// </summary>
    /// <param name="email">Электронная почта пользователя</param>
    /// <returns>Task с признаком существования пользователя с указанной электронной почтой</returns>
    public async Task<bool> IsUserExistsByEmailAsync(
        string email)
    {
        if (email == string.Empty)
        {
            throw new ArgumentException(
                ConstStrings.UserEmailNullError);
        }
        return await _dataServiceProvider
            .IsUserExistsByEmailAsync(email);
    }

    /// <summary>
    /// Асинхронная проверка на существование пользователя с указанным именем
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <returns>Task с признаком существования пользователя с указанным именем</returns>
    public async Task<bool> IsUserExistsByNameAsync(
        string name)
    {
        if (name == string.Empty)
        {
            throw new ArgumentException(
                ConstStrings.UserNameNullError);
        }
        return await _dataServiceProvider
            .IsUserExistsByNameAsync(name);
    }
    #endregion
}