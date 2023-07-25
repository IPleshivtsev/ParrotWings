using ParrotWingsApi.Data.Models;

namespace ParrotWingsApi.Services.ApiService;

public interface IApiService
{
    #region Get
    /// <summary>
    /// Асинхронное получение пользователя по учетным данным
    /// </summary>
    /// <param name="accountData">Модель учетных данных</param>
    /// <returns>Task с моделью полученного пользователя</returns>
    Task<User> GetUserByAccountDataAsync(
        AccountData accountData);

    /// <summary>
    /// Асинхронное получение пользователя по идентификатору пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Task с моделью полученного пользователя</returns>
    Task<User> GetUserById(
        Guid id);

    /// <summary>
    /// Асинхронное получение списка пользователей по фильтру
    /// </summary>
    /// <param name="filter">Фильтр по имени пользователя</param>
    /// <returns>Task со списком пользователей</returns>
    Task<List<User>> GetUsersByFilterAsync(
        string filter);

    /// <summary>
    /// Асинхронное получение всех транзакций пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Task со списком транзакций пользователя</returns>
    Task<List<Transaction>> GetAllTransactionsByUserIdAsync(
        Guid userId);
    #endregion

    #region Create
    /// <summary>
    /// Асинхронное создание нового пользователя
    /// </summary>
    /// <param name="newUser">Модель нового пользователя</param>
    /// <returns>Task с моделью созданного пользователя</returns>
    Task<User> CreateUserAsync(
        User newUser);

    /// <summary>
    /// Асинхронное создание новой транзакции
    /// </summary>
    /// <param name="newTransaction">Модель новой транзакции</param>
    /// <returns>Task с моделью созданной транзакции</returns>
    Task<Transaction> CreateTransactionAsync(
        Transaction newTransaction);

    /// <summary>
    /// Асинхронное создание нового обращения
    /// </summary>
    /// <param name="newAppeal">Модель нового обращения</param>
    /// <returns>Task с моделью созданного обращения</returns>
    Task<Appeal> CreateAppealAsync(
        Appeal newAppeal);
    #endregion

    #region Check
    /// <summary>
    /// Асинхронная проверка на существование пользователя с указанной электронной почтой
    /// </summary>
    /// <param name="email">Электронная почта пользователя</param>
    /// <returns>Task с признаком существования пользователя с указанной электронной почтой</returns>
    Task<bool> IsUserExistsByEmailAsync(
        string email);

    /// <summary>
    /// Асинхронная проверка на существование пользователя с указанным именем
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <returns>Task с признаком существования пользователя с указанным именем</returns>
    Task<bool> IsUserExistsByNameAsync(
        string name);
    #endregion
}