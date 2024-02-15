using BLL.Services.Interfaces;
using BLL.Services.Realizations;
using Domain.Models;

namespace Tests.Services;

public class CodeWriterServiceTests
{
    private readonly CodeWriterService _sut;
    private static readonly char[] CharArrayToIgnore = new[] { '\r', '\n', '\t' };

    public CodeWriterServiceTests()
    {
        _sut = new CodeWriterService();
    }
    
    [Fact]
    public void ShouldCreateSwitchStatement()
    {
        // Arrange
        var answerPair = new List<TelegramAnswerPairModel>
        {
            new TelegramAnswerPairModel
            {
                Message = "test1",
                Answer = "answer1"
            },
            new TelegramAnswerPairModel
            {
                Message = "test2",
                Answer = "answer2"
            }
        };
        
        // Act
        var result = _sut.CreateSwitchConstructor(answerPair);
        
        // Assert

        const string mustBeResult = """
            switch (message)
            {
            case "test1":
            answer = "answer1";
            break;
            case "test2":
            answer = "answer2";
            break;
            }
            """;
        AssertEqualIgnoringCharacters(mustBeResult, result, CharArrayToIgnore);
    }

    [Fact]
    public void ShouldCreateSwitchStatementWithDefaultCase()
    {
        // Arrange
        var answerPair = new List<TelegramAnswerPairModel>
        {
            new TelegramAnswerPairModel
            {
                Message = "test1",
                Answer = "answer1"
            },
            new TelegramAnswerPairModel
            {
                Message = "test2",
                Answer = "answer2"
            },
            new TelegramAnswerPairModel()
            {
                Message = "default",
                Answer = "defaultAnswer"
            }
        };
        
        // Act
        var result = _sut.CreateSwitchConstructor(answerPair);
        
        // Assert

        var mustBeResult = """
            switch (message)
            {
            case "test1":
            answer = "answer1";
            break;
            case "test2":
            answer = "answer2";
            break;
            default:
            answer = "defaultAnswer";
            break;
            }
            """;
        AssertEqualIgnoringCharacters(mustBeResult, result, CharArrayToIgnore);

    }
    
    private static void AssertEqualIgnoringCharacters(string expected, string actual, IEnumerable<char> charactersToIgnore)
    {
        foreach (var c in charactersToIgnore)
        {
            expected = expected.Replace(c.ToString(), "");
            actual = actual.Replace(c.ToString(), "");
        }
        Assert.Equal(expected, actual, ignoreAllWhiteSpace:true);
    }
}