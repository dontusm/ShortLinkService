namespace ShortLinkService.Application.Dtos;

/// <summary>
/// DTO, представляющее информацию о сокращенной ссылке.
/// </summary>
public class UrlDto
{
    /// <summary>
    /// Уникальный идентификатор ссылки.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Полный оригинальный URL-адрес.
    /// </summary>
    public string LongUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Уникальный короткий код для доступа к ссылке.
    /// </summary>
    public string ShortCode { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата и время создания ссылки (в формате UTC).
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Общее количество переходов по данной ссылке.
    /// </summary>
    public int ClickCount { get; set; }
}