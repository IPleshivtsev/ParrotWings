namespace ParrotWingsApi.Services.LoggerService;

/// <summary>
/// Интерфейс сервиса логирования
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// Логирование сообщения ошибки
    /// </summary>
    /// <param name="message">Сообщение ошибки</param>
    void Error(string message);

    /// <summary>
    /// Логирование информационного сообщения
    /// </summary>
    /// <param name="message">Информационное сообщение</param>
    void Info(string message);

    /// <summary>
    /// Завершение логирования метода
    /// </summary>
    void EndLogMethod();
}