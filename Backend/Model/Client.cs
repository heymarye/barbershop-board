using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model
{
    [Table("clients")]
    public class Client
    {
        [Column("id")] public int Id { get; init; }
        [Column("firstName")] public string? FirstName { get; init; }
        [Column("lastName")] public string? LastName { get; init; }
        [Column("mail")] public string? Mail { get; init; }
        [Column("address")] public string? Address { get; init; }
        [Column("mobile")] public string? Mobile { get; init; }
    }
}