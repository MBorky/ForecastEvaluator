using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForecastEvaluator.DataModels
{
    public class ForecastDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HourID { get; set; }
        public int ForecastID { get; set; }
        public TimeOnly Hour {  get; set; }
        public decimal? WindSpeed { get; set; }
        public decimal? WindGusts { get; set; }
        public int? WindDirection { get; set; }
        public WeatherForecast WeatherForecast { get; set; }
    }
}
