namespace _2.Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public DateTime Expires { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}