using ServeTogether.Data;

namespace ServeTogether.Models;

/// <summary>
/// One member signed up for one slot. A member can hold only one sign-up per slot
/// (enforced by a unique index in ApplicationDbContext).
/// </summary>
public class SignUp
{
    public int Id { get; set; }

    public int SlotId { get; set; }
    public Slot? Slot { get; set; }

    public string UserId { get; set; } = string.Empty;
    public ApplicationUser? User { get; set; }

    public DateTime SignedUpAt { get; set; } = DateTime.UtcNow;
}
