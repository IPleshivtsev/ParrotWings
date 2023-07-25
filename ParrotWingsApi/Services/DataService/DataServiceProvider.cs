using Microsoft.EntityFrameworkCore;
using ParrotWingsApi.Context;
using ParrotWingsApi.Data.Models;
using ParrotWingsApi.Data.Resource;

namespace ParrotWingsApi.Services.DataService;

public class DataServiceProvider : IDataServiceProvider
{
    private IDbContextFactory<EntityContext> _ctxFactory;

    public DataServiceProvider(
        IDbContextFactory<EntityContext> ctxFactory)
    {
        _ctxFactory = ctxFactory;
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
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            /// в будущем использовать FirstOrDefaultAsync (пока не стабилен)
            return ctx.Users
                .Where(x => x.Email == accountData.Email
                && x.Password == accountData.Password)
                .Select(c => new User
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Balance = c.Balance
                })
                .FirstOrDefault();
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }

    /// <summary>
    /// Асинхронное получение пользователя по идентификатору пользователя
    /// </summary>
    /// <param name="id">Идентификатор пользователя</param>
    /// <returns>Task с моделью полученного пользователя</returns>
    public async Task<User> GetUserById(
        Guid id)
    {
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            /// в будущем использовать FirstOrDefaultAsync (пока не стабилен)
            return ctx.Users
            .Where(x => x.Id == id)
            .Select(c => new User
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                Balance = c.Balance
            })
            .FirstOrDefault();
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }

    /// <summary>
    /// Асинхронное получение списка пользователей по фильтру
    /// </summary>
    /// <param name="filter">Фильтр по имени пользователя</param>
    /// <returns>Task со списком пользователей</returns>
    public async Task<List<User>> GetUsersByFilterAsync(
        string filter)
    {
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            return await ctx.Users
            .Where(x => x.Name.ToLower()
            .Contains(filter.ToLower()))
            .Select(c => new User {
                Id = c.Id, 
                Name = c.Name 
            })
            .ToListAsync();
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }

    /// <summary>
    /// Асинхронное получение всех транзакций пользователя по его идентификатору
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Task со списком транзакций пользователя</returns>
    public async Task<List<Transaction>> GetAllTransactionsByUserIdAsync(
        Guid userId)
    {
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Transactions != null)
        {
            return await ctx.Transactions
            .Where(c => c.SenderId == userId
            || c.RecipientId == userId)
            .Select(c => new Transaction
            {
                Id = c.Id,
                CreatedDate = c.CreatedDate,
                Amount = c.Amount,
                TransferCardNumber = c.TransferCardNumber,
                SenderId = c.SenderId,
                SenderName = c.SenderName,
                RecipientId = c.RecipientId,
                RecipientName = c.RecipientName
            })
            .ToListAsync();
        }
        throw new NullReferenceException(
                    ConstStrings.TransactionTableMissing);
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
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            await AddEntityToDb(ctx, newUser);
            return newUser;
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }

    /// <summary>
    /// Асинхронное создание новой транзакции
    /// </summary>
    /// <param name="newTransaction">Модель новой транзакции</param>
    /// <returns>Task с моделью созданной транзакции</returns>
    public async Task<Transaction> CreateTransactionAsync(
        Transaction newTransaction)
    {
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            if (ctx.Transactions != null)
            {
                User sender = new User();
                newTransaction.CreatedDate = DateTime.Now;

                if (newTransaction.SenderId != null)
                {
                    sender = await ctx.Users
                        .Where(c => c.Id == newTransaction.SenderId)
                        .FirstOrDefaultAsync();
                    int newBalance = sender.Balance - newTransaction.Amount;

                    if (newBalance < 0)
                    {
                        throw new Exception(
                            ConstStrings.InsufficientFundsError);
                    }

                    sender.Balance = newBalance;
                    await UpdateEntityToDb(ctx, sender, false);
                    newTransaction.SenderName = sender.Name;
                }

                User recipient = await ctx.Users
                    .Where(c => c.Id == newTransaction.RecipientId)
                    .FirstAsync();
                recipient.Balance = recipient.Balance + newTransaction.Amount;
                newTransaction.RecipientName = recipient.Name;

                await AddEntityToDb(ctx, newTransaction, false);
                await UpdateEntityToDb(ctx, recipient);
                return newTransaction;
            }
            throw new NullReferenceException(
                        ConstStrings.TransactionTableMissing);
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }

    /// <summary>
    /// Асинхронное создание нового обращения
    /// </summary>
    /// <param name="newAppeal">Модель нового обращения</param>
    /// <returns>Task с моделью созданного обращения</returns>
    public async Task<Appeal> CreateAppealAsync(
        Appeal newAppeal)
    {
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Appeals != null)
        {
            newAppeal.CreatedDate = DateTime.Now;
            await AddEntityToDb(ctx, newAppeal);
            return newAppeal;
        }
        throw new NullReferenceException(
                    ConstStrings.AppealTableMissing);
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
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            /// в будущем использовать FirstOrDefaultAsync (пока не стабилен)
            return ctx.Users
                .Where(x => x.Email == email)
                .FirstOrDefault() == null
            ? false
            : true;
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }

    /// <summary>
    /// Асинхронная проверка на существование пользователя с указанным именем
    /// </summary>
    /// <param name="name">Имя пользователя</param>
    /// <returns>Task с признаком существования пользователя с указанным именем</returns>
    public async Task<bool> IsUserExistsByNameAsync(
        string name)
    {
        var ctx = await _ctxFactory.CreateDbContextAsync();

        if (ctx.Users != null)
        {
            /// в будущем использовать FirstOrDefaultAsync (пока не стабилен)
            return ctx.Users
                .Where(x => x.Name == name)
                .FirstOrDefault() == null 
            ? false 
            : true;
        }
        throw new NullReferenceException(
                    ConstStrings.UserTableMissing);
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Асинхронное создание модели T в БД
    /// </summary>
    /// <typeparam name="T">Модель T</typeparam>
    /// <param name="_ctx">Контекст БД</param>
    /// <param name="entity">Объект модели T</param>
    /// <param name="isSave">Признак сохранения изменений</param>
    /// <returns>Возвращает Task с созданной моделью T</returns>
    private async Task<T> AddEntityToDb<T>(
        EntityContext _ctx,
        T entity,
        bool isSave = true) where T : class
    {
        _ = await _ctx.AddAsync(entity);

        if (isSave)
        {
            _ = await _ctx.SaveChangesAsync();
        }

        return entity;
    }

    /// <summary>
    /// Асинхронное обновление модели T в БД
    /// </summary>
    /// <typeparam name="T">Модель T</typeparam>
    /// <param name="_ctx">Контекст БД</param>
    /// <param name="entity">Объект модели T</param>
    /// <param name="isSave">Признак сохранения изменений</param>
    /// <returns>Возвращает Task с измененной моделью T</returns>
    private async Task<T> UpdateEntityToDb<T>(
        EntityContext _ctx,
        T entity,
        bool isSave = true) where T : class
    {
        _ = _ctx.Update(entity);

        if (isSave)
        {
            _ = await _ctx.SaveChangesAsync();
        }

        return entity;
    }
    #endregion
}