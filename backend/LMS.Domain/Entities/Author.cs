using LMS.Domain.Common;

namespace LMS.Domain.Entities;

public class Author : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Biography { get; set; }
    public string? Nationality { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Book> Books { get; set; } = new List<Book>();
}
