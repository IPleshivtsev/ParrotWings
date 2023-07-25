using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParrotWingsApi.Data.Models;
using ParrotWingsApi.Data.Resource;
using ParrotWingsApi.Services.ApiService;
using ParrotWingsApi.Services.LoggerService;
using System.Security.Claims;
using IAuthorizationService = ParrotWingsApi.Services.AuthorizationService.IAuthorizationService;

namespace ParrotWingsApi.Controllers;

[Route("api/pw")]
[ApiController]
public class ApiController : ControllerBase
{
    private IAuthorizationService _authorizationService;
    private IApiService _apiService;
    private ILoggerService _loggerService;

    /// <summary>
    /// Инициализация нового экземпляра класса
    /// </summary>
    /// <param name="dataServiceProvider">Сервис работы с БД</param>
    /// <param name="loggerService">Сервис логирования</param>
    public ApiController(
        IAuthorizationService authorizationService,
        IApiService apiService,
        ILoggerService loggerService)
    {
        _authorizationService = authorizationService;
        _apiService = apiService;
        _loggerService = loggerService;
    }

    ///// <summary>
    ///// Асинхронная авторизация пользователя
    ///// </summary>
    ///// <param name="loginModel">Модель авторизации</param>
    ///// <returns>Task с токеном авторизации пользователя</returns>
    [HttpPost("Login")]
    public async Task<IActionResult> Login(
        [FromBody] AccountData accountData)
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            accountData);

        try
        {
            User user = await _apiService
                .GetUserByAccountDataAsync(accountData);
            string token = GetToken(user.Id.Value);
            LogEndInfo(token);
            return Ok(token);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Асинхронное получение данных текущего пользователя
    /// </summary>
    /// <returns>Task с моделью данных текущего пользователя</returns>
    [Authorize]
    [HttpGet]
    [Route("GetCurrentUserData")]
    public async Task<IActionResult> GetCurrentUserData()
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            string.Empty);

        try
        {
            User user = await _apiService
                .GetUserById(GetUserIdFromHeaders().Value);
            LogEndInfo(user);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Асинхронная регистрация пользователя
    /// </summary>
    /// <param name="newUser">Модель нового пользователя</param>
    /// <returns>Task с токеном авторизации нового пользователя</returns>
    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register(
        [FromBody] User newUser)
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            newUser);

        try
        {
            await _apiService.CreateUserAsync(newUser);

            if (newUser.Id == null)
            {
                throw new NullReferenceException(
                    ConstStrings.UserSaveError);
            }

            string token = GetToken(newUser.Id.Value);
            LogEndInfo(token);
            return Ok(token);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Асинхронное получение списка пользователей по фильтру
    /// </summary>
    /// <param name="filter">Фильтр по имени пользователя</param>
    /// <returns>Task со списком пользователей</returns>
    [Authorize]
    [HttpGet]
    [Route("GetUsersByFilter")]
    public async Task<IActionResult> GetUsersByFilter(
        string filter = "")
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            filter);

        try
        {
            List<User> userList = await _apiService
                .GetUsersByFilterAsync(filter);
            LogEndInfo(userList);
            return Ok(userList);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Асинхронное создание новой транзакции
    /// </summary>
    /// <param name="newTransaction">Модель новой транзакции</param>
    /// <returns>Task с моделью созданной транзакции</returns>
    [Authorize]
    [HttpPost]
    [Route("CreateTransaction")]
    public async Task<IActionResult> CreateTransaction(
        [FromBody] Transaction newTransaction)
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            newTransaction);

        try
        {
            await _apiService.CreateTransactionAsync(newTransaction);

            if (newTransaction.Id == null)
            {
                throw new NullReferenceException(
                    ConstStrings.TransactionSaveError);
            }

            LogEndInfo(newTransaction);
            return Ok(newTransaction);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Асинхронное получение всех транзакций пользователя
    /// </summary>
    /// <returns>Task со списком транзакций</returns>
    [Authorize]
    [HttpGet]
    [Route("GetUserTransactions")]
    public async Task<IActionResult> GetUserTransactions()
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            string.Empty);

        try
        {
            List<Transaction> userTransactions = await _apiService
                .GetAllTransactionsByUserIdAsync(GetUserIdFromHeaders().Value);
            LogEndInfo(userTransactions);
            return Ok(userTransactions);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Асинхронное создание нового обращения
    /// </summary>
    /// <param name="newAppeal">Модель нового обращения</param>
    /// <returns>Task с признаком удачного сохранения</returns>
    [HttpPost]
    [Route("CreateAppeal")]
    public async Task<IActionResult> CreateAppeal(
        [FromBody] Appeal newAppeal)
    {
        LogStartInfo(
            ControllerContext.RouteData.Values["action"].ToString(),
            newAppeal);

        try
        {
            await _apiService.CreateAppealAsync(newAppeal);

            if (newAppeal.Id == null)
            {
                throw new NullReferenceException(
                    ConstStrings.AppealSaveError);
            }

            LogEndInfo(newAppeal);
            return Ok(newAppeal);
        }
        catch (Exception ex)
        {
            _loggerService.Error(ex.Message);
            _loggerService.EndLogMethod();
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение идентификатора пользователя из заголовков
    /// </summary>
    /// <returns>Идентификатор пользователя</returns>
    public Guid? GetUserIdFromHeaders()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;

        if (identity != null)
        {
            Guid userId = new Guid(identity.Claims
                .Where(x => x.Type == "UserId")
                .Select(x => x.Value)
                .FirstOrDefault());

            if (userId == Guid.Empty)
            {
                throw new NullReferenceException(
                    ConstStrings.UnknownError401);
            }
            return userId;
        }
        throw new NullReferenceException(
            ConstStrings.UnknownError401);
    }

    /// <summary>
    /// Получение идентификатора пользователя из заголовков
    /// </summary>
    /// <returns>Идентификатор пользователя</returns>
    public string GetToken(Guid id)
    {
        string token = _authorizationService
                .GenerateToken(id);

        if (token == null)
        {
            throw new NullReferenceException(
                ConstStrings.GenerateTokenError);
        }
        return token;
    }

    /// <summary>
    /// Логирование начальной информации
    /// </summary>
    /// <param name="methodName">Название метода</param>
    /// <param name="data">Отправляемые данные</param>
    public void LogStartInfo(
            string methodName,
            object data)
    {
        _loggerService.Info(string.Format(
            "Starting... FileName: {0}. Method: {1}.",
            GetType().Name, methodName));
        _loggerService.Info(string.Format(
            "      Data: {0}",
            JsonConvert.SerializeObject(data)));
    }

    /// <summary>
    /// Логирование конечной информации
    /// </summary>
    /// <param name="methodName">Название метода</param>
    /// <param name="data">Отправляемые данные</param>
    public void LogEndInfo(
        object data)
    {
        _loggerService.Info(string.Format(
                "End. Return: {0}",
                JsonConvert.SerializeObject(data)));
        _loggerService.EndLogMethod();
    }
}