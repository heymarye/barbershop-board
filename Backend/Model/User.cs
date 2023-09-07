using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model
{
    public class User
    {
        [Column("id")] public int Id { get; init; }

        [Column("username")] public string Username { get; init; } = "";

        [Column("password")] public string Password { get; init; } = "";
    }
}
