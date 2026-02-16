using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShortLinkService.Application.Commands.CreateShortUrl; 
using ShortLinkService.Application.Commands.DeleteShortUrl; 
using ShortLinkService.Application.Commands.UpdateShortUrl;
using ShortLinkService.Application.Queries.GetUrlByCode;
using ShortLinkService.Application.Queries.GetUrlList;
using ShortLinkService.Web.Contracts;

namespace ShortLinkService.Web.Controllers;

/// <summary>
/// Главный контроллер управления сокращенными ссылками.
/// Обеспечивает как отдачу UI, так и API-интерфейс для операций CRUD.
/// </summary>
public class HomeController(IMediator mediator) : Controller
{
    /// <summary>
    /// Возвращает главную страницу приложения (SPA).
    /// </summary>
    [HttpGet("/")]
    public IActionResult Index()
    {
        // Почему PhysicalFile? Чтобы гарантированно отдать твой HTML из папки wwwroot
        return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/html");
    }

    /// <summary>
    /// Возвращает список всех созданных сокращенных ссылок.
    /// </summary>
    [HttpGet("/api/urls")]
    public async Task<IActionResult> GetUrls(CancellationToken ct)
    {
        var urls = await mediator.Send(new GetUrlListQuery(), ct);
        return Json(urls); // Возвращаем JSON для нашего JS
    }

    /// <summary>
    /// Создает новую сокращенную ссылку для указанного URL.
    /// Если ссылка уже существует, возвращает существующую (идемпотентность).
    /// </summary>
    [HttpPost("/api/shorten")]
    public async Task<IActionResult> Create([FromForm] string longUrl, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(longUrl) || !Uri.IsWellFormedUriString(longUrl, UriKind.Absolute))
            return BadRequest("Некорректный URL");

        await mediator.Send(new CreateShortUrlCommand(longUrl), ct);
        return Ok(); // JS получит статус 200 и обновит страницу
    }

    /// <summary>
    /// Обновляет целевой (длинный) URL для существующей записи.
    /// </summary>
    [HttpPost("/Home/Edit")]
    public async Task<IActionResult> Edit([FromForm] UpdateUrlRequest request, CancellationToken ct)
    {
        // Почему FromForm? JS отправляет данные через FormData
        await mediator.Send(new UpdateShortUrlCommand(request.Id, request.NewLongUrl), ct);
        return Ok();
    }

    /// <summary>
    /// Удаляет запись о сокращенной ссылке по её идентификатору.
    /// </summary>
    [HttpPost("/Home/Delete/{id}")]
    public async Task<IActionResult> Delete(long id, CancellationToken ct)
    {
        await mediator.Send(new DeleteShortUrlCommand(id), ct);
        return Ok();
    }

    /// <summary>
    /// Основной эндпоинт редиректа. Ищет длинный URL по короткому коду.
    /// </summary>
    /// /// <param name="code">Уникальный 7-символьный код ссылки.</param>
    [HttpGet("/{code}")]
    public async Task<IActionResult> RedirectTo(string code, CancellationToken ct)
    {
        var urlDto = await mediator.Send(new GetUrlByCodeQuery(code), ct);

        if (urlDto == null)
            return NotFound("Короткая ссылка не найдена.");
        
        return Redirect(urlDto.LongUrl);
    }
}