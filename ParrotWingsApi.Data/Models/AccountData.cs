namespace ParrotWingsApi.Data.Models;

/// <summary>
/// Модель учетных данных
/// </summary>
public class AccountData
{
    /// <summary>
    /// Электронная почта пользователя
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }
}