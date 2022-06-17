namespace Sample.Components;

public class Registration
{
    public int Id { get; set; }

    public Guid RegistrationId { get; set; }
    public DateTime RegistrationDate { get; set; }

    public string MemberId { get; set; } = null!;
    public string EventId { get; set; } = null!;

    public decimal Payment { get; set; }
}