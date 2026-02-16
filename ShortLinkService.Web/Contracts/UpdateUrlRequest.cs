namespace ShortLinkService.Web.Contracts;

/// <summary>
/// Запрос на обновление существующей сокращенной ссылки.
/// </summary>
public class UpdateUrlRequest
{
    /// <summary>
    /// Идентификатор обновляемой ссылки
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Новый целевой URL-адрес, на который будет выполняться перенаправление.
    /// </summary>
    public string NewLongUrl { get; set; } = string.Empty;
}