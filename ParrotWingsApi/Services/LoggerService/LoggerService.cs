namespace ParrotWingsApi.Services.LoggerService;

/// <summary>
/// Модель сервиса логирования
/// </summary>
public class LoggerService : ILoggerService
{
    /// <summary>
    /// Путь к выходному файлу
    /// </summary>
    public string _filePath;

    /// <summary>
    /// Инициализация логирования
    /// </summary>
    /// <param name="pathLog">Путь к папке с логами</param>
    public LoggerService(string pathLog)
    {
        if (!Directory.Exists(pathLog))
        {
            Directory.CreateDirectory(pathLog);
        }

        _filePath = pathLog + $"\\LogApi_{DateTime.Now.ToShortDateString()}.txt";
    }

    /// <summary>
    /// Логирование сообщения ошибки
    /// </summary>
    /// <param name="message">Сообщение ошибки</param>
    public void Error(string message)
    {
        LogMessage(
            string.Format("{0} Error: {1}\n",
            DateTime.Now.ToLongTimeString(), 
            message));
    }

    /// <summary>
    /// Логирование информационного сообщения
    /// </summary>
    /// <param name="message">Информационное сообщение</param>
    public void Info(string message)
    {
        LogMessage(string.Format(
            "{0} Info: {1}\n", 
            DateTime.Now.ToLongTimeString(),
            message));
    }

    /// <summary>
    /// Добавление сообщения в файл логирования
    /// </summary>
    /// <param name="message">Сообщение</param>
    private void LogMessage(string message)
    {
        File.AppendAllText(
            _filePath, 
            message);
    }

    /// <summary>
    /// Завершение логирования метода
    /// </summary>
    public void EndLogMethod()
    {
        LogMessage("\n");
    }
}