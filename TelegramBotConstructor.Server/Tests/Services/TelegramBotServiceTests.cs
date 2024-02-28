using BLL.Services.Realizations;
using Domain.Models;

namespace Tests.Services;

public class TelegramBotServiceTests
{
    private readonly TelegramBotService _sut;

    public TelegramBotServiceTests()
    {
        _sut = new TelegramBotService(new FileStorageService(), new CodeWriterService());
    }

    [Fact]
    public void ShouldExtractAllNotNestedStates()
    {
        // Arrange
        var pairModels = new List<TelegramAnswerPairModel>
        {
            new TelegramAnswerPairModel
            {
                Message = "1",
                Answer = "1",
                Button = false,
                Nested =
                [
                    new TelegramAnswerPairModel
                    {
                        Message = "1.1",
                        Answer = "1.1",
                        Button = false
                    }
                ]
            },
            new TelegramAnswerPairModel
            {
                Message = "2",
                Answer = "2",
                Button = false,
                Nested =
                [
                    new TelegramAnswerPairModel
                    {
                        Message = "1.1",
                        Answer = "1.1",
                        Button = false
                    }
                ]
            },
            new TelegramAnswerPairModel
            {
                Message = "3",
                Answer = "3",
                Button = false
            }
        };
        
        const int expectedCount = 2;
        // Act
        var result = _sut.GetStates(pairModels);
        
        // Assert
        Assert.Equal(expectedCount, result.Count);

        for (var i = 1; i <= expectedCount; i++)
        {
            Assert.Contains(result, x => x.Id == i);
        }
    }
    
    [Fact]
    public void ShouldExtractAllNestedStates()
    {
        // Arrange
        var pairModels = new List<TelegramAnswerPairModel>
        {
            new TelegramAnswerPairModel
            {
                Message = "1",
                Answer = "1",
                Button = false,
                Nested =
                [
                    new TelegramAnswerPairModel
                    {
                        Message = "1.1",
                        Answer = "1.1",
                        Button = false,
                        Nested =
                        [
                            new TelegramAnswerPairModel
                            {
                                Message = "1.1.1",
                                Answer = "1.1.1",
                                Button = false,
                                Nested =
                                [
                                    new()
                                    {
                                        Answer = "1.1.1.1",
                                        Message = "1.1.1.1",
                                        Button = false,
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            new TelegramAnswerPairModel
            {
                Message = "2",
                Answer = "2",
                Button = false,
                Nested =
                [
                    new TelegramAnswerPairModel
                    {
                        Message = "1.1",
                        Answer = "1.1",
                        Button = false,
                        Nested =
                        [
                            new TelegramAnswerPairModel
                            {
                                Message = "1.1.1",
                                Answer = "1.1.1",
                                Button = false,
                                Nested =
                                [
                                    new()
                                    {
                                        Answer = "1.1.1.1",
                                        Message = "1.1.1.1",
                                        Button = false,
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            new TelegramAnswerPairModel
            {
                Message = "3",
                Answer = "3",
                Button = false
            }
        };
        
        const int expectedCount = 6;
        
        // Act
        var result = _sut.GetStates(pairModels);
        
        // Assert
        Assert.Equal(expectedCount, result.Count);
        
        for (var i = 1; i <= expectedCount; i++)
        {
            Assert.Contains(result, x => x.Id == i);
        }
    }
}