using AutoMapper;
using Moq;
using ShortLinkService.Application.Commands.CreateShortUrl;
using ShortLinkService.Application.Common;
using ShortLinkService.Application.Dtos;
using ShortLinkService.Core.Entities;

namespace ShortLinkService.Tests.UnitTests;

[TestFixture]
public class CreateShortUrlTests
{
    private Mock<IUrlRepository> _repositoryMock;
    private Mock<IShortenerService> _shortenerMock;
    private Mock<IMapper> _mapperMock;
    private CreateShortUrlCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _repositoryMock = new Mock<IUrlRepository>();
        _shortenerMock = new Mock<IShortenerService>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CreateShortUrlCommandHandler(_repositoryMock.Object, _shortenerMock.Object, _mapperMock.Object);
    }

    [Test]
    public async Task Handle_WhenUrlIsNew_ShouldSaveAndReturnCode()
    {
        // Arrange
        var command = new CreateShortUrlCommand("https://google.com");
        var generatedCode = "7McS5Jq";
        
        _repositoryMock.Setup(r => r.GetByLongUrlAsync(command.LongUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Url)null);
            
        _shortenerMock.Setup(s => s.GenerateCode()).Returns(generatedCode);

        _mapperMock.Setup(m => m.Map<UrlDto>(It.IsAny<Url>()))
            .Returns(new UrlDto { ShortCode = generatedCode });
        
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Url>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.That(result.ShortCode, Is.EqualTo(generatedCode));
    }

    [Test]
    public async Task Handle_WhenUrlAlreadyExists_ShouldNotCreateNewOne()
    {
        // Arrange
        var command = new CreateShortUrlCommand("https://exists.com");
        var existingEntity = new Url { 
            Id = 10, 
            LongUrl = "https://exists.com", 
            ShortCode = "already" 
        };

        var expectedDto = new UrlDto { ShortCode = "already" };
        
        _mapperMock.Setup(m => m.Map<UrlDto>(existingEntity))
            .Returns(expectedDto);
        
        _repositoryMock.Setup(r => r.GetByLongUrlAsync(command.LongUrl, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Url>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.That(result.ShortCode, Is.EqualTo(existingEntity.ShortCode));
    }
}