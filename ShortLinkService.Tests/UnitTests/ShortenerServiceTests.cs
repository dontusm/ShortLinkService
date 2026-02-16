using ShortLinkService.Infrastructure.Services;

namespace ShortLinkService.Tests.UnitTests;

    [TestFixture] 
    public class ShortenerServiceTests
    {
        private ShortenerService _sut;

        [SetUp] 
        public void SetUp()
        {
            _sut = new ShortenerService();
        }

        [Test]
        public void GenerateCode_ShouldOnlyContainAllowedCharacters()
        {
            // Arrange
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Act
            var result = _sut.GenerateCode();

            // Assert
            foreach (var c in result)
            {
                Assert.That(allowedChars, Does.Contain(c), $"Код содержит недопустимый символ: {c}");
            }
        }

        [Test]
        public void GenerateCode_ShouldBeThreadSafe()
        {
            // Arrange
            const int threadCount = 100;
            const int codesPerThread = 100;
            var allCodes = new System.Collections.Concurrent.ConcurrentBag<string>();

            // Act
            Parallel.For(0, threadCount, _ =>
            {
                for (var i = 0; i < codesPerThread; i++)
                {
                    allCodes.Add(_sut.GenerateCode());
                }
            });

            // Assert
            var uniqueCodes = new HashSet<string>(allCodes);
            Assert.That(uniqueCodes.Count, Is.EqualTo(threadCount * codesPerThread), 
                "При многопоточной генерации возникли дубликаты!");
        }
    }
