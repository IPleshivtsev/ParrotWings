namespace ParrotWingsApi.Services.AuthorizationService;

/// <summary>
/// Интерфейс сервиса авторизации
/// </summary>
public interface IAuthorizationService
{
    /// <summary>
    /// Генерирование токена на основе электронной почты пользователя
    /// </summary>
    /// <param name="userId">Идентификатор пользователя</param>
    /// <returns>Сгенерированный токен</returns>
    string GenerateToken(Guid userId);
}