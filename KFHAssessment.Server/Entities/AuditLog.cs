namespace KFHAssessment.Server.Entities;

public class AuditLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public int? EntityId { get; set; }
    public string? Detail { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string? IpAddress { get; set; }
}