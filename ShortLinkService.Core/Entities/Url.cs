namespace ShortLinkService.Core.Entities;

/// <summary>
/// Cущность соответствия между длинным URL и его сокращенной версией.
/// </summary>
public class Url
{
    /// <summary>
    /// Уникальный идентификатор записи (BigInt в БД).
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Оригинальный длинный URL.
    /// </summary>
    public string LongUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Сгенерированный уникальный код (7 символов).
    /// </summary>
    public string ShortCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата и время создания записи (UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Общее количество переходов по данной ссылке.
    /// </summary>
    public int ClickCount { get; set; }
}