using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServeTogether.Models;

/// <summary>
/// A time block within a project, with a limit on how many helpers it needs.
/// </summary>
public class Slot
{
    public int Id { get; set; }

    public int ServiceProjectId { get; set; }
    public ServiceProject? ServiceProject { get; set; }

    [DataType(DataType.Time)]
    public DateTime StartTime { get; set; }

    [DataType(DataType.Time)]
    public DateTime EndTime { get; set; }

    [Range(1, 200)]
    public int Capacity { get; set; }

    public List<SignUp> SignUps { get; set; } = new();

    // Only accurate when SignUps has been loaded with .Include(s => s.SignUps)
    [NotMapped]
    public int SpotsLeft => Capacity - SignUps.Count;
}
