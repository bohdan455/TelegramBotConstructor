using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class TelegramAnswerPairModel
{
    [Required]
    public string Message { get; set; }

    [Required]
    public string Answer { get; set; }
    
    [Required]
    public bool Button { get; set; }
}