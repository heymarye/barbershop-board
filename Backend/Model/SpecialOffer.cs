using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Model
{
    [Table("specialOffers")]
    public class SpecialOffer
    {
        [Column("id")] public int Id { get; init; }
        [Column("serviceId")] public int? ServiceId { get; init; }
        [Column("code")] public string? Code { get; init; }
        [Column("percentage")] public double? Percentage { get; init; }
        [Column("fromDate")] public string? FromDate { get; init; }
        [Column("toDate")] public string? ToDate { get; init; }
        [JsonIgnore] public virtual Service? Service { get; init; }
    }
}