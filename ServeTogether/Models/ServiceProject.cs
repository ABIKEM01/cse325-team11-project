using System.ComponentModel.DataAnnotations;
using ServeTogether.Data;

namespace ServeTogether.Models;

/// <summary>
/// A service project a leader posts, e.g. "Clean up the park on Saturday".
/// </summary>
public class ServiceProject
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(2000)]
    public string? Description { get; set; }

    [Required]
    [StringLength(200)]
    public string Location { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime Date { get; set; }

    // The leader who created the project
    public string CreatedById { get; set; } = string.Empty;
    public ApplicationUser? CreatedBy { get; set; }

    public List<Slot> Slots { get; set; } = new();
}
