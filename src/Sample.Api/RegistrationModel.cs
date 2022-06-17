namespace Sample.Api;

using System.ComponentModel.DataAnnotations;


public class RegistrationModel
{
    [Required]
    public string EventId { get; set; } = null!;

    [Required]
    public string MemberId { get; set; } = null!;

    [Required]
    public decimal Payment { get; set; }
}