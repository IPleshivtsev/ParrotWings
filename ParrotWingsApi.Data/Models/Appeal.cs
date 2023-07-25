namespace ParrotWingsApi.Data.Models;

/// <summary>
/// Модель обращения
/// </summary>
public class Appeal
{
    /// <summary>
    /// Идентификатор обращения
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Дата обращения
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Тема обращения
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Текст обращения
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; }
}