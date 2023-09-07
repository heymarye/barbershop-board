using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model
{
    [Table("services")]
    public class Service
    {
        [Column("id")] public int Id { get; init; }
        [Column("title")] public string? Title { get; init; }
        [Column("description")] public string? Description { get; init; }
        [Column("price")] public double? Price { get; init; }
        [Column("duration")] public string Duration { get; init; } = "";
    }
}