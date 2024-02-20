using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class TelegramAnswerPairModel
{
    [Required]
    public string Message { get; init; } = null!;

    [Required]
    public string Answer { get; init; } = null!;
    
    [Required]
    public bool Button { get; set; }

    public List<TelegramAnswerPairModel> Nested { get; set; } = [];
}