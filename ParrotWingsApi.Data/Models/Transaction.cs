namespace ParrotWingsApi.Data.Models;

/// <summary>
/// Модель транзакции
/// </summary>
public class Transaction
{
    /// <summary>
    /// Идентификатор транзакции
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Дата транзакции
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Идентификатор отправителя
    /// </summary>
    public Guid? SenderId { get; set; }

    /// <summary>
    /// Имя отправителя
    /// </summary>
    public string SenderName { get; set; }

    /// <summary>
    /// Идентификатор получателя
    /// </summary>
    public Guid? RecipientId { get; set; }

    /// <summary>
    /// Имя получателя
    /// </summary>
    public string RecipientName { get; set; }

    /// <summary>
    /// Сумма транзакции
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    /// Номер карты отправителя
    /// </summary>
    public string TransferCardNumber { get; set; } = string.Empty;
}