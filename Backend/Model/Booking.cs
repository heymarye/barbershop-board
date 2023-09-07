using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Model
{
    [Table("bookings")]
    public class Booking
    {
        [Column("id")] public int Id { get; init; }
        [Column("hairdresserId")] public int? HairdresserId { get; init; }
        [Column("serviceId")] public int? ServiceId { get; init; }
        [Column("specialOfferId")] public int? SpecialOfferId { get; init; }
        [Column("date")] public string? Date { get; init; }
        [Column("time")] public string? Time { get; init; }
        [JsonIgnore] public virtual Hairdresser? Hairdresser { get; init; }
        [JsonIgnore] public virtual Service? Service { get; init; }
        [JsonIgnore] public virtual SpecialOffer? SpecialOffer { get; init; }
    }
}